
//判別搜尋項目

$(document).delegate('input', 'propertychange input', function () {

    if (this.id == 'GameSearch') {
        SearchTG($(this).val().trim());
    } else {
        SearchShop($(this).val().trim());
    }
})


function SearchTG(keyword) {
    $.post("/TableGame/SearchTableGameByName", { name: keyword }, function (data) {
        $('#TGResultList').empty();
        $('#TGResultList').css('z-index', '-1');
        //空值時不顯示結果
        if (keyword == "") {
            return;
        } else if (data.length == 0) {
            $('#TGResultList').css('z-index', '10');
            $('#TGResultList').append('<a>查無此桌遊</a>');
            return;
        }

        //顯示搜尋結果
        $('#TGResultList').css('z-index', '10');

        if (location.pathname == "/Shop/ShopIndexForPlayer") {
            for (i = 0; i < data.length; i++) {
                let result = '<a href="#" id="' + data[i].ID + '">' + data[i].ChineseName + '</a>';
                $('#TGResultList').append(result);
            }
        }
        else {
            for (i = 0; i < data.length; i++) {
                let result = '<a href="' + data[i].Link + '">' + data[i].ChineseName + '</a>';

                $('#TGResultList').append(result);
            }
        }



    })
}

$('#TGResultList').click(function (evt) {
    let searchTG = evt.target;
    if (searchTG.id == "")
        return;
    $('#searchedTableGameID').val(searchTG.id);
    $('#GameSearch').val(searchTG.text);
    $('#TGResultList').empty();
});





function SearchShop(keyword) {
    $.post("/Shop/SearchShopByName", { name: keyword }, function (data) {
        $('#ShopResultList').empty();

        //空值時不顯示結果
        if (keyword == "") {
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

