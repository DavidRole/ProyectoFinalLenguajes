var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": "/Kitchen/Order/GetAll"
        },
        "columns": [
            { "data": "id", width: "5%" },
            { "data": "customer.firstName", width: "20%" },
            //{ "data": "OrderDishes", width: "40%"},
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="container btn-group" role="group">
                            <a href="/Kitchen/Order/Details/${data}" class="btn btn-outline-primary mx-2">
                                <i class="bi bi-arrows-fullscreen"></i>
                            </a>
                        </div>
                    `
                },
                orderable: false,
                width: "5%"
            }
        ]
    });
}
