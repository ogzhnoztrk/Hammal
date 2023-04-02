var dataTable

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Customer/Advertisement/getall"
        },
        "columns": [
            { "data":"advertiserID", "width":"15%"},
            { "data":"title", "width":"15%"},
            { "data":"category.name", "width":"15%"},
            { "data":"releaseDate", "width":"15%"},
            { "data":"lastDate", "width":"15%"},
            
            {
                "data": "id", "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Customer/Advertisement/Upsert?id=${data}"
                            class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i>Edit</a>
                            <a onClick=Delete('/Customer/Advertisement/Delete/${data}') 
                            class="btn btn-danger mx-2"><i class="bi bi-trash"></i>Delete</a>                            
                        </div>`
                }, "width": "15%"
            },
           
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => { location.reload() }, 500);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}