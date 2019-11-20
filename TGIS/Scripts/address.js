//選取縣市時會出現行政區的ajax
init();
$("#CityID").change(init);
function init() {
    CId = $("#CityID").val();
    Did = $("#DistrictID").val();
    $.post("/ajax/generateStateList", { CId, Did }, callback)
}
function callback(val) {
    $("#DistrictID").html(val);
}
