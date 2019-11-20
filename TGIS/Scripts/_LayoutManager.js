//此為使用在_LayoutManager的js檔案

//點擊「多加一筆」按鈕後新增一個輸入框
$("#moreLinkBtn").click(function () {
    $("#linkInputBox").append('<br/><input type="text" name="links" class="form-control" placeholder="新增一項連結..." />');
    return false;
})
//其他
$(document).ready(function () {
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
        $('#sidebar>ul>li>.toggle').toggleClass('dropdown-toggle');
    });
});


