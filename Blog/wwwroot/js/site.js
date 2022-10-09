let index = 0;
let tagValues = document.getElementById("TagValues")
let tagList = document.getElementById("TagList");

const AddTag = () => {
    let tagEntry = document.getElementById("TagEntry");
    let searchResult = search(tagEntry.value);
    if (searchResult) {
         Swal.fire({
            icon: 'error',
            title: 'ERROR!',
            text: searchResult,
            confirmButtonColor: '#212422',
            width: 300,
            timer: 3000,
            timerProgressBar: true,
        })
        tagEntry.value = "";
    } else {
        let newOption = new Option(tagEntry.value, tagEntry.value);
        tagList.options[index++] = newOption;
        tagEntry.value = "";
        return true;
    }
}

const DeleteTag = () => {
    let tagCount = 1;
    if (!tagList) return false;

    if (tagList.selectedIndex == -1) {
        Swal.fire({
            icon: 'error',
            title: 'error',
            text: 'Please choose a tag to delete!',
            confirmButtonColor: '#212422',
            width: 300,
            timer: 3000,
            timerProgressBar: true,
        });
        return true;
    }
    while (tagCount > 0) {
        if (tagList.selectedIndex >= 0) {
            tagList.options[tagList.selectedIndex] = null;
            tagCount--;
        } else
            tagCount = 0;
        index--;
    }
}

$("form").on("submit", () => {
    $("#TagList option").prop("selected", "selected");
})

if (tagValues != "") {
    let tagArray = tagValues.split(",");
    for (let i = 0; i < tagArray.length; i++) {
        ReplaceTag(tagArray[i], i);
        index++;
    }
}

const ReplaceTag = (tag, index) => {
    let newOption = new Option(tag, tag);
    document.getElementById("TagList").options[index] = newOption;
}

function search(str) {
    if (str == "") return "Please enter a non-empty tag.";
    if (tagList) {
        let options = tagList.options;
        for (let i = 0; i < options.length; i++) {
            if (options[i].value == str) return `#${str} is a duplicate.`;
        }
    }
}
