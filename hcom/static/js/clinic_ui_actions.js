function showAsCard() {
    $('#tableView').hide();
    $('#cardView').show();
    $('.dataTables_wrapper .row').hide();
}

function showAsTable() {
    $('#tableView').show();
    $('#cardView').hide();
    $('.dataTables_wrapper .row').show();
}

$(document).ready(function () {
    $('#tableView').DataTable({
	// processing: true,
	// serverSide: true,
	// ajax: {
	//     datatype: 'json',
	//     url: '/users/user-list/'
	// },
	// columns: [
	//     { "data": "id"},
	//     { "render":
	//       function (data, type, row, meta) {
	// 	  return '<a class="text-decoration-none" href="'
	// 	      + row.id
	// 	      + '">'
	// 	      + row.username
	// 	      + '</a>'
	//       }
	//     },
	//     { "data": "first_name"},
	//     { "data": "last_name"},
	//     { "data": "email"},
	//     { "data": "is_staff"},
	//     { "render":
	//       function (data, type, row, meta) {
	// 	  return '<div class="btn-group" role="group">'
	// 	      + '<a class="btn-sm btn-success" type="button" title="Edit User '
	// 	      + row.username
	// 	      + '" href="'
	// 	      + row.id
	// 	      + '"><i class="fas fa-edit"></i></a>'
	// 	      +'<a class="btn-sm btn-danger" type="button" title="Delete User '
	// 	      + row.username
	// 	      + '" href="'
	// 	      + row.id
	// 	      + '"><i class="fas fa-trash"></i></a>'
	// 	      + '</div>'
	//       }
	//     },
	// ]
	});
    $('.dataTables_wrapper .row').hide();
});

function AddEditDataEntry(id) {
    
}

function DeleteDataEntry(id) {
    
}
