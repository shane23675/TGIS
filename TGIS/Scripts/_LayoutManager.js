//此為使用在_LayoutManager的js檔案

//點擊「多加一筆」按鈕後新增一個輸入框
$("#moreLinkBtn").click(function () {
    $("#linkInputBox").append('<br/><input type="text" name="links" class="form-control" />');
    return false;
})
