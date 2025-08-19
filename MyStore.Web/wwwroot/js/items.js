$(document).ready(function () {
    $('#itemGrid').DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Item/LoadItemGridData",
            "type": "POST",
            "dataType": "json"
        },
        "columns": [
            { "data": "itemName", "name": "ItemName", "autoWidth": true },
            { "data": "category.categoryName", "name": "CategoryName", "autoWidth": true, "orderable": false },
            { "data": "price", "name": "Price", "autoWidth": true },
            {
                "data": "createdOn",
                "name": "CreatedOn",
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
            {
                "data": "id",
                "render": function (data, type, row) {
                    return `
                                <button class="btn btn-sm me-1" onclick="editItem('${data}')"><i class="bi bi-pen-fill"></i></button>
                                <button class="btn btn-sm" onclick="deleteItem('${data}')"><i class="bi bi-trash-fill"></i></button>
                            `;
                },
                "orderable": false,
                "searchable": false,
                "width": "150px"
            }
        ]
    })
});



function openAddNewModal() {
    $("#formAddItemError").addClass("d-none").text("");
    var categoryDropdown = $('#categorySelect');
    categoryDropdown.html(`
        <option value="">Select Category</option>
    `);
    $.ajax({
        method: "GET",
        url: "/Category/GetAllCategory",
        success: function (response) {
            response.forEach((item) => {
                categoryDropdown.append(`
                    <option value="${item.id}">${item.categoryName}</option>
                `);
            });
            $('#itemAddModal').modal('show');
        },
        error: function (err) {
            console.log("error : ", err.responseText);
        }
    });
}

$('#categoryAddForm').validate({
    rules: {
        categoryId: {
            required:true
        },
        itemName: {
            required:true
        },
        price: {
            required: true,
            min:1
        }
    },
    messages: {
        categoryId: {
            required: "Category is required."
        },
        itemName: {
            required: "Item name is required."
        },
        price: {
            required: "Price is required.",
            min:"Price must be greater than zero."
            
        }
    },
    submitHandler: function (form, event)
    {
        event.preventDefault();

        $.ajax({
            method: "POST",
            url: "/Item/AddNewItem",
            data: $(form).serialize(),
            success: function (response) {
                // reload DataTable if needed
                $('#itemGrid').DataTable().ajax.reload();

                // reset the form
                form.reset();
                $("#formAddItemError").addClass("d-none").text("");
                $('#itemAddModal').modal('hide');
            },
            error: function (err) {
                $("#formAddItemError").removeClass("d-none").text(err.responseText || "Error while saving category");
            }

        })

    }
});

$('#categoryUpdateForm').validate({
    rules: {
        categoryId: {
            required: true
        },
        itemName: {
            required: true
        },
        price: {
            required: true,
            min: 1
        }
    },
    messages: {
        categoryId: {
            required: "Category is required."
        },
        itemName: {
            required: "Item name is required."
        },
        price: {
            required: "Price is required.",
            min: "Price must be greater than zero."

        }
    },
    submitHandler: function (form, event) {
        event.preventDefault();

        $.ajax({
            method: "PUT",
            url: "/Item/UpdateItem?itemId=" +$(form).find("[name='id']").val(),
            data: $(form).serialize(),
            success: function (response) {
                // reload DataTable if needed
                $('#itemGrid').DataTable().ajax.reload();

                // reset the form
                form.reset();
                $("#formUpdateItemError").addClass("d-none").text("");
                $('#itemUpdateModal').modal('hide');
            },
            error: function (err) {
                $("#formUpdateItemError").removeClass("d-none").text(err.responseText || "Error while saving category");
            }

        })

    }
});

function deleteItem(id) {
    $("#hiddenItemId").val(id);
    $("#formDeleteItemError").addClass("d-none").text("");
    $('#itemDeleteModal').modal('show');
}

$('#btnItemDelete').click(function () {
    var itemId = $("#hiddenItemId").val();
    $.ajax({
        method: "DELETE",
        url: "/Item/DeleteItem?itemId=" + itemId,
        success: function (response) {
            // reload DataTable if needed
            $('#itemGrid').DataTable().ajax.reload();

            $("#formDeleteItemError").addClass("d-none").text("");
            $('#itemDeleteModal').modal('hide');
        },
        error: function (err) {
            $("#formDeleteItemError").removeClass("d-none").text(err.responseText || "Error while saving category");
        }
    });
});