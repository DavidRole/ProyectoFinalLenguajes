var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    var $tbl = $('#tblData');

    var STATE_ON_TIME = $tbl.data('onTime');
    var STATE_OVERTIME = $tbl.data('overtime');
    var STATE_LATE = $tbl.data('late');
    var STATE_DELIVERED = $tbl.data('delivered');
    var STATE_NULLED = $tbl.data('nulled');


    dataTable = $tbl.DataTable({
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
            { data: null, orderable: false, width: '10%', defaultContent: '' },
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
        ],
        columnDefs: [{
            targets: 3,
            render: function (_data, _type, row) {
                const created = new Date(row.date);
                const diffMin = Math.floor((Date.now() - created) / 60000);

                let cls;
                if (diffMin < 5) {
                    cls = 'badge bg-success';
                }
                else if (diffMin >= 5 && diffMin <=15) {
                    cls = 'badge bg-warning text-dark';
                    if (row.status != STATE_OVERTIME)
                        updateStatus(row.id, STATE_OVERTIME);
                }
                else if (diffMin > 15) {
                    cls = 'badge bg-danger';
                    if (row.status != STATE_LATE)
                        updateStatus(row.id, STATE_LATE);
                }

                return `<span class="${cls}">${row.status}</span>`;
            }
        }]
    });

    setInterval(() => dataTable.ajax.reload(null, false), 6000);
}

function updateStatus(id, status) {
    $.ajax({
        url: `/Kitchen/Order/Update/${id}`,
        type: 'POST',               
        data: { status: status },
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
            } else {
                toastr.error(data.message);
            }

        },
        error: function () {
            toastr.error("Error connecting to endpoint.");
        }
    });
}