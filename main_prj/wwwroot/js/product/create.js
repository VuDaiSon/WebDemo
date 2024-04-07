// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Display corresponding form for product specs
$(document).ready(function () {
    $('#Product_CategoryId').change(function () {
        var categoryId = $(this).val();
        $.ajax({
            url: '/Products/GetSpecsCreateView',
            type: 'GET',
            data: { categoryId: categoryId },
            success: function (result) {
                $('#partialViewCont').html(result);
            }
        });
    });
});

//Load images preview
$(document).ready(function () {
    var imageImput = document.getElementById("PreviewImage");
    var preview = document.getElementById("preview");

    imageImput.addEventListener('change', function () {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                preview.src = e.target.result;
            };
            reader.readAsDataURL(file);
        } else {
            preview.src = "#";
        }
    });

    var detailImages = document.getElementById("DetailImages");
    var detailPreviews = document.getElementById("detailPreviews");

    detailImages.addEventListener('change', function () {
        detailPreviews.innerHTML = '';
        const files = this.files;
        for (let i = 0; i < files.length; i++) {
            const file = this.files[i];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    const img = document.createElement("img");
                    img.width = img.height = 100;                    
                    img.classList.add("detail-preview");
                    img.src = e.target.result;
                    detailPreviews.appendChild(img);
                };
                reader.readAsDataURL(file);
            }
        }
    });
})

//Handle controller's response
/*$(document).ready(function () {
    var form = document.getElementById("productForm");
    form.addEventListener('submit', function () {
        $.get("/Products/Create", function (response) {
            if (response.success == true) {
                alert("Added successfully");
                *//*window.location.href = response.returnUrl;*//*
            } else if (response.success == false) {
                alert("An error has occured");
            }
        })
    })
    
})*/







//  OLD AJAX CODE FOR SENDING DATA TO CONTROLLER
/*$(document).ready(function () {
    $('#productForm').submit(function (e) {
        e.preventDefault(); // Prevent the default form submission

        var fullData = {};
        var data = {};        

        //IMAGE SUBMISSION
        *//*uploadImages();*//*
        
        // INFO SUBMISSION
        
        // Get product data
        data.ProductName = $("#Product_ProductName").val();
        data.Price = $("#Product_Price").val();
        data.StockQuantity = $("#Product_StockQuantity").val();
        data.ProductDescription = $("#Product_ProductDescription").val();
        data.Warranty = $("#Product_Warranty").val();
        data.CategoryId = parseInt($("#SelectedCategoryId").val(), 10);

        var spec = {};
        //Get specs data
        switch (data.CategoryId) {
            case 1:
                spec.Brand = $("#Monitor_Brand").val();
                spec.Model = $("#Monitor_Model").val();
                spec.Size = $("#Monitor_Size").val();
                spec.AspectRatio = $("#Monitor_AspectRatio").val();
                spec.Resolution = $("#Monitor_Resolution").val();
                spec.RefreshRate = $("#Monitor_RefreshRate").val();
                fullData = {
                    Product: data,
                    Monitor: spec
                };
                break;
            case 2:
                spec.Brand = $("#Headphone_Brand").val();
                spec.Model = $("#Headphone_Model").val();
                spec.Compatibilities = $("#Headphone_Compatibilities").val();
                spec.ConnectionType = $("#Headphone_ConnectionType").val();
                spec.Battery = $("#Headphone_Battery").val();
                spec.Accessories = $("#Headphone_Accessories").val();
                spec.Microphone = $("#Headphone_Microphone").val();
                spec.HeadphoneType = $("#Headphone_HeadphoneType").val();
                spec.Color = $("#Headphone_Color").val();
                fullData = {
                    Product: data,
                    Headphone: spec
                };
                break;
            case 3:
                spec.Brand = $("#Mouse_Brand").val();
                spec.Model = $("#Mouse_Model").val();
                spec.Weight = $("#Mouse_Weight").val();
                spec.ConnectionType = $("#Mouse_ConnectionType").val();
                spec.Battery = $("#Mouse_Battery").val();
                spec.Resolution = $("#Mouse_Resolution").val();                
                spec.Color = $("#Mouse_Color").val();
                fullData = {
                    Product: data,
                    Mouse: spec
                };
                break;
            case 4:
                spec.Brand = $("#Keyboard_Brand").val();
                spec.Model = $("#Keyboard_Model").val();
                spec.Weight = $("#Keyboard_Weight").val();
                spec.ConnectionType = $("#Keyboard_ConnectionType").val();
                spec.Battery = $("#Keyboard_Battery").val();
                spec.Switch = $("#Keyboard_Switch").val();
                spec.Led = $("#Keyboard_Led").val();
                spec.KeyboardType = $("#Keyboard_KeyboardType").val();
                spec.Color = $("#Keyboard_Color").val();
                fullData = {
                    Product: data,
                    Keyboard: spec
                };
                break;
            default:
                break;
        }

        console.log(fullData);
        var jsonData = JSON.stringify(fullData);

        // Send an AJAX request to the Create action
        $.ajax({
            url: "/Products/Create",
            type: 'POST',
            data: jsonData,            
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {

                if (response.success) {
                    alert("Added successfully");
                    // Redirect to the index page if the creation is successful
                    window.location.href = response.redirectUrl;
                } else {
                    // Display validation errors if the creation fails
                    // For simplicity, you can display errors in an alert box
                    alert(response.errors);
                }
            },
            error: function () {
                // Handle AJAX errors
                alert('An error occurred while processing the request.');
            }
        });                
    });

    
    function uploadImages() {


        var previewImage = document.getElementById("PreviewImage");
        var detailImages = document.getElementById("DetailImages");

        var formData = new FormData();
        formData.append('PreviewImage', previewImage.files[0]);

        for (var i = 0; i < detailImages.files.length; i++) {
            formData.append('DetailImages', detailImages.files[i]);
        }

        console.log(formData);

        $.ajax({
            url: "/FileHandling/SaveImages",
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                console.log('Image uploaded successfully', response);
            },
            error: function (xhr, status, error) {
                console.log('Error uploading images: ', error);
            }
        });
    }
});*/