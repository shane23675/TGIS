//將所有<input type="date" />的value中的"/"替換為"-"，才能正確顯示
$("input[type='date']").each(function () {
    var date = $(this).attr("value");
    $(this).val(date.replace(/\//g, "-"));
})

//將所有<input type="datetime-local" />的value中的"/"替換為"-"，日期和時間之間加上"T"，才能正確顯示
$("input[type='datetime-local']").each(function () {
    var date = $(this).attr("value");
    $(this).val(date.replace(/\//g, "-").replace(" ", "T"));
})
