
//判別搜尋項目

$(document).delegate('input', 'propertychange input', function () {

    if (this.id == 'GameSearch') {
        SearchTG($(this).val().trim());
    } else if (this.id == 'ShopSearch') {
        SearchShop($(this).val().trim());
    } else {
        return;
    }

    //輸入無值時，清空id not yet
    if ($('#GameSearch').val() == "") {
        $('#searchedTableGameID').attr('name', '');
        $('#searchedTableGameID').val("");
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
            $('#TGResultList').append('<a class="text-secondary">查無此桌遊</a>');
            return;
        }

        //顯示搜尋結果
        $('#TGResultList').css('z-index', '10');

        if (location.pathname == "/Shop/ShopIndexForPlayer" || location.pathname == "/Analysis/TableGameTrend") {
            for (i = 0; i < data.length; i++) {
                let result = '<a javascipt:;  id="' + data[i].ID + '" class="list-group-item">' + data[i].ChineseName + '</a>';
                $('#TGResultList').append(result);
            }
        }
        else {
            for (i = 0; i < data.length; i++) {
                let result = '<a class="list-group-item" href="' + data[i].Link + '">' + data[i].ChineseName + '</a>';
                $('#TGResultList').append(result);
            }
        }



    })
}

//點擊選擇搜尋結果(店家頁面&店家桌遊分析)
var TGIdList = [];
$('#TGResultList').click(function (evt) {
    let searchTG = evt.target;
    if (location.pathname == "/Shop/ShopIndexForPlayer") {
        if (searchTG.id == "")
            return;
        $('#searchedTableGameID').attr('name', 'searchedTableGameID');
        $('#searchedTableGameID').val(searchTG.id);
        $('#GameSearch').val(searchTG.text);
        $('#TGResultList').empty();
    } else {
        let addCheck = TGSelect(searchTG.id);
        if (addCheck == 'ok') {
            $('#TGSearchForShop').append("<input name='tableGameIDs' id=" + searchTG.id + " type='hidden' value=" + searchTG.id + " class='btn btn-success p-1 mr-1 mb-1' />");
            $('#TGSearchForShop').append(" <span id=" + searchTG.id + " class='btn btn-success p-1 mr-1 mb-1' style='cursor:pointer'>" + searchTG.text + "<i class='fas fa - times'></i></span>");
            $('#TGResultList').empty();
            $('#GameSearch').val("");
        } else {
            return alert(addCheck);
        }
    }
});

//店家桌遊分析判斷桌遊是否重複選取&超出選取上限
function TGSelect(tgid) {
    if (TGIdList.indexOf(tgid) == -1 && TGIdList.length < 5) {
        TGIdList.push(tgid);
        return 'ok';
    } else {
        return '新增失敗!\n提醒：\n1.桌遊不可重複新增\n2.一次最多只可查詢5筆桌遊';
    }
}



function SearchShop(keyword) {
    $.post("/Shop/SearchShopByName", { name: keyword }, function (data) {
        $('#ShopResultList').empty();

        //空值時不顯示結果
        if (keyword == "") {
            return;
        } else if (data.length == 0) {
            $('#ShopResultList').append('<a class="text-secondary">查無此店家</a>');
            return;
        }

        //顯示搜尋結果
        for (i = 0; i < data.length; i++) {
            let result = '<a class="list-group-item" href="' + data[i].Link + '">' + data[i].ShopName + '</a>';
            $('#ShopResultList').append(result);
        }
    })
}


//店家查看桌遊點擊，取消已選取桌遊
$('#TGSearchForShop').click(function (evt) {
    let searchTG = evt.target.id;
    $('#TGSearchForShop>input[value="' + searchTG + '"]').remove();
    $('#TGSearchForShop>span[id="' + searchTG + '"]').remove();
})
