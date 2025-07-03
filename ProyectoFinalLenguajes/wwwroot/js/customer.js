var dataTable;

$(document).ready(function () {
    console.log("Loading Table");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": "/Admins/Customer/GetAll",
            dataSrc: "data"
        },
        "columns": [
            { "data": "firstName", width: "20%" },
            { "data": "lastName", width: "20%" },
            { "data": "email", width: "30%" },
            { "data": "isAble", width: "10%" },
            {
                data: 'id',
                render: function (id) {
                    return `
                      <div class="btn-group" role="group">
                        <a href="/Admins/Customer/Upsert/${id}" class="btn btn-outline-primary">
                          <i class="bi bi-pencil-square"></i>
                        </a>
                        <a onclick="Delete('${id}')" class="btn btn-outline-danger">
                          <i class="bi bi-trash3"></i>
                        </a>
                      </div>`;
                },
                orderable: false,
                width: '20%'
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
                url: "/Admins/Customer/delete/" + _id,
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
