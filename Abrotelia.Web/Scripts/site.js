(function ($) {

    var
        initGallerySearch = function (search) {
            $('input', $(search)).typeahead({
                minLength: 1,
                items: 10,
                showHintOnFocus: true,
                source: function (query, process) {
                    if (1 > query.length) return;
                    return $.get('/gallerySearch/', { type: 'typeahead', query: query }, function (data) {
                        return process(data.Items);
                    });
                },

            });
        },

        subscribeToNews = function (email, result) {
            $.post("/Action/NewsSubscriptionHandler.ashx?mode=save", {
                email: $(email).val(),
                __RequestVerificationToken: document.querySelector("input[name=__RequestVerificationToken]").getAttribute("value")
            })
            .success(function (data) {
                showMessage(result, true, "Prijava je bila uspešna");
            })
            .fail(function (data) {
                if ("409" == data.status) {
                    showMessage(result, true, "Prijava je bila uspešna");
                }
                else {
                    showMessage(result, false, "Prišlo je do napake. Strežnik je javil: " + data.status + " " + data.statusText);
                }
            });
        },

        showMessage = function (txtMessage, success, message) {
            var className = success ? "alert-success" : "alert-danger";
            txtMessage.removeClass("alert-success");
            txtMessage.removeClass("alert-danger");
            txtMessage.addClass(className);
            txtMessage.text(message);
            txtMessage.fadeIn();
            setTimeout(function () {
                txtMessage.fadeOut("slow", function () {
                    txtMessage.removeClass(className);
                });
            }, 4000);
        },

        displayGoogleMap = function () {
            var maxHeight = Math.min(450, document.body.clientHeight);
            var maxWidth = Math.min(600, document.body.clientWidth - 30);
            document.documentElement.innerHTML =
                document.documentElement.innerHTML.replace('[@@ZEMLJEVID@@]',
                '<iframe width="' + maxWidth + '" height="' + maxHeight + '" frameborder="0" style="border:0;" ' +
                'src="https://www.google.com/maps/embed/v1/place?q=koprska%20ulica%2094&key=AIzaSyD9iV1cZ2r3GE7RZf0b5TuQNO-_pJBsohA" allowfullscreen></iframe>');
        };

    $(document).ready(function () {
        var gallerySearch = document.getElementById("gallerySearch");
        if (gallerySearch) {
            initGallerySearch(gallerySearch);
        }
        var newsSubscription = document.getElementById("subscriptionForm");
        if (newsSubscription) {
            $(newsSubscription).submit(function (e) {
                e.preventDefault();
                subscribeToNews(document.getElementById("newsEmail"), $(".anart-news-subscription .alert"));
            });
        }
        var googleMapPosition = document.documentElement.innerHTML.indexOf('[@@ZEMLJEVID@@]');
        if (googleMapPosition != -1) {
            displayGoogleMap();
        }
    });

})(jQuery);