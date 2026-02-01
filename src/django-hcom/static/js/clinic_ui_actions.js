//vim:foldmethod=indent:ts=2

const Modal = new bootstrap.Modal('#theModal');

const search = document.querySelector('#dataSearchField');
search.addEventListener("input",
    (event) => searchTool(event.target.value))

const addRcrdBtn = document.querySelector('#AddRecordBtn')
addRcrdBtn.addEventListener('click',
    () => CreateDataEntry())

let table = new DataTable('#theTable', {
    processing: true,
    serverSide: true,
    autoWidth: false,
    ajax: {
	    datatype: 'json',
	    url: `/clinic/json/`
    },
    columns: [
	    { "data": "id" },
	    { "render": (data, type, row) => `<a class="text-decoration-none" href="#" onclick="ReadDataEntry(${row.id})">${row.physician_name}</a>`},
	    { "data": "opening_time" },
	    { "data": "closing_time" },
	    { "data": "is_archived" },
	    { "render": (data, type, row) => 
	        `
	  <div class="btn-group" role="group">
  	  	<a class="btn btn-sm btn-success" type="button" title="Edit ${row.physician_name}" href="#" onclick="UpdateDataEntry(${row.id})"><i class="fas fa-edit"></i></a>
  	  	<a class="btn btn-sm btn-danger" type="button" title="Delete ${row.physician_name}" href="#" onclick="DeleteDataEntry(${row.id})"><i class="fas fa-trash"></i></a>

  	  </div>
  	`
	    },
    ]
})

const viewtype = document.querySelector('#viewtype').value

let redirect_to_url = `/clinic/`

function CreateDataEntry() {
    // Variable declarations
    let url = `/clinic/add/`;
    // Modal Settings and loading
    $("#theModalTitle").text("Add Clinic");
    $("#theModalButtonCancel").show();
    $("#theModalButtonSubmit").val("Save");
    $('#theModalBody').load(url, () => Modal.show());
    // Action taken with modal submit
    $('#theModalButtonSubmit').click(function () {
  	    if ($('#ClinicForm').valid()) {
  	        let FormData = $('#ClinicForm').serialize(); // collect form data and serialize it
  	        $.ajax({
	  	        type: "POST",
	  	        data: FormData,
	  	        url: url,
	  	        success: function () {
	  	            Modal.hide();
	  	            if (viewtype == "True") { window.location.href = redirect_to_url}
	  	            else table.draw();
	                $('#theModalButtonSubmit').off('click');
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
    let url = `/clinic/${id}/summary/`  
    // Modal Settings and loading
    $("#theModalTitle").text("Clinic Detail");
    $("#theModalButtonCancel").hide();
    $("#theModalButtonSubmit").val("Ok");
    $('#theModalBody').load(url, function() { Modal.show(); });
    // Action taken with modal submit
    $("#theModalButtonSubmit").click(function() {
	    Modal.hide();
	    $("#theModalButtonSubmit").off('click');
    });
}


function UpdateDataEntry(id) {
    // Variable declarations
    let url = `/clinic/edit/${id}/`
    // Modal Settings and loading
    // console.log("URL: ", url)
    $('#theModalTitle').text('Edit Clinic');
    $('#theModalButtonCancel').show();
    $('#theModalButtonSubmit').val("Update");
    $('#theModalBody').load(url, () => Modal.show())
    // Action taken with modal submit
    $('#theModalButtonSubmit').click(function () {
  	    if ($('#ClinicForm').valid()) {
  	        let FormData = $('#ClinicForm').serialize(); // collect form data and serialize it
  	        $.ajax({
	  	        type: "POST",
	  	        data: FormData,
	  	        url: url,
	  	        success: function () {
	  	            Modal.hide();
	  	            if (viewtype == "True") { window.location.href = redirect_to_url}
	  	            else table.draw();
	                $('#theModalButtonSubmit').off('click');
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

}

function searchTool(search) {
    $.ajax({
        type: "GET",
        url: `/clinic/search/`,
        data: { 'search': search }, 
        success: function (response) {
            let target = document.querySelector('#cardView')
            target.innerHTML = response
        }
    })
}
