
const $upload = $('#upload'),
    $crop = $('#crop'),
    $result = $('#result'),
    $croppie = $('#croppie');

var cr,info,
    cr_img = '',
    img_w = 320,
    img_h = 320,
    isCrop = 0;

var photo = document.getElementById("photo"),
    filedrag = document.getElementById("filedrag");

photo.addEventListener("change", FileSelectHandler, false);

function FileSelectHandler(e) {
    // fetch FileList object
    var files = e.target.files;
    console.log(files);
    console.log(e);
    var reader = new FileReader();
    reader.onload = function (e) {
        $upload.hide();
        if (cr_img == '') {
            cr_img = e.target.result;
            console.log(e);
            console.log(e.target);
            console.log(cr_img);
            cropInit();
        }
        else {
            cr_img = e.target.result;
            bindCropImg();
        }
        $crop.fadeIn(300);
    }
    reader.readAsDataURL(files[0]);
    console.log(reader.readAsDataURL);
}

// 裁切設定
function cropInit() {
    cr = $croppie.croppie({
        viewport: {
            width: img_w,
            height: img_h,
            type: 'circle'
        },
        boundary: {
            width: img_w,
            height: img_h,
            type: 'circle'
        },
        mouseWheelZoom: true,
        enableOrientation: true
    });

    $(".cr-slider-wrap").append('<button type="button" id="cr-rotate" class="btn btn-dark ml-1" onClick="cropRotate(-90);">↻ Rotate</button>');

    bindCropImg();
}

// 綁定圖片
function bindCropImg() {
    cr.croppie('bind', {
        url: cr_img
    });
}

// 旋轉按鈕
function cropRotate(deg) {
    cr.croppie('rotate', parseInt(deg));
}

// 取消裁切
function cropCancel() {
    if ($upload.is(':hidden')) {
        $upload.fadeIn(300).siblings().hide();
        $('#crop_close').hide();
        photo.value = "";
        isCrop = 0;
    }
}

// 圖片裁切
function cropResult() {
    if (!isCrop) {
        isCrop = 1;
        cr.croppie('result', {
            type: 'canvas',
            //size: 'viewport', 
            format: 'png',
            quality: 1,
            circle: true
        }).then(function (resp) {
            console.log(resp);  //change
            console.log(cr_img); //origin
            console.log(photo.files[0]);
            cr_img = resp;
            console.log();
            $crop.hide();
            $result.find('img').attr('src', resp);
            $result.fadeIn(300);
            $('#crop_close').fadeIn(300);
        });
    }
}