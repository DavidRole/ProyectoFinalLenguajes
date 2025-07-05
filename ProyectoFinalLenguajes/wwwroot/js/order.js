var dataTable,
    STATE_ON_TIME,
    STATE_OVERTIME,
    STATE_LATE,
    STATE_DELIVERED,
    STATE_NULLED;

$(document).ready(function () {
    loadDataTable();

    var lastOrder = document.getElementById("lastOrder");
    lastOrder.innerHTML = `
            <p class="text-primary">No orders delivered yet</p>
        `;

});

function loadDataTable() {
    var $tbl = $('#tblData');

    STATE_ON_TIME = $tbl.data('ontime');
    STATE_OVERTIME = $tbl.data('overtime');
    STATE_LATE = $tbl.data('late');
    STATE_DELIVERED = $tbl.data('delivered');
    STATE_NULLED = $tbl.data('nulled');


    dataTable = $tbl.DataTable({
        ajax: {
            "url": "/Kitchen/Order/GetAll"
        },
        lengthMenu: [5, 10, 20, 50],
        order: [[0, 'desc']],
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
                           <a onclick="Deliver(${data})" class="btn btn-primary">
                              <i class="bi bi-arrow-right-square"></i>
                            </a>
                        </div>
                    `;
                },
                orderable: false,
                width: "15%"
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
                else if (diffMin >= 5 && diffMin <= 15) {
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

    setInterval(() => dataTable.ajax.reload(null, false), 1000);
}

function Deliver(id) {
    var lastOrder = document.getElementById("lastOrder");
    if (!lastOrder) return console.error("No #lastOrder element found");
    lastOrder.innerHTML = `
    <label class="btn btn-outline-secondary">#${id}</label>
    <a onclick="Rollback(${id})" class="btn btn-outline-primary">
        <i class="bi bi-arrow-counterclockwise"></i>
    </a>
    `;

    updateStatus(id, STATE_DELIVERED);
}

function Rollback(id) {
    updateStatus(id, STATE_ON_TIME);
    var lastOrder = document.getElementById("lastOrder");
    lastOrder.innerHTML = `
    <p class="text-primary">No orders delivered yet</p>
    `;
}

function updateStatus(id, status) {
    $.ajax({
        url: `/Kitchen/Order/Update/${id}`,
        type: 'POST',
        data: { status: status },
        success: function (data) {
            if (data.success) {
                console.log(data.message);
            } else {
                console.log(data.message);
            }

        },
        error: function () {
            console.log("Error connecting to endpoint.");
        }
    });
}