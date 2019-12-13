//修正店家及桌遊篩選時無法使用分頁的問題

//找到所有的分頁用連結<a>
var pageLinks = $(".pagination  a");
for (var i = 0; i < pageLinks.length; i++) {
    //找到該連結的href的最後一個字即為頁碼
    let link = pageLinks.eq(i);
    let href = link.attr("href");
    if (href == undefined)
        continue;
    //刪除該連結的原本效果
    link.attr("href", "javascript:;");
    let page = parseInt(href[href.length - 1]);
    link.click(function () {
        //將<input id="page">的value改為該頁碼
        $("#page").val(page);
        //送出表單Post請求以達到篩選效果
        $("#filter").submit();
    })
}
