
var Test = {};

Test.goNext = function() {
    $.post(nextUrl, function (responseText){
        $(".container").html(responseText);
    });
}

Test.goNextWithAnswer = function () {
    window.clearTimeout(SD);
    var data = jQuery("#question :input").serialize();
    $.post(nextUrl, data, function (responseText) {
        $(".container").html(responseText);
    });
}

Test.init = function(){
    $.get(startUrl, function (responseText) 
    { 
        $(".container").html(responseText);
    });
};