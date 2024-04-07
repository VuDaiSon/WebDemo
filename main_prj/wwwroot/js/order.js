// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    
    document.getElementById('btn-submit').addEventListener('click', function () {
        var buyerName = document.getElementById('buyer-name').value.trim();
        var buyerEmail = document.getElementById('buyer-email').value.trim();
        var buyerAddress = document.getElementById('buyer-address').value.trim();
        var buyerNumber = document.getElementById('buyer-number').value.trim();
        console.log(buyerName);

        document.getElementById('buyer-number-validate').innerText = "";
        document.getElementById('buyer-name-validate').innerText = "";
        document.getElementById('buyer-address-validate').innerText = "";
        document.getElementById('buyer-email-validate').innerText = "";

        if (buyerName == "") {
            var nameErr = "Vui lòng nhập tên";
            document.getElementById('buyer-name-validate').innerText = nameErr;
            document.getElementById('buyer-name').focus();
        }
        else if (buyerEmail == "") {
            var emailErr = "Vui lòng nhập email";
            document.getElementById('buyer-email-validate').innerText = emailErr;
            document.getElementById('buyer-email').focus();
        }
        else if (buyerNumber == "") {
            var numberErr = "Vui lòng nhập số điện thoại";
            document.getElementById('buyer-number-validate').innerText = numberErr;
            document.getElementById('buyer-number').focus();
        }
        else if (buyerAddress == "") {
            var addressErr = "Vui lòng nhập địa chỉ";
            document.getElementById('buyer-address-validate').innerText = addressErr;
            document.getElementById('buyer-address').focus();
        }
        else {
            if (!document.getElementById('shipto').checked) {
                var data = {
                    name: buyerName,
                    orderDate: new Date(),
                    totalValue: @Model.Total()
                };
                console.log(data);
            }
        }


    })

    function isValidEmail(email) {
        // Regular expression for basic email validation
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }
});
