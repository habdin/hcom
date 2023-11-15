// vim:foldmethod=indent:ts=4

const Modal = new bootstrap.Modal('#theModal');

function showDataAsCard() {
	$('#tableView').empty();
	window.location.reload();
	// By default the #AddRecordButtonDiv will revert to the default after reloading the page, so no
	// need to put the statement that I inserted in the showDataAsTable that changes the target <div>
}

function showDataAsTable() {
	$('#AddRecordButtonDiv').html(
	    `
	    	<button class="btn btn-primary mt-2 mb-3" title="Add New Record" 
        		onclick="CreateDataEntryDt()">
        		<i class="fas fa-plus-square"></i>
	    	</button>
	    `
	);
	$('#tableView').html(
	    `
				<table class="table table-bordered table-striped table-hover mt-3" id="theTable">
		    	<thead class="table-primary">
						<tr> 
			    		<th>ID</th>
			    		<th>Name</th>
			    		<th>Category</th>
			    		<th>Actions</th>
						</tr>
		    	</thead>
			</table> 
	    `
	);
	$('#theTable').DataTable({
		processing: true,
		serverSide: true,
		autoWidth: false,
		ajax: {
			datatype: 'json',
			url: '/dummy/dummy-list'
		},
		columns: [
		    { "data": "id" },
		    { "render":
		      function(data, type, row) {
			  return `<a class="text-decoration-none" href="#" onclick="ReadDataEntry(${row.id})">${row.name}</a>`;
		      }
		    },
		    { "data": "category" },
		    { "render":
		      function(data, type, row) {
			  return `<div class="btn-group" role="group">
<a class="btn btn-sm btn-success" type="button" title="Edit ${row.name}" href="#" onclick="UpdateDataEntryDt(${row.id})"><i class="fas fa-edit"></i></a>
<a class="btn btn-sm btn-danger" type="button" title="Delete ${row.name}" href="#" onclick="DeleteDataEntryDt(${row.id})"><i class="fas fa-trash"></i></a></div>`
		      }
		    },
		]
	});
	$('#cardView').empty().hide();
}

// CRUD operations for card style data display

function CreateDataEntry() {
	// ------ Set the function variables ------
	let url = '/dummy/add/';

	// ----- Statements -----

	// Change the name of modal title and buttons
	$("#theModalTitle").text("Add Dummy");
	$("#theModalButtonCancel").show();
	$("#theModalButtonSubmit").val("Save");

	/* load the user creation form into the modal body then
			 load the modal.
			*/
	$('#theModalBody').load(url, function() {
		Modal.show();
	});

	// Add functionality to the Modal Submit Button
	$('#theModalButtonSubmit').click(function() {
		// Collect the data from the form and transform the data into JSON
		let FormData = $("#DummyForm").serialize();
		// Send the Json via Ajax to the server for processing
		$.ajax({
			type: "POST",
			data: FormData,
			url: url,
			success: function() {
				Modal.hide();
				window.location.href = '/dummy/';
				$("#theModalButtonSubmit").off('click');
			}
		});
	});
}

function ReadDataEntry(id) {
	// ----- Set the function variables -----
	let url = `/dummy/${id}/`;
	// ----- Statements -----
	// Change the name of modal title and buttons
	$('#theModalTitle').text('Dummy Detail');
	$('#theModalButtonCancel').hide();
	$('#theModalButtonSubmit').val('Ok');

	/* load the user creation form into the modal body then
			 load the modal.
			*/
	$('#theModalBody').load(url, function() {
		Modal.show();
	});

	// Add functionality to the Modal Submit Button
	$("#theModalButtonSubmit").click(function() {
		$("#theModalButtonSubmit").off('click');
		$("#theModalButtonCancel").show(1000);
		Modal.hide();
	});
}

function UpdateDataEntry(id) {
	// ----- Set the function variables -----
	let url = `/dummy/edit/${id}/`;

	// ----- Statements -----
	// Change the name of modal title and buttons
	$("#theModalTitle").text("Edit Dummy");
	$("#theModalButtonCancel").show();
	$("#theModalButtonSubmit").val("Update");

	/* load the user creation form into the modal body then
			 load the modal.
			*/
	$('#theModalBody').load(url, function() {
		Modal.show();
	});

	// Add functionality to the Modal Submit Button
	$('#theModalButtonSubmit').click(function() {
		// Collect the data from the form and transform the data into JSON
		let FormData = $("#DummyForm").serialize();
		// Send the Json via Ajax to the server for processing
		$.ajax({
			type: "POST",
			data: FormData,
			url: url,
			success: function() {
				Modal.hide();
				window.location.href = '/dummy/';
				$('#theModalButtonSubmit').off('click');
			},
		});
	});
}

function DeleteDataEntry(id) {
	// Set the function variables
	let url = `/dummy/delete/${id}/`;

	// ----- Statements -----
	// Change the name of modal title and buttons
	$("#theModalTitle").text("Delete Dummy");
	$("#theModalButtonCancel").show();
	$("#theModalButtonSubmit").val("Confirm Delete");

	/* load the user creation form into the modal body then
			 load the modal.
			*/
	$('#theModalBody').load(url, function() {
		Modal.show();
	});

	// Add functionality to the Modal Submit Button
	$('#theModalButtonSubmit').click(function() {
		// Collect the data from the form and transform the data into JSON
		let FormData = $("#DummyForm").serialize();
		// Send the Json via Ajax to the server for processing
		$.ajax({
			type: 'POST',
			data: FormData,
			url: url,
			success: function() {
				Modal.hide();
				window.location.href = '/dummy/';
				$('#theModalButtonSubmit').off('click');
			}
		});
	});
}

// CRUD operations for DataTable style data display

function CreateDataEntryDt() {
	/* 
	 Displays the modal dialog with the empty form fields as received from the server then
	 collects the form data from the form fields, serializes the form into JSON, sends the
	 JSON to the corresponding function in the server then receives the server response 
	 and renders it to the page.
	*/

	// ------ Set the function variables ------
	let url = '/dummy/add/';
	let table = $('#theTable').DataTable();
	
	// ----- Statements -----

	// Change the name of modal title and buttons
	$("#theModalTitle").text("Add Dummy");
	$("#theModalButtonCancel").show();
	$("#theModalButtonSubmit").val("Save");

	/* load the user creation form into the modal body then
		 load the modal.
	*/
	$('#theModalBody').load(url, function() {
		Modal.show();
	});

	// Add functionality to the Modal Submit Button
	$('#theModalButtonSubmit').click(function() {
		// Collect the data from the form and transform the data into JSON
		let FormData = $("#DummyForm").serialize();
		// Send the Json via Ajax to the server for processing
		$.ajax({
			type: "POST",
			data: FormData,
			url: url,
			success: function() {
				Modal.hide();
				table.draw();
				$("#theModalButtonSubmit").off('click');
			}
		});
	});
}

function UpdateDataEntryDt(id) {
  /*
   Displays the modal dialog includes a form whose fields are filled with the data 
   received from the server. The function serializes the form data into JSON which
   is then sent to the backend server function. UpdateDataEntryDt receives the 
   response from the server and renders it to the page.
  */
  
  // ----- Set the function variables -----
  let url = `/dummy/edit/${id}/`;
  let table = $('#theTable').DataTable();

  // ----- Statements -----
  // Change the name of modal title and buttons
  $("#theModalTitle").text("Edit Dummy");
  $("#theModalButtonCancel").show();
  $("#theModalButtonSubmit").val("Update");

  /* load the user creation form into the modal body then
	 load the modal.
	*/
  $('#theModalBody').load(url, function() {
	  Modal.show();
  });

  // Add functionality to the Modal Submit Button
  $('#theModalButtonSubmit').click(function() {
	  // Collect the data from the form and transform the data into JSON
	  let FormData = $("#DummyForm").serialize();
	  // Send the Json via Ajax to the server for processing
	  $.ajax({
		  type: "POST",
		  data: FormData,
		  url: url,
		  success: function() {
			  Modal.hide();
			  table.draw();
			  $("#theModalButtonSubmit").off('click');
		  }
	  });
  });
}

function DeleteDataEntryDt(id) {
  // Set the function variables
  let url = `/dummy/delete/${id}/`;
  let table = $('#theTable').DataTable();

  // ----- Statements -----
  // Change the name of modal title and buttons
  $("#theModalTitle").text("Delete Dummy");
  $("#theModalButtonCancel").show();
  $("#theModalButtonSubmit").val("Confirm Delete");

  /* load the user creation form into the modal body then
	 load the modal.
	*/
  $('#theModalBody').load(url, function() {
	  Modal.show();
  });

  // Add functionality to the Modal Submit Button
  $('#theModalButtonSubmit').click(function() {
	  // Collect the data from the form and transform the data into JSON
	  let FormData = $("#DummyForm").serialize();
	  // Send the Json via Ajax to the server for processing
	  $.ajax({
		  type: "POST",
		  data: FormData,
		  url: url,
		  success: function() {
			  Modal.hide();
			  table.draw();
			  $("#theModalButtonSubmit").off('click');
		  }
	  });
  });
}

const search = document.querySelector('#dataSearchField');
search.addEventListener("input", (event) => searchTool(event.target.value))

function searchTool(search) {
  	let url = `/dummy/search/`;
	if (search != null) {
	  $.ajax({
	  	type: "GET",
	  	url: url,
	  	data: {
	  	  search: search,
	  	},
	  	success: function (response) {
	  	  $('#cardView').empty();
	  	  $('#cardView').html(response)
	  	}
	  });
	}
}
