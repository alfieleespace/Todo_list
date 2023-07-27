$(document).ready(function () {

    $.ajax({
        url: "/TodoItems/BuildTable",
        success: function (result) {
            $(".tableDiv").html(result);
        }
    })
})

