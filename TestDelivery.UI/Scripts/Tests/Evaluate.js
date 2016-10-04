function loadTestEvaluation() {
    var container = $(".main-container");
    var url = container.attr("data-url");
    $.get(url, {}, function (response, status, xhr) {
        if(status !== "success")
            return;
        container.html(response);
    });
}

function markCorrectAnswer(url) {
    $.post(url, {}, function () {
        loadTestEvaluation();
    });
}

function markIncorrectAnswer(url) {
    $.post(url, {}, function () {
        loadTestEvaluation();
    });
}

function evaluateAnswer(url, elem) {
    var score = $(elem).closest(".evaluate-answer").find("input[name=score]").val();
    $.post(url, { score: score }, function () {
        loadTestEvaluation();
    });
}

function convertMarkdown(textareaSelector, destinationSelector) {
    var markdown = $(textareaSelector).val().trim();
    var converter = new Markdown.Converter();
    $(textareaSelector)
        .parent()
        .find(destinationSelector)
        .html(converter.makeHtml(markdown));
}

