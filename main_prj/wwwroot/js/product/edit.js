//Load images preview
$(document).ready(function () {
    var imageImput = document.getElementById("PreviewImage");
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

    var detailImages = document.getElementById("DetailImages");
    var detailPreviews = document.getElementById("detailPreviews");

    detailImages.addEventListener('change', function () {
        detailPreviews.innerHTML = '';
        const files = this.files;
        for (let i = 0; i < files.length; i++) {
            const file = this.files[i];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    const img = document.createElement("img");
                    img.width = img.height = 100;
                    img.classList.add("detail-preview");
                    img.src = e.target.result;
                    detailPreviews.appendChild(img);
                };
                reader.readAsDataURL(file);
            }
        }
    });
})