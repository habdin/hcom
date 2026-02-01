// vim: foldmethod=indent:ts=4

const Modal = new bootstrap.Modal('#theModal');

const search = document.querySelector('#dataSearchField');
search.addEventListener("input", (event) => searchTool(event.target.value))

const addRcrdBtn = document.querySelector('#AddRecordBtn')
addRcrdBtn.addEventListener('click', () => CreateDataEntry())

let table = new DataTable('#theTable', {
  processing: true,
  serverSide: true,
  autoWidth: false,
  ajax: {
  	datatype: 'json',
  	url: `/users/json/`,
  },
  columns: [
  	{ "data": "id"},
  	{ "render": (data,type, row) => 
  	  `<a class="text-decoration-none" href="#" onclick="ReadDataEntry(${row.id})"> ${row.username} </a>` },
  	{ "data": "first_name" },
  	{ "data": "last_name" },
  	{ "data": "email" },
  	{ "data": "is_staff" },
  	{ "render": (data, type, row) => 
	  `
	  <div class="btn-group" role="group">
  	  	<a class="btn btn-sm btn-success" type="button" title="Edit ${row.username}" href="#" onclick="UpdateDataEntry(${row.id})"><i class="fas fa-edit"></i></a>
  	  	<a class="btn btn-sm btn-danger" type="button" title="Delete ${row.username}" href="#" onclick="DeleteDataEntry(${row.id})"><i class="fas fa-trash"></i></a>

  	  </div>
  	`
  	},
  ],
})

const viewtype = document.querySelector('#viewtype').value

let redirect_to_url = `/users/`

function CreateDataEntry() {
  // Variable Declaration
  let url = '/users/add/';
  // Modal Settings and loading
  $("#theModalTitle").text("Add User");
  $("#theModalButtonCancel").show();
  $("#theModalButtonSubmit").val("Save");
  $('#theModalBody').load(url, function() { Modal.show(); });
  // Action taken with modal submit
  $('#theModalButtonSubmit').click(function() {
  	if ($("#UserCreateForm").valid()) {
  	  // Collect the data from the form and transform the data into JSON
	  let FormData = $("#UserCreateForm").serialize();
	  $.ajax({
	  	type: "POST",
	  	data: FormData,
	  	url: url,
	  	success: function() {
	  	  $('#theModalButtonSubmit').off('click');
		  Modal.hide();
		  if (viewtype == "True") { window.location.href = redirect_to_url }
	  	  else { table.draw(); }
	  	}
	  });
  	}
  	else
  	{ $('#theModalButtonSubmit').off('click'); }
  });
}

function ReadDataEntry(id) {
  // Variable declarations
  let url = `/users/${id}/`;
  // Modal Settings and loading
  $('#theModalTitle').text('User Detail');
  $('#theModalButtonCancel').hide();
  $('#theModalButtonSubmit').val('Ok');
  $('#theModalBody').load(url, function() { Modal.show(); });
  // Action taken with modal submit
  $("#theModalButtonSubmit").click(function() {
	$("#theModalButtonSubmit").off('click');
	$("#theModalButtonCancel").show(1000);
	Modal.hide();
  });
}

function UpdateDataEntry(id) {
  // Variable declarations
  let url = `/users/edit/${id}/`;
  // Modal Settings and loading
  $("#theModalTitle").text("Edit User");
  $("#theModalButtonCancel").show();
  $("#theModalButtonSubmit").val("Update");
  $('#theModalBody').load(url, function() { Modal.show(); });
  // Action taken with modal submit
  $('#theModalButtonSubmit').click(function() {
	if ($('#UserCreateForm').valid()) {
	  let FormData = $("#UserCreateForm").serialize(); // collect form data and serialize it
	  $.ajax({
	  	type: "POST",
	  	data: FormData,
	  	url: url,
	  	success: function() {
		  $('#theModalButtonSubmit').off('click');
		  Modal.hide();
		  if (viewtype == "True") { window.location.href = redirect_to_url }
	  	  else { table.draw(); } 
	  	},
	  });
	}
	else $('#theModalButtonSubmit').off('click');
  });
}

function DeleteDataEntry(id) {
  // Variable declarations
  let url = `/users/delete/${id}/`
  // Modal Settings and loading
  $("#theModalTitle").text("Delete User");
  $("#theModalButtonCancel").show();
  $("#theModalButtonSubmit").val("Confirm Delete");
  $('#theModalBody').load(url, function() { Modal.show(); });
  // Action taken with modal submit
  $('#theModalButtonSubmit').click(function() {
	let FormData = $("#UserDeleteForm").serialize(); // collect form data and serialize it
	$.ajax({
	  method: 'POST',
	  data: FormData,
	  url: url,
	  success: function() {
		$('#theModalButtonSubmit').off('click');
		Modal.hide();
	  	  if (viewtype == "True") { window.location.href = redirect_to_url}
	  	  else { table.draw(); }
	  }
	});
  });
}

function searchTool(search) {
  $.ajax({
  	type: "GET",
  	url: `/users/search/`,
  	data: { 'search': search },
  	success: function (response) {
  	  let target = document.querySelector('#cardView')
  	  target.innerHTML = response
  	}
  })
}
