
var canEditTemplate = true;

function initCreateTemplatePage(canEdit) {

    canEditTemplate = canEdit ? true : false;

    $("#tabs").tabs();

    $(".selected-questions").sortable();
    $(".selected-questions").disableSelection();
    $("input[type=submit]").click(function (event) { onSubmitClick(this, event); });


    $(".editor-field .js-markdown-preview").each(function (index, element) {
        showPreview(element);
    });

    $(".editor-field textarea").autosize({ append: "\n" });

    $("form.js-template-form").submit(function saveTemplate(event) {
        var form = $(event.target);
        var url = form.attr("action");
        $.post(url, form.serialize());
        return false;
    });
}

var converter = new Markdown.Converter();

function showPreview(element) {
    var textarea = $(element).parent().find("textarea");    
    $(element).html(converter.makeHtml(textarea.val()));
    textarea.on('input', function (event) {
        $(event.target).parent().find(".js-markdown-preview").html(converter.makeHtml(event.target.value));
    });
}


function initQuestions() {
    $(QUESTION_SELECTOR).click(function (event) { onQuestionClick(this, event); })
                        .disableSelection();
}

function onQuestionClick(questionElement, event) {
    var question = $(questionElement);
    var questionId = question.attr("data-question-id");
    var selectedQuestion = $(".selected-questions .question[data-question-id=" + questionId + "]");
    var questionExists = selectedQuestion.length != 0;
    if (questionExists) {
        selectedQuestion.effect("highlight", {}, 500);
        return;
    }

    $(".selected-questions").append(question.clone().addClass("added"));
}

function shiftSelect(questionElement) {
    if (!lastSelectedQuestion || questionElement == lastSelectedQuestion) {
        toggleSelection(questionElement);
        return;
    }

    var startFound = false;
    var endFound = false;
    $(QUESTION_SELECTOR).map(function (index, question) {
        if (endFound)
            return;

        if (!startFound) {
            if (question == questionElement || question == lastSelectedQuestion)
                startFound = true;
        }
        else {
            if (question == lastSelectedQuestion || question == questionElement)
                endFound = true;
        }

        if (startFound)
            select(question);

    });
}

function toggleSelection(questionElement) {
    if (isSelected(questionElement))
        deselect(questionElement);
    else
        select(questionElement);
}

var QUESTION_SELECTOR = ".questions .question";
var SELECTED_CLASS = "selected";
var lastSelectedQuestion = null;

function isSelected(questionElement) {
    return $(questionElement).hasClass(SELECTED_CLASS);
}

function select(questionElement) {
    $(questionElement).addClass(SELECTED_CLASS);
    lastSelectedQuestion = questionElement;
}

function deselect(questionElement) {
    $(questionElement).removeClass(SELECTED_CLASS);
}

function getSelection() {
    return $(".selected-questions .question").map(function (index, question) {
        var q = $(question);
        var prefix = q.hasClass("added") ? "" : "@";
        return prefix + q.attr("data-question-id");
    }).toArray();
}

function beforeSubmit() {
    var selection = getSelection();
    $("input[name=QuestionIds]").val(selection);
}

function onSubmitClick(button, event) {
    beforeSubmit();
}

function deleteQuestion(elem) {
    if (!canEditTemplate)
        return;

    $(elem).closest(".item").remove();
}

function editQuestionInPlace(elem) {
    var questionId = $(elem).closest(".question").attr("data-question-id");
    var container = $(".edit-question-container");
    var url = container.attr("data-url")
                       .replace("****", questionId);

    editQuestionInPlaceUrl(url);
}

function editQuestionInPlaceUrl(url) {
    $.get(url, function (response) {
        $(".edit-question-container").html(response);

        $(".edit-question-container textarea").autosize({ append: "\n" });

        $(".edit-question-container .editor-field .js-markdown-preview").each(function (index, element) {
            showPreview(element);
        });
    });
}

function createNewTemplateQuestion(url) {
    $.getJSON(url, function (json) {
        reloadQuestionListItem(json.questionId);
    });
}

function reloadQuestionListItem(questionId) {
    var questionContainer = $(".selected-questions");
    var url = questionContainer.attr("data-url")
                    .replace("****", questionId);

    $.get(url, function (response) {
        var existingQuestion = questionContainer.find(".question.item[data-question-id='" + questionId + "']");
        if (existingQuestion.length > 0) {
            existingQuestion.replaceWith(response);
        }
        else {
            questionContainer.append(response);
            editQuestionInPlace(questionContainer.children().last());
        }
    });
}

function saveTemplateQuestion(elem, onSave) {
    var form = $(elem).closest(".edit-question-form");
    var url = form.attr("data-url");
    var questionId = form.find("input[name='editQuestion.Id']").val();

    if (typeof (onSave) == "function")
        onSave();

    $.post(url, form.find(":input").serialize(), function (response, status, xhr) {
        reloadQuestionListItem(questionId);
    });

    return true;
}
