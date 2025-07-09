var datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    var $tbl = $('#tblData');
    dataTable = $tbl.DataTable({
        ajax: {
            "url": "/Admins/Order/GetAll"
        },
        lengthMenu: [5, 10, 20, 50],
        columnDefs: [
            {
                targets: '_all',  
                className: 'dt-left'
            }
        ],
        "columns": [
            { "data": "id", width: "5%" },
            {
                "data": "customer.firstName",
                "render": function (data) {
                    return data ?? 'No name';
                },
                width: "20%"
            },
            {
                "data": "orderDishes",
                "render": function (data) {
                    if (!Array.isArray(data)) return "No dishes";

                    return data.map(d => {
                        const dishName = d.dish?.name ?? 'Unknown';
                        const quantity = d.quantity ?? 0;
                        return `${dishName} x${quantity}`;
                    }).join("<br>");
                },
                width: "40%"
            },
            {
                "data": "date",
                "render": function (data) {
                    const date = new Date(data);
                    return date.toLocaleString('es-CR', {
                        year: 'numeric',
                        month: '2-digit',
                        day: '2-digit',
                        hour: '2-digit',     
                        minute: '2-digit',    
                    });
                    
                },
                width: "15%"
            },
            { data: null, orderable: false, width: '10%', defaultContent: '' },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="container btn-group" role="group">
                            <a href="/Admins/Order/Details/${data}" class="btn btn-outline-primary">
                                <i class="bi bi-arrows-fullscreen"></i>
                            </a>
                            <a href="/Admins/Order/EditStatus/${data}" class="btn btn-outline-primary">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                        </div>
                    `;
                },
                orderable: false,
                width: "5%"
            }
        ],
    });
}
