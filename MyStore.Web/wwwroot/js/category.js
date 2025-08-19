    $(document).ready(function () {
                $('#categoryGrid').DataTable({
                    "processing": true,
                    "serverSide": true,
                    "filter": true,
                    "ajax":{
                        "url":"/Category/LoadCategoryData",
                        "type":"POST",
                        "dataType":"json"
                    },
                    "columns":[
                        { "data": "categoryName", "name": "CategoryName", "autoWidth": true },
                        {
                            "data": "createdOn",
                            "name": "CreatedOn",
                            "autoWidth": true,
                            "render": function (data) 
                            {
                                if (data) 
                                {
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
                            "render": function (data, type, row) 
                            {
                                return `
                                    <button class="btn btn-sm me-1" onclick="editCategory('${data}')"><i class="bi bi-pen-fill"></i></button>
                                    <button class="btn btn-sm" onclick="deleteCategory('${data}')"><i class="bi bi-trash-fill"></i></button>
                                `;
                            },
                            "orderable": false,
                            "searchable": false,
                            "width": "150px"
                        }
                    ]
                })
    });

$('#saveCategoryBtn').click(()=>{
    $("#formError").addClass("d-none").text("");
});

$('#categoryAddForm').validate({
    rules: {
        categoryName: {
            required:true
        }
    },
    messages: {
        categoryName: {
            required: "Category name is required."
        }
    },
    submitHandler: function (form, event) {
       event.preventDefault(); 
        $("#formError").addClass("d-none").text("");
        $.ajax({
            url: '/Category/AddCategory',
            type: 'POST',
            data: $(form).serialize(),
            success: function (response) {

                // reload DataTable if needed
                $('#categoryGrid').DataTable().ajax.reload();

                // reset the form
                form.reset();
                $("#formError").addClass("d-none").text("");
                $('#categoryAddModal').modal('hide');
            },
            error: function (err) {
                $("#formError").removeClass("d-none").text(err.responseText || "Error while saving category");
            }
        });
    }
});



$('#categoryUpdateForm').validate({
    rules: {
        categoryName: {
            required: true
        }
    },
    messages: {
        categoryName: {
            required: "Category name is required."
        }
    },
    submitHandler: function (form, event) {
        event.preventDefault();
        $("#formError").addClass("d-none").text("");

        $.ajax({
            url: '/Category/UpdateCategory',
            type: 'POST',
            data: $(form).serialize(),
            success: function (response) {
                // reload DataTable if needed
                $('#categoryGrid').DataTable().ajax.reload();

                // reset the form
                form.reset();
                $("#formEditError").addClass("d-none").text("");
                $('#categoryEditModal').modal('hide');
            },
            error: function (err) {
                $("#formEditError").removeClass("d-none").text(err.responseText || "Error while updating category");
            }
        });
    }
});

function editCategory(id) {
    $("#formEditError").addClass("d-none").text("");
    $.ajax({
        url: '/Category/GetCategoryDetail?categoryId=' + id,
        type: 'GET',
        success: function (response) {
            // Populate form fields
            populateForm("#categoryUpdateForm", response);

            // Hide old error messages
            $("#formError").addClass("d-none").text("");

            // Show modal
            $('#categoryEditModal').modal('show');
        },
        error: function () {
            $("#formError").removeClass("d-none").text("Error loading category.");
        }
    });
    
}

function populateForm(formSelector, data) {
    $(formSelector)[0].reset(); // clear old values first
    $.each(data, function (key, value) {
        let $field = $(`${formSelector} [name='${key}']`);
        if ($field.length) {
            if ($field.attr("type") === "date") {
                let date = new Date(value);
                value = date.toISOString().split('T')[0]; // format to yyyy-MM-dd
            }
            $field.val(value);
        }
    });
}

function deleteCategory(id) {
    $('#hiddenCategoryId').val(id);
    $("#formDeleteError").addClass("d-none").text("");
    $('#categoryDeleteModal').modal('show');
}

$('#btnCategoteDelete').click(() => {
    var categoryId = $('#hiddenCategoryId').val();
    $.ajax({
        url: '/Category/RemoveCategory?categoryId=' + categoryId,
        type: 'DELETE',
        success: function (response) {
            // reload DataTable if needed
            $('#categoryGrid').DataTable().ajax.reload();

            // Hide old error messages
            $("#formDeleteError").addClass("d-none").text("");

            // Show modal
            $('#categoryDeleteModal').modal('hide');
        },
        error: function (err) {
            $("#formDeleteError").removeClass("d-none").text(err.responseText || "!Internal serval error. Please try after sometime.");
        }
    });
});

function checkClick() {
    console.log("btn clicked........")
}