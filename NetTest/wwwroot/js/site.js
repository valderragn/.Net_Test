// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function previewImage(event) {
    const input = event.target;
    const reader = new FileReader();
    reader.onload = function () {
        const dataURL = reader.result;
        const imagePreview = document.getElementById('imagePreview');
        imagePreview.src = dataURL;
        imagePreview.style.display = 'block';
    };
    reader.readAsDataURL(input.files[0]);

}
function toggleEdit(button) {
    var card = button.closest('.card-body');
    var displayElements = card.querySelectorAll('.display-mode');
    var editElements = card.querySelectorAll('.edit-mode');
    var saveButton = card.querySelector('.save-btn');

    displayElements.forEach(el => el.style.display = el.style.display === 'none' ? '' : 'none');
    editElements.forEach(el => el.style.display = el.style.display === 'none' ? '' : 'none');
    button.style.display = button.style.display === 'none' ? '' : 'none';
    saveButton.style.display = saveButton.style.display === 'none' ? '' : 'none';
}

function saveChanges(button, id) {
    var card = button.closest('.card-body');
    var dataName = card.querySelector('input[type="text"]').value;
    var dataDesc = card.querySelector('textarea').value;
    var dataImage = card.querySelector('input[type="file"]').files[0];
    var url = card.getAttribute('data-edit-url');
    var token = $('input[name="__RequestVerificationToken"]').val();

    var formData = new FormData();
    formData.append('DataId', id);
    formData.append('DataName', dataName);
    formData.append('DataDesc', dataDesc);
    if (dataImage) {
        formData.append('DataImage', dataImage);
    }
    formData.append('__RequestVerificationToken', token);

    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                window.location.href = '/Home';
            } else {
                console.error("Error: " + response.message);
            }
        },
        error: function (error) {
            console.error(error);
        }
    });
}

