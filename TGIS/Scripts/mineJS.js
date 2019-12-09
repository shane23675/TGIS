$('.rg').mousedown(function (e) {
    var tar = e.currentTarget.children;
    console.log(tar);
    $('.rg>label').removeClass().addClass('btn btn-dark');
    $(tar).removeClass().addClass('btn btn-warning');
});


// 計算剩餘字數
$('.countResult').text($('.textCount').attr('maxlength') + ' words left');
$('.textCount').keyup(function () {
    let count = $(this).val().length;
    let result = $(this).attr('maxlength') - count;
    if (result != 0)
        $('.countResult').text(result + ' words left');
    else
        $('.countResult').text('Stop typeing,ASSHOLE!!!');
});

