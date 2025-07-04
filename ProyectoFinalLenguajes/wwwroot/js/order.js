var dataTable;

$(document).ready(function () {
    loadDataTable();
    setInterval(dataTable.ajax.reload, 6000);
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": "/Kitchen/Order/GetAll"
        },
        lengthMenu: [5, 10, 20, 50], 
        "columns": [
            { "data": "id", width: "5%" },
            {
                "data": "customer.firstName",
                "render": function (data) {
                    return data ?? 'Sin nombre';
                },
                width: "20%"
            },
            {
                "data": "orderDishes",
                "render": function (data) {
                    if (!Array.isArray(data)) return "Sin platos";

                    return data.map(d => {
                        const dishName = d.dish?.name ?? 'Desconocido';
                        const quantity = d.quantity ?? 0;
                        return `${dishName} x${quantity}`;
                    }).join("<br>");
                },
                width: "40%"
            },
            {
                data: null,
                render: function (_data, _type, row) {
                    const status = row.status;
                    const created = new Date(row.date);
                    const now = new Date();
                    const diffMs = now - created;
                    const diffMin = Math.floor(diffMs / 1000 / 60);

                    
                    let cls = "";
                    if (diffMin <= 7) {
                        cls = 'text-bg-success';
                    } else if (diffMin > 7 && diffMin <= 14) {
                        cls = 'text-bg-warning'
                    } else if (diffMin > 14 ) {
                        cls = 'text-bg-danger'
                    }

                    return `<span class="${cls}">${status}</span>`;
                },
                width: "10%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="container btn-group" role="group">
                            <a href="/Kitchen/Order/Details/${data}" class="btn btn-outline-primary mx-2">
                                <i class="bi bi-arrows-fullscreen"></i>
                            </a>
                        </div>
                    `;
                },
                orderable: false,
                width: "5%"
            }
        ]
    });
    
    
}
