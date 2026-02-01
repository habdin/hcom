//vim:foldmethod=indent:ts=2

// =======================================================================================
// constants
// =======================================================================================

const isDevelopment = document.getElementById('debugState').value === 'true';
// const isDevelopment = false;

// =======================================================================================
// Class declarations
// =======================================================================================

export class ComponentInitializer {
	elementInitializer({ component, elementName, elementId, customLogic }) {
		// Check if the component is there and if not initialize it
		if (!this[component]) {
			const element = document.getElementById(elementId);
			if (element) {
				// Log the success of the element initialization when the app is in development state
				if (typeof customLogic === 'function') {
					const result = customLogic(element);
					this[component] = result || element;
				} else {
					this[component] = element;
				}
				if (isDevelopment) {
					console.log(`${elementName} was successfully initialized.`);
				}
			} else {
				if (isDevelopment) {
					console.warn(`${elementName} is not present in this page.`);
				}
				return null;
			}
		}
		return this[component];
	}

	/**
	 * Batch initialize a list of components.
	 *
	 * @param {Array<Object>} configs - List of initialization configs for multiple components.
	 * @param {string} configs[].component - The name for the component to be initialized.
	 * @param {string} configs[].elementName - The name or tag of the component used in the DOM (e.g., 'div', 'button').
	 * @param {string} configs[].elementID - The DOM ID of the element to initialize (e.g., 'component1').
	 * @param {function} [configs[].customLogic] - Optional function for additional customization or initialization logic.
	 *  If not provided, the element is initialized with default behavior.
	 */
	batchInitialize(configs) {
		configs.forEach(config => this.elementInitializer(config));
	}
}


export class CRUDSManager {
	/**
	 * CRUDSManager class to manage CRUD operations. It uses internally the Modal and Viewtype_Value.
	 * 
	 * @class CRUDSManager
	 * @constructor
	 * 
	 * @param {string} baseURL - The base URL for the CRUD operations.
	 * 
	 * @property {string} baseURL - The base URL for making API requests.
	 * @property {Object} Modal - The Bootstrap modal object initialized for the application.
	 * @property {string} Viewtype_Value - The current value of the Viewtype field.
	 * 
	 * @example
	 * // Example usage:
	 * const crudsManager = new CRUDSManager("/api/v1");
	 * 
	 */
	constructor(baseURL) {
		this.baseURL = baseURL;
		this.currentId = null;
		const CompInit = new ComponentInitializer();
		// This line is debugging the initialization of the class using the constructor method.
		if (isDevelopment) {
			console.log(`CRUDSManager initialized with base URL: ${this.baseURL}`);
		}

		const components = [
			{
				component: "Modal", elementId: "theModal", elementName: "Modal",
				customLogic: (element) => new bootstrap.Modal(element)
			},
			{
				component: "Viewtype", elementId: "viewtype", elementName: "Viewtype field",
				customLogic: (element) => {
					this.Viewtype_Value = element.value.toLowerCase();
				}
			}
		];
		// BatchInitialize Componets

		CompInit.batchInitialize(components);

		// The Modal Submit should be explicitly initialized to be further used in Modal submission events
		if (this.Modal) {
			this.M_Submit = document.getElementById("theModalButtonSubmit");
			this.Modal.hide = this.Modal.hide.bind(this.Modal);
		}
	}


	/**
	 * Tweaks the modal's parts (title, buttons) based on the provided options.
	 * Initializes and adjusts the modal title, submit button label, and shows or hides the cancel button.
	 *
	 * @param {Object} [options={}] - An object containing options to customize the modal.;
	 * @param {string} [options.title=""] - The title to set for the modal. Defaults to an empty string.
	 * @param {string} [options.btn1_name=""] - The label for the submit button. Defaults to an empty string.
	 * @param {boolean} [options.show_btn2=true] - Determines whether the cancel button should be displayed. Defaults to true.
	 *
	 * @throws {TypeError} If the `title` or `btn1_name` parameters are not strings.
	 *
	 * @example
	 * const crudManager = new CRUDSManager(baseUrl);
	 * crudManager.tweakModalParts({
	 *   title: "Delete Confirmation",
	 *   btn1_name: "Delete",
	 *   show_btn2: false
	 * });
	 */
	modalPartsTweaker({ title = "", btn1_name = "", show_btn2 = true } = {}) {
		const M_Title = document.getElementById("theModalTitle");
		const M_Cancel = document.getElementById("theModalButtonCancel");
		const M_Submit = document.getElementById("theModalButtonSubmit");
		// Ensure the parameters are of correct type
		if (typeof title !== "string" || typeof btn1_name !== "string") {
			console.error("Invalid parameter type.");
			return;
		}
		// Show or hide the Cancel button
		show_btn2 ? $(M_Cancel).show() : $(M_Cancel).hide();
		// Tweak the title of the Modal
		$(M_Title).text(title);

		// Tweak the title of the Modal
		$(M_Submit).val(btn1_name);
	}

	/**
	 * Loads content into the modal body from the specified URL.
	 *
	 * Notes on `this` context:
	 * - Arrow functions ensure `this` refers to the CRUDSManager instance.
	 * - Avoid using `function` here, as it would rebind `this` to the `load` context.
	 * 
	 * @param {string} url - The URL to load content from. 
	 **/
	modalBodyLoader(url = "") {
		const M_Body = document.getElementById("theModalBody");
		// Ensure the URL is a valid toString
		if (typeof url !== "string") {
			console.error("URL parameter should be provided as string.");
			return;
		}
		// Clear previous content in the modal body.
		$(M_Body).empty();

		// Load new content into M_Body
		$(M_Body).load(url, (status) => {
			if (status === "error") {
				console.error("Failed to load content from URL:", url);
				return;
			}
			this.Modal.show();
		});
	}

	/**
	 * Extracts the integer part from the element id having the form: "actionType-intID".
	 *
	 * @param {string} element - The element enclosing the target id.
	 */
	elementIntIdgrabber(element) {
		// Reset the currentId before attempting to extract a new one.
		this.resetId();

		// Ensure the element has an id and parse it to extract the integer part
		if (element?.id) {
			const intId = parseInt(element.id.split("-").pop(), 10);
			// Set the currentId if the parsed value is a valid number, otherwise null
			this.currentId = isNaN(intId) ? null : intId;
		}

		// Return the currentId, which is either the parsed ID or null
		return this.currentId;
	}


	// TODO: Investigate how DataTables updates key elements (e.g., total records, filtered records, pagination)
	// during table.draw() and table.ajax.reload() in server-side processing mode.
	// Focus on identifying:
	// 1. Which parts of the table are dynamically updated.
	// 2. How to trigger updates to total/filtered counts without manually refreshing the page.

	// TODO: Refactor submitSuccessHandler to handle both cardview and tableview appropriately.
	// Goals:
	// 1. Separate and centralize logic for cardview (using page refresh) and tableview (using table.draw()).
	// 2. Ensure modularity and reusability of success handlers for future CRUD operations.
	// 3. Revisit and optimize once DataTables internals are better understood.
	modalFormSubmitHandler(event, form, url) {
		event.preventDefault();
		if ($(form).valid()) {
			const Form_Data = $(form).serialize();
			$.ajax({
				type: "POST",
				data: Form_Data,
				url: url,
				success: window.location.reload(),
			});
			this.Modal.hide();
		}
		else console.error("Please be sure the form entries are complete and adequate.");
	}


	/**
	 * Resets the currentId to null.
	 * 
	 * This method is used to clear the stored ID (if any) to ensure a clean state 
	 * before a new ID is processed or fetched. It helps maintain the integrity 
	 * of the `currentId` and prevents accidental reuse of an old ID.
	 */
	resetId() {
		this.currentId = null;
	}

	create({ title, btn1_name, show_btn2, url, form } = {}) {
		// console.log("Creating new Element.");
		this.modalPartsTweaker({ title: title, btn1_name: btn1_name, show_btn2: show_btn2 });
		this.modalBodyLoader(url);
		$(this.M_Submit).off("click").click((event) => {
			const Ensured_Form = typeof form === "function" ? form() : form;
			this.modalFormSubmitHandler(event, Ensured_Form, url);
		});
	}

	read({ title, btn1_name, show_btn2, url } = {}) {
		// console.log("Read Element details.");
		this.modalPartsTweaker({ title: title, btn1_name: btn1_name, show_btn2: show_btn2 });
		this.modalBodyLoader(url);
		$(this.M_Submit).off("click").click(this.Modal.hide);
	}

	update({ title, btn1_name, show_btn2, url, form } = {}) {
		// console.log("Updating the current element.");
		this.modalPartsTweaker({ title: title, btn1_name: btn1_name, show_btn2: show_btn2 });
		this.modalBodyLoader(url);
		$(this.M_Submit).off("click").click((event) => {
			const Ensured_Form = typeof form === "function" ? form() : form;
			this.modalFormSubmitHandler(event, Ensured_Form, url);
		});
	}

	delete({ title, btn1_name, show_btn2, url, form } = {}) {
		// console.log("Deleting the current element.");
		this.modalPartsTweaker({ title: title, btn1_name: btn1_name, show_btn2: show_btn2 });
		this.modalBodyLoader(url);
		$(this.M_Submit).off("click").click((event) => {
			const Ensured_Form = typeof form === "function" ? form() : form;
			this.modalFormSubmitHandler(event, Ensured_Form, url);
		});
	}

	search(searchTerm, url) {
		// console.log("The search results will be here soon.");
		console.log("The search term is:", searchTerm);
		// Check the search string format and exit the function is the search term is not a string.
		if (typeof searchTerm !== "string") {
			console.error("Invalid search parameter: Expected a string.");
			return;
		}
		$.ajax({
			type: "GET",
			url: url,
			data: { search: searchTerm },
			success: function (response) {
				let target = document.getElementById("cardView");
				target.innerHTML = response;
			},
		});
	}
}

// =======================================================================================
// Function declarations
// =======================================================================================

/**
 * Handles Read, Update, and Delete events for a container.
 *
 * @param {string} containerId - The ID of the container where events will be captured.
 * @param {object} options - Contains URLs and configurations for CRUD operations.
 * @param {object} manager - Instance of CRUDSManager to handle operations.
 */
export function handleRUDevents(containerId, options, manager) {
	// Check if the manager instance is provided; it is essential for the function to work
	if (!manager) {
		console.error("Manager instance is required but not provided.");
		return;
	}
	// Destructure options for easier access
	const { readOptions, updateOptions, deleteOptions } = options;

	// Locate the container element by its ID
	const container = document.getElementById(containerId);

	// Handle case where the container is not found
	if (!container) {
		console.error(`Container with Id "${containerId}" not found.`);
		return;
	}

	// Add a click event listener to the container to handle delegated events.
	container.addEventListener("click", (event) => {
		// Identify the closest target with an appropriate ID pattern
		const target = event.target.closest("[id^='dataItemDetail-'], [id^='dataItemUpdate-'], [id^='dataItemDelete-']");

		if (target) event.preventDefault();


		// Exit if no valid target is clicked
		if (!target) return;
		try {
			// Extract the integer ID using the Manager's method
			const intId = manager.elementIntIdgrabber(target);

			// Determine the action type (Detail, Update, Delete) from the target's ID
			const actionType = target.id.split("-")[0];

			let url; // Initialize the URL variable
			switch (actionType) {
			case "dataItemDetail":
				url = `${readOptions.url}${intId}/`;
				manager.read({ ...readOptions, url });
				break;

			case "dataItemUpdate":
				url = `${updateOptions.url}${intId}/`;
				manager.update({ ...updateOptions, url });
				break;

			case "dataItemDelete":
				url = `${deleteOptions.url}${intId}/`;
				manager.delete({ ...deleteOptions, url });
				break;
			}
		} catch (error) {
			// Handle and log errors gracefully
			console.error(`Error processing event for target ID ${target.id}`, error);
		}
	});
}
