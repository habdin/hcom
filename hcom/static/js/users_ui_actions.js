// vim: foldmethod=indent
function showDataAsCard() {
	$('#tableView').empty();
	window.location.reload();
}

function showDataAsTable() {
	$('#AddRecordButtonDiv')
		.html(` <button class="btn btn-primary mt-2 mb-3" title="Add New Record" data-bs-toggle="modal" data-bs-target="#theModal"
                onclick="CreateDataEntry_Datatables(0)">
            <i class="fas fa-plus-square"></i>
        </button>`);
	$('#tableView')
		.html(`<table class= "table table-bordered table-striped table-hover mt-3" id = "theTable">
		 	 	 	 <thead class="table-primary">
						<tr> 
		 			 	 	<th>ID</th>
							<th>Username</th>
							<th>First Name</th>
							<th>Last Name</th>
							<th>Email</th>
							<th>Staff Status</th>
							<th>Actions</th>
						</tr>
					</thead>
				</table > `);
	$('#theTable').DataTable({
		processing: true,
		serverSide: true,
		autoWidth: false,
		ajax: {
			datatype: 'json',
			url: '/users/user-list'
		},
		columns: [
			{ "data": "id" },
			{
				"render":
					function(data, type, row) {
						return `<a class="text-decoration-none" href="#" onclick="ReadDataEntry(${row.id})">${row.username}</a>`;
					}
			},
			{ "data": "first_name" },
			{ "data": "last_name" },
			{ "data": "email" },
			{ "data": "is_staff" },
			{
				"render":
					function(data, type, row) {
						return `<div class="btn-group" role="group">
<a class="btn btn-sm btn-success" type="button" title="Edit User ${row.username}" href="#" onclick="UpdateDataEntry(${row.id})"><i class="fas fa-edit"></i></a>
<a class="btn btn-sm btn-danger" type="button" title="Delete User ${row.username}" href="#" onclick="DeleteDataEntry(${row.id})"><i class="fas fa-trash"></i></a></div>`
					}
			},
		]
	});
	$('#cardView').empty().hide();
}

function CreateDataEntry() {
	// ------ Set the function variables ------
	var url = '/users/add/';

	// ----- Statements -----

	// Change the name of modal title and buttons
	$("#theModalTitle").text("Add User");
	$("#theModalButtonCancel").show();
	$("#theModalButtonSubmit").val("Save");

	/* load the user creation form into the modal body then
		 load the modal.
		*/
	$('#theModalBody').load(url, function() {
		$("#theModal").modal('show');
	});

	// Add functionality to the Modal Submit Button
	$('#theModalButtonSubmit').click(function() {
		// Collect the data from the form and transform the data into JSON
		var FormData = $("#UserCreateForm").serialize();
		// Send the Json via Ajax to the server for processing
		$.ajax({
			type: "POST",
			data: FormData,
			url: url,
			success: function() {
				$("#theModal").modal('hide');
				window.location.href = '/users/'
				$("#theModalButtonSubmit").off('click');
			}
		});
	});
}

function ReadDataEntry(id) {
	// ----- Set the function variables -----
	var url = `/users/${id}/`;
	// ----- Statements -----
	// Change the name of modal title and buttons
	$('#theModalTitle').text('User Detail');
	$('#theModalButtonCancel').hide();
	$('#theModalButtonSubmit').val('Ok');

	/* load the user creation form into the modal body then
		 load the modal.
		*/
	$('#theModalBody').load(url, function() {
		$("#theModal").modal('show');
	});

	// Add functionality to the Modal Submit Button
	$("#theModalButtonSubmit").click(function() {
		$("#theModalButtonSubmit").off('click');
		$("#theModalButtonCancel").show(1000);
		$("#theModal").modal('hide');
	});
}

function UpdateDataEntry(id) {
	// ----- Set the function variables -----
	var url = `/users/edit/${id}/`;

	// ----- Statements -----
	// Change the name of modal title and buttons
	$("#theModalTitle").text("Edit User");
	$("#theModalButtonCancel").show();
	$("#theModalButtonSubmit").val("Update");

	/* load the user creation form into the modal body then
		 load the modal.
		*/
	$('#theModalBody').load(url, function() {
		$("#theModal").modal('show');
	});

	// Add functionality to the Modal Submit Button
	$('#theModalButtonSubmit').click(function() {
		// Collect the data from the form and transform the data into JSON
		var FormData = $("#UserCreateForm").serialize();
		// Send the Json via Ajax to the server for processing
		$.ajax({
			method: "POST",
			data: FormData,
			url: url,
			success: function() {
				$("#theModal").modal('hide');
				window.location.href = '/users/';
				$('#theModalButtonSubmit').off('click');
			},
		});
	});
}

function DeleteDataEntry(id) {
	// Set the function variables
	var url = `/users/delete/${id}/`

	// ----- Statements -----
	// Change the name of modal title and buttons
	$("#theModalTitle").text("Delete User");
	$("#theModalButtonCancel").show();
	$("#theModalButtonSubmit").val("Confirm Delete");

	/* load the user creation form into the modal body then
		 load the modal.
		*/
	$('#theModalBody').load(url, function() {
		$("#theModal").modal('show');
	});

	// Add functionality to the Modal Submit Button
	$('#theModalButtonSubmit').click(function() {
		// Collect the data from the form and transform the data into JSON
		var FormData = $("#UserDeleteForm").serialize();
		// Send the Json via Ajax to the server for processing
		$.ajax({
			method: 'POST',
			data: FormData,
			url: url,
			success: function() {
				$('#theModal').modal('hide');
				window.location.href = '/users/';
				$('#theModalButtonSubmit').off('click');
			}
		});
	});
}
