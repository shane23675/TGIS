//離開編輯器前若有修改則跳小窗重複確認

var isChange = false;
$(function () {
    $("input,textarea,select").change(function () {
        isChange = true;
        $(this).addClass("editing");
    });

    $(window).bind('beforeunload', function () {
        if (isChange || $(".editing").get().length > 0) {
            return '更改尚未儲存，確定離開？';
        }
    })
});
$("input:submit").click(function () {
    $(window).unbind('beforeunload');
});

