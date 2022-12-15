var dataTable

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable(
        {
            "ajax": {
                "url": "/Item/GetAll"
            },
            "columns": [
                { "data": "title", "width": "15%" },
                { "data": "make", "width": "15%" },
                { "data": "model", "width": "15%" },
                { "data": "created", "width": "15%" },
                { "data": "category", "width": "15%" },
                { "data": "price", "width": "15%" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                               <div class="w-75 btn-group" role="group" >
                                    <a class="btn btn-primary mx-2" href="/Admin/Product/Upsert?id=${data}"><i class="bi bi-pencil-square"></i>Edit</a>
                                    <a onClick=Delete('/Item/Delete/${data}') class="btn btn-danger mx-2" ><i class="bi bi-x-circle"></i>Delete</a>
                                </div>
                                `
                    },
                    "width": "15%"
                }
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
                        dataTable.ajax.reload();
                        toastr.success(data.message);

                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}