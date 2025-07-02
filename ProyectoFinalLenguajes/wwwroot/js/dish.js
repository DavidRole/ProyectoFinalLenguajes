var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": "/Admins/Dish/GetAll"
        },
        "columns": [
            { "data": "name", width: "50%" },
            { "data": "isAble", width: "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="container btn-group" role="group">
                            <a href="/Admins/Dish/Upsert/${data}" class="btn btn-outline-primary mx-2">
                                <i class="bi bi-pencil-square"></i>
                                Edit
                            </a>

                            <a onClick = Delete(${data}) class="btn btn-outline-danger mx-2">
                                <i class="bi bi-trash3"></i>
                                Delete
                            </a>
                        </div>
                    `
                },
                width: "30%"
            }
        ]
    });
}

function Delete(_id) {

    Swal.fire({
        theme: "dark",
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Admins/Admin/delete/" + _id,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }

                },
                error: function () {
                    toastr.error("Error connecting to endpoint.");
                }
            });
        }
    });
}