var buttonPlus = Array.from(document.querySelectorAll(".btn-quantity-plus"));
buttonPlus.forEach(function (button) {
    button.addEventListener('click', function () {
        var quantities = getValues('.item-quantity');
        var valuesUponChange = [];
        quantities.forEach(function (quantity) {
            valuesUponChange.push(quantity)
        });

        var index = buttonPlus.indexOf(button);
        valuesUponChange[index] += 1;
        
        updateSubtotal(index, valuesUponChange[index]);
        updateTotal();
        totalAll();

        var ids = document.querySelectorAll("[data-cart-detail-id]")[index];
        var id = ids.getAttribute("data-cart-detail-id");
        console.log(id);
        var data = {
            id: id,
            quantity: 1
        };


        fetch("/Cart/UpdateCart", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        })
            .then(function (response) {
                if (response.ok) {
                    return response.text();
                } else {
                    throw new Error("Error sending data to controller")
                }
            })
            .catch(function (error) {
                console.log("An error occurs: ", error)
            })
    })
});


var buttonMinus = Array.from(document.querySelectorAll(".btn-quantity-minus"));
buttonMinus.forEach(function (button) {
    button.addEventListener('click', function () {
        var quantities = getValues('.item-quantity');
        var valuesUponChange = [];
        quantities.forEach(function (quantity) {
            valuesUponChange.push(quantity);                        
        });

        var index = buttonMinus.indexOf(button);
        if (valuesUponChange[index] > 1) valuesUponChange[index] -= 1;
        else button.disabled == true;
        
        updateSubtotal(index, valuesUponChange[index]);
        updateTotal();
        totalAll();       

        var ids = document.querySelectorAll("[data-cart-detail-id]")[index];
        var id = ids.getAttribute("data-cart-detail-id");
        var data = {
            id: id,
            quantity: -1
        };


        fetch("/Cart/UpdateCart", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        })
            .then(function (response) {
                if (response.ok) {
                    return response.text();
                } else {
                    throw new Error("Error sending data to controller")
                }
            })
            .catch(function (error) {
                console.log("An error occurs: ", error)
            })

    })
});



var removeBtns = Array.from(document.querySelectorAll(".btn-remove"));
removeBtns.forEach(function (btn, index) {
    btn.addEventListener("click", function () {
        var lineToRemove = document.querySelectorAll(".item-row");
        if (lineToRemove.length >= index) {
            var ids = document.querySelectorAll("[data-cart-detail-id]")[index];
            var id = ids.getAttribute("data-cart-detail-id");
            console.log(id);
            var data = {
                id: id
            };
            lineToRemove[index].remove();

            fetch("/Cart/RemoveCartLine", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            })
                .then(function (response) {
                    if (response.ok) {
                        return response.text();
                    } else {
                        throw new Error("Error sending data to controller")
                    }
                })
                .catch(function (error) {
                    console.log("An error occurs: ", error)
                })

            updateTotal();
            totalAll();
        }
        else {
            console.log("No lines to remove");
        }
    });
});

document.querySelector("body").addEventListener('onload', updateTotal());
document.querySelector("body").addEventListener('onload', totalAll());

function getValues(className) {
    var values = [];
    var list = document.querySelectorAll(className);
    list.forEach(function (item) {
        values.push(parseInt(item.value));
    });
    return values;
}

function updateSubtotal(index, quantity) {
    var subtotalValues = Array.from(document.querySelectorAll(".item-subtotal"));
    var prices = document.querySelectorAll(".item-price");
    var updatedSubtotal = quantity * parseInt(prices[index].textContent.replace(/,/g, ""));
    subtotalValues[index].innerText = updatedSubtotal.toLocaleString('en-US') + "₫";
}

function updateTotal() {
    var subtotal = 0;
    var subtotalValues = document.querySelectorAll(".item-subtotal");
    subtotalValues.forEach(function (value) {
        subtotal += parseInt(value.textContent.replace(/,/g, ""));
    });

    document.getElementById('subtotal-all').innerText = subtotal.toLocaleString('en-US') + "₫";
}

function totalAll() {
    var shippingFee = parseInt(document.getElementById("shipping-fee").textContent.replace(/,/g, ""));
    var allSubtotals = parseInt(document.getElementById("subtotal-all").textContent.replace(/,/g, ""));
    var total = shippingFee + allSubtotals;
    console.log(total);
    document.getElementById('total-all').innerText = total.toLocaleString('en-US') + "₫";
}