$(document).ready(function () {
    $('#orderGrid').DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Order/LoadOrderData",
            "type": "POST",
            "dataType": "json"
        },
        "columns": [
            { "data": "orderNumber", "name": "OrderNumber", "autoWidth": true },
            {
                "data": "orderDate",
                "name": "OrderDate",
                "autoWidth": true,
                "render": function (data) {
                    if (data) {
                        let d = new Date(data);
                        let day = d.getDate().toString().padStart(2, '0');
                        let month = (d.getMonth() + 1).toString().padStart(2, '0');
                        let year = d.getFullYear();
                        return `${day}/${month}/${year}`; // DD/MM/YYYY
                    }
                    return "";
                }
            },
            { "data": "total", "name": "Total", "autoWidth": true },
            { "data": "customerName", "name": "CustomerName", "autoWidth": true },
            {
                "data": "id",
                "render": function (data, type, row) {
                    return `
                                <button class="btn btn-sm me-1" onclick="editOrder('${data}')"><i class="bi bi-pen-fill"></i></button>
                                <button class="btn btn-sm" onclick="deleteOrder('${data}')"><i class="bi bi-trash-fill"></i></button>
                            `;
                },
                "orderable": false,
                "searchable": false,
                "width": "150px"
            }
        ]
    })
});


function deleteOrder(id) {
    $('#hiddenOrderId').val(id);
    // Hide old error messages
    $("#orderDeleteError").addClass("d-none").text("");
    $('#orderDeleteModal').modal('show');
}

$('#orderDeleteBtn').click(function()  {
    var orderId = $('#hiddenOrderId').val();
    $.ajax({
        method: "DELETE",
        url: "/Order/RemoveOrder?orderId=" + orderId,
        success: function (response) {
            // reload DataTable if needed
            $('#orderGrid').DataTable().ajax.reload();

            // Hide old error messages
            $("#orderDeleteError").addClass("d-none").text("");

            // Show modal
            $('#orderDeleteModal').modal('hide');
        },
        error: function (err) {
            $("#orderDeleteError").removeClass("d-none").text(err.responseText || "!Internal serval error. Please try after sometime.");
        }
    });
});

function editOrder(id) {
    window.location.href = "/Order/OrderEditView?orderId=" + id;
}                                