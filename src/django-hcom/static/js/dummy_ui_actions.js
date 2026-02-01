// vim:foldmethod=indent:ts=2

// =======================================================================================
// Imports
// =======================================================================================

import { CRUDSManager, ComponentInitializer, handleRUDevents } from "./ui_actions.js";

// =======================================================================================
// constants
// =======================================================================================

const Base_URL = `/dummy/`;
const Table_URL = Base_URL + `json/`;
const Search_URL = Base_URL + `search/`;

const Manager = new CRUDSManager(Base_URL);
const CompInit = new ComponentInitializer();

const Viewtype_Value = Manager.Viewtype_Value;

const Table = new DataTable("#theTable", {
  processing: true,
  serverSide: true,
  autoWidth: false,
  ajax: {
    datatype: "json",
    url: Table_URL,
  },
  columns: [
    { data: "id" },
    {
      render: (data, type, row) =>
        `<a class="text-decoration-none" href="#" id="dataItemDetail-${row.id}">${row.name}</a>`,
    },
    { data: "category" },
    {
      render: (data, type, row) =>
        `
        <div class="btn-group" role="group">
          <a class="btn btn-sm btn-success" type="button" title="Edit ${row.name}" href="#" id="dataItemUpdate-${row.id}"><i class="fas fa-edit"></i></a>
          <a class="btn btn-sm btn-danger" type="button" title="Delete ${row.name}" href="#" id="dataItemDelete-${row.id}"><i class="fas fa-trash"></i></a>
        </div>
        `,
    },
  ],
});

window.table = Table;

const form = () => {
  return document.getElementById("DummyForm");
};

const Create_Options = {
  title: "Add Dummy",
  btn1_name: "Submit",
  url: Base_URL + `add/`,
  form: form,
  // redirect_to_url: Base_URL,
  // table: Table,
  // viewtype_value: Viewtype_Value,
};

const Read_Options = {
  title: "Dummy Detail",
  btn1_name: "Dismiss",
  show_btn2: false,
  url: Base_URL,
};

const Update_Options = {
  title: "Edit Dummy",
  btn1_name: "Update",
  url: Base_URL + `edit/`,
  form: form,
  // redirect_to_url: Base_URL,
  // // table: Table,
  // viewtype_value: Viewtype_Value,
};

const Delete_Options = {
  title: "Delete Dummy",
  btn1_name: "Confirm Delete",
  url: Base_URL + `delete/`,
  form: form,
  // redirect_to_url: Base_URL,
  // table: Table,
  // viewtype_value: Viewtype_Value,
};

// Initialize AddRcrdBtn
const AddRcrdBtn = CompInit.elementInitializer({
  component: "AddRecordBtnElement",
  elementId: "AddRecordBtn",
  elementName: "Add Button",
});
// Initialize SearchField
const SearchField = CompInit.elementInitializer({
  component: "searchFieldElement",
  elementId: "dataSearchField",
  elementName: "Search Field",
});

// =======================================================================================
// Event Listeners
// =======================================================================================

// Event Listener for Create Method. The button that is involved in data creation is
// present in both cardview and tableview.
AddRcrdBtn.addEventListener("click", () => Manager.create(Create_Options));

// Event Listener for searching.
SearchField.addEventListener("input", (event) => Manager.search(event.target.value, Search_URL));

/**
 * Checks for the current view type through the viewtype hidden field value:
 * viewtype value of `true` indicates a table view. 
 * That of `false` indicates card view.
 **/
if (Viewtype_Value !== "true") handleRUDevents("cardView", { readOptions: Read_Options, updateOptions: Update_Options, deleteOptions: Delete_Options }, Manager);
else handleRUDevents("tableView", { readOptions: Read_Options, updateOptions: Update_Options, deleteOptions: Delete_Options }, Manager);
