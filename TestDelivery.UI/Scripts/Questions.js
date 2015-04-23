
var FreeTextQuestion = {};

FreeTextQuestion.addAnswerTemplate = function () {
    var answerTemplate = $(".answers .answer-template-container").html();
    $(".answers .answer-container").append(answerTemplate);
}

FreeTextQuestion.deleteAnswer = function (element) {
    $(element).closest(".answer").remove();
}

FreeTextQuestion.renameContainers = function (form) {

    var elem = form.find(".answer-container .answer")
                .each(function (index, element) {
                    $(element).find("input").each(function (_, input) {
                        input.name = input.name.replace("answertemplate", "CorrectAnswers[" + index + "]");
                    });
                });
}

FreeTextQuestion.onSave = function () {
    FreeTextQuestion.renameContainers($("form"));
    return true;
}

var MultipleChoiceQuestion = {};

MultipleChoiceQuestion.addAnswerTemplate = function () {
    var answerTemplate = $(".answers .answer-template-container").html();
    $(".answers .answer-container").append(answerTemplate);
}

MultipleChoiceQuestion.deleteAnswer = function (element) {
    $(element).closest(".answer").remove();
}

MultipleChoiceQuestion.renameContainers = function () {
    var elem = $(".answer-container .answer")
                .each(function (index, element) {
                    $(element).find("input").each(function (_, input) {
                        input.name = input.name.replace("answertemplate", "Answers[" + index + "]");
                    });
                });
}

MultipleChoiceQuestion.onSave = function () {
    MultipleChoiceQuestion.renameContainers();
    return true;
}

var Problem = {};

Problem.onSave = function () {
    return true;
}
