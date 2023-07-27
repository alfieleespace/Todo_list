$(document).ready(function () {
    $(".checkbox").change(function () {
        let self = $(this);
        let id = self.attr("id");
        let isDone = self.prop("checked");
        $.ajax({
            url: "TodoItems/AjaxEdit",
            data: {
                id: id,
                isDone: isDone
            },
            type: "POST",
            success: function (result) {
                $(".tableDiv").html(result);
            }
        })
    })

    $(".btn-delete").click(function () {
        let self = $(this);
        let id = self.attr("id");
        $.ajax({
            type: "DELETE",
            url: "TodoItems/AjaxDelete",
            data: { id: id },
            success: function (result) {
                $(".tableDiv").html(result);
            }
        })
    })
})


