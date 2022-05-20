// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    bindModalDataToggle();
});


function bindModalDataToggle() {
    var modalPlaceholderElement = $('#ajax-modal-placeholder');
    var waitModal = $("#processing-dialog").find('.modal');
    var loaded = false;
    waitModal.on("shown.bs.modal", function (e) {
        if (loaded) {
            waitModal.modal('hide');
            loaded = false;
        }
    });
    $('body').on('click', 'button[data-toggle="ajax-modal"],a[data-toggle="ajax-modal"]', function (event) {
        if (event) {
            event.preventDefault();
        }
        var button = $(this);
        var url = button.data('url');
        waitModal.modal('show');
        $.get(url).done(function (data) {
            waitModal.modal('hide');
            loaded = true;
            var newElement = $("<div></div>");
            newElement.append($(data));
            $('body').append(newElement);
            var modal = newElement.find('.modal');
            modal.on("hidden.bs.modal", function (e) {
                var callback = button.data('callback');
                if (callback) {
                    var str = callback + "(" + JSON.stringify(modal.data("result")) + ");";
                    eval(str);
                }
                setTimeout(function () {
                    newElement.detach();
                }, 1000);
            });
            modal.modal('show');
        });
    });

}
