
//Load images preview
$(document).ready(function () {
    var imageImput = document.getElementById("imageFile");
    var preview = document.getElementById("preview");

    imageImput.addEventListener('change', function () {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                preview.src = e.target.result;
            };
            reader.readAsDataURL(file);
        } else {
            preview.src = "#";
        }
    });
});
  