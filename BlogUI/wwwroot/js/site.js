// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let index = 0;

function AddTag() {
    let tagItems = document.getElementById("TagItems");
    let tagList = document.getElementById("TagList");
    let newOption = new Option(tagItems.value, tagItems.value);
    tagList.options[index++] = newOption;
    tagItems.value = "";
    return true;
}

const DeleteTag = () => {
    let tagCount = 1;
    let tagList = document.getElementById("TagList");
    while (tagCount > 0) {
        let selectedIndex = tagList.selectedIndex;
        if (selectedIndex >= 0) {
            tagList.options[selectedIndex] = null;
            tagCount--;
        } else {
            tagCount = 0;
            index--;
        }
        
    }
}


$("form").on("submit", function() {
    $("#TagList option").prop("selected", "selected");
})