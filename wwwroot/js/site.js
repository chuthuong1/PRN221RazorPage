
var connection = new signalR.HubConnectionBuilder().withUrl("/CartHub").build();

connection.on("CartUpdated", function (productId, unitsInStock) {
    // Cập nhật số lượng sản phẩm trong bảng
    console.log("quantity" + unitsInStock);
    var Pid = document.querySelector(`[pid='${productId}']`);
    console.log("Pid"+Pid);
    if (Pid != null) {
        var qty = Pid.querySelector(".quantityAbc");
        console.log(qty);
        if (qty) {
            qty.innerText = unitsInStock;
        }

    }
    console.log("qty"+qty);

});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

