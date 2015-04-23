
function loadPage(container, pageNr) {
    container = $(container);

    $.get(container.attr("data-url"), { page: pageNr }, function (response) {
        $(container).html(response);
    });
}

function nextPage(elem) {
    loadPage($(elem).closest(".paging-container"), currentPage(elem) + 1);
}

function previousPage(elem) {
    loadPage($(elem).closest(".paging-container"), currentPage(elem) - 1);
}

function currentPage(elem) {
    return parseInt($(elem).closest(".pager").attr("data-current-page"));
}