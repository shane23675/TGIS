$('.rg').mousedown(function (e) {
    var tar = e.currentTarget.children;
    console.log(tar);
    $('.rg>label').removeClass().addClass('btn btn-dark');
    $(tar).removeClass().addClass('btn btn-warning');
});

//$('.rg').mousedown(function (e) {
//    var tar = e.currentTarget.children;
//    console.log(tar);
//    //$('.rg[input:radio]').removeClass('radioCheck radioUnCheck')
//    $('.rg>div').removeClass().addClass('btn btn-dark');
//    $(tar).removeClass().addClass('btn btn-warning');
//});