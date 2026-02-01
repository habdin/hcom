// vim:foldmethod=indent:ts=2:sw=2

const Modal = new bootstrap.Modal('#theModal');

const search = document.querySelector('#dataSearchField');
search.addEventListener("input", (event) => searchTool(event.target.value));

const addRcrdBtn = document.querySelector('#AddRecordBtn');
addRcrdBtn.addEventListener('click', () => CreateDataEntry());

let table = new DataTable('#theTable', {
  processing: true,
  serverSide: true,
  autoWidth: false,
  ajax: {
    datatype: 'json',
    url: `/pharmacy/drug/json/`,
  },
  columns: [
    { "data": "id" },
    { "render": (data, type, row) => `<a class="text-decoration-none" href="#" onclick="ReadDataEntry(${row.id})">${row.drug_name}</a>`},
    { "data": "company" },
    { "data": "drug_dose" },
    { "data": "drug_unit" },
    { "data": "drug_form" },
    { "data": "drug_price" },
	  { "render": (data, type, row) => 
	    `
	          <div class="btn-group" role="group">
		          <a class="btn btn-sm btn-success" type="button" title="Edit ${row.name}" href="#" onclick="UpdateDataEntry(${row.id})"><i class="fas fa-edit"></i></a>
		          <a class="btn btn-sm btn-danger" type="button" title="Delete ${row.name}" href="#" onclick="DeleteDataEntry(${row.id})"><i class="fas fa-trash"></i></a>
	          </div>
	          `
	  },
  ],
});

const viewtype = document.querySelector('#viewtype').value

let redirect_to_url = `/pharmacy/drug/`

function CreateDataEntry() {
  // Variable declarations
  let url = `/pharmacy/drug/add/`;
  // Modal Settings and loading
  $("#theModalTitle").text("Add Drug");
  $("#theModalButtonCancel").show();
  $("#theModalButtonSubmit").val("Save");
  $('#theModalBody').load(url, function() { Modal.show(); });
  // Action taken with modal submit
  $('#theModalButtonSubmit').click(function () {
  	if ($('#DrugForm').valid()) {
  	  let FormData = $('#DrugForm').serialize(); // collect form data and serialize it
  	  $.ajax({
	  	  type: "POST",
	  	  data: FormData,
	  	  url: url,
	  	  success: function () {
	  	    $('#theModalButtonSubmit').off('click');
	  	    Modal.hide();
	  	    if (viewtype == "True") { window.location.href = redirect_to_url}
	  	    else table.draw();
	  	  }
  	  })
	  }
	  else {
	    // validity = $('#DummyForm').valid();
	    // console.log('form is valid: ', validity);
	    $('#theModalButtonSubmit').off('click');
	  }
  })
}

function ReadDataEntry(id) {
  // Variable declarations
  url = `/pharmacy/drug/${id}/`;
  // Modal Settings and loading
  $("#theModalTitle").text("Drug Detail");
  $("#theModalButtonCancel").hide();
  $("#theModalButtonSubmit").val("Ok");
  $('#theModalBody').load(url, function() { Modal.show(); });
  // Action taken with modal submit
  $("#theModalButtonSubmit").click(function() {
	  $('#theModalButtonSubmit').off('click');
	  Modal.hide();
  });
}

function UpdateDataEntry(id) {
  // Variable declarations
  let url = `/pharmacy/drug/edit/${id}/`;
  // Modal Settings and loading
  $("#theModalTitle").text("Edit Drug");
  $("#theModalButtonCancel").show();
  $("#theModalButtonSubmit").val("Update");
  $('#theModalBody').load(url, function() { Modal.show(); });
  // Action taken with modal submit
  $('#theModalButtonSubmit').click(function () {
  	if ($('#DrugForm').valid()) {
  	  let FormData = $('#DrugForm').serialize(); // collect form data and serialize it
  	  $.ajax({
	  	  type: "POST",
	  	  data: FormData,
	  	  url: url,
	  	  success: function () {
	  	    $('#theModalButtonSubmit').off('click');
	  	    Modal.hide();
	  	    if (viewtype == "True") { window.location.href = redirect_to_url}
	  	    else table.draw();
	  	  }
  	  })
	  }	
	  else {
	    // validity = $('#DummyForm').valid();
	    // console.log('form is valid: ', validity);
	    $('#theModalButtonSubmit').off('click');
	  }
  })
}

function DeleteDataEntry(id) {
  // Variable declarations
  let url = `/pharmacy/drug/delete/${id}/`
  // Modal Settings and loading
  $('#theModalTitle').html("Delete Drug");
  $('#theModalButtonSubmit').val("Confirm Delete");
  $('#theModalButtonCancel').show();
  $('#theModalBody').load(url, () => Modal.show());
  // // Action taken with modal submit
  $('#theModalButtonSubmit').click(function() {
  	let FormData = $('#DrugForm').serialize(); 
  	$.ajax({
	  	type: "POST",
	  	data: FormData,
	  	url: url,
	  	success: function () {
	  	  $('#theModalButtonSubmit').off('click');
	  	  Modal.hide();
	  	  if (viewtype == "True") { window.location.href = redirect_to_url}
	  	  else table.draw();
	  	}
	  });
  });
}

function searchTool(search) {
  $.ajax({
    type: "GET",
    url: `/pharmacy/drug/search/`,
    data: { 'search': search }, 
    success: function (response) {
      let target = document.querySelector('#cardView')
      target.innerHTML = response
    }
  })
}

