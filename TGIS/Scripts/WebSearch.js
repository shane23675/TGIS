
//判別搜尋項目

$(document).delegate('input', 'propertychange input', function () {
    if (this == GameSearch) {
        SearchTG($(this).val());
    } else {
        SearchShop($(this).val());
    }
})


function SearchTG(keyword) {
    $.post("/TableGame/SearchTableGameByName", { name: keyword }, function (data) {
        $('#TGResultList').empty();
        $('#TGResultList').css('z-index', '-1');
        //空值時不顯示結果
        if (keyword == "" || keyword == " ") {
            return;
        } else if (data.length == 0) {
            $('#TGResultList').css('z-index', '10');
            $('#TGResultList').append('<a>查無此桌遊</a>');
            return;
        }

        //顯示搜尋結果
        $('#TGResultList').css('z-index', '10');
        for (i = 0; i < data.length; i++) {
            let result = '<a href="' + data[i].Link + '">' + data[i].ChineseName + '</a>';
            
            $('#TGResultList').append(result);
        }
    })
}

function SearchShop(keyword) {
    $.post("/Shop/SearchShopByName", { name: keyword }, function (data) {
        $('#ShopResultList').empty();

        //空值時不顯示結果
        if (keyword == "" || keyword == " ") {
            return;
        } else if (data.length == 0) {
            $('#ShopResultList').append('<a>查無此店家</a>');
            return;
        }

        //顯示搜尋結果
        for (i = 0; i < data.length; i++) {
            let result = '<a href="' + data[i].Link + '">' + data[i].ShopName + '</a>';
            console.log(result);
            $('#ShopResultList').append(result);
        }
    })
}

