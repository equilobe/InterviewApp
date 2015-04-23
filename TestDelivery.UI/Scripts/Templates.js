
function startNewTemplate() {
    $(".new-template-form").show();
    $(".new-template-button").hide();
}

function cancelNewTemplate() {
    $(".new-template-form").hide();
    $(".new-template-button").show();
}

function deleteItem(url) {
    var ok = confirm("Delete item and all related data?");
    if (!ok)
        return;
    window.location = url;
}