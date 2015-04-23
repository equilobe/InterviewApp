var EditableList = {};

EditableList.addItemClicked = function (button) {
    var list = $(button).closest(".list.editable");
    EditableList.addItem(list);
};

EditableList.addItem = function (list) {
    var templateContent = list.children(".template").first().html();
    list.append(templateContent);    
    EditableList.renameContainers(list);
};

EditableList.deleteItemClicked = function (button) {
    var list = $(button).closest(".list.editable");
    $(button).closest(".item").remove();
    EditableList.renameContainers(list);
};

EditableList.renameContainers = function (list) {
    list = $(list);
    var collectionName = list.attr("data-property-name");
    var level = parseInt(list.attr("data-property-level"))

    var elem = list.children(".item")
                .each(function (index, element) {
                    $(element).find(":input").each(function (_, input) {
                        var startIndex = level == 0 ? -1 : getNthIndexOf(input.name, ".", level);
                        var endIndex = getNthIndexOf(input.name, ".", level + 1);
                        if (endIndex == -1)
                            endIndex = input.name.length;
                        var textBefore = input.name.substring(0, startIndex + 1);
                        var textAfter = input.name.substring(endIndex, input.name.length);
                        input.name = textBefore + collectionName + "[" + index + "]" + textAfter;
                    });
                });
};

function getNthIndexOf(str, strToFind, n) {
    var lastPos = -1;
    for (i = 0; i < n; i++) {
        var lastPos = str.indexOf(strToFind, lastPos + 1);
        if (lastPos < 0)
            break;
    }
    return lastPos;
}


var Editables = {};

Editables.initWithPreview = function (container) {
    container = $(container);
    container.editables(
    {
        beforeFreeze: function (display) {
            display.html(this.val());
        }
    });
};
