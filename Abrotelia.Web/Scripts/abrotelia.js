(function ($) {
    var authorId, galleryItemId, pageId, newsSubscriptionId, textEditField, textImage, textMessage,

    saveAuthor = function (e) {
        textEditField.cleanHtml();
        $("[name='authorDescription']").val(htmlEncode(textEditField.html()));
        var description = htmlEncode(textEditField.html());
        $.post("/Admin/AuthorHandler.ashx?mode=save", {
            id: authorId,
            fullName: $("[name='pageHeader']").val(),
            description: description,
            __RequestVerificationToken: document.querySelector("input[name=__RequestVerificationToken]").getAttribute("value")
        })
        .success(function (data) {
            if ("" == authorId) {
                location.href = data;
                return;
            }
            showMessage(true, "Avtor je bil uspešno shranjen");
        })
        .fail(function (data) {
            if (data.status === 409) {
                showMessage(false, "Avtor že obstaja");
            } else {
                showMessage(false, "Prišlo je do napake. Strežnik je javil: " + data.status + " " + data.statusText);
            }
        });
    },

    saveGalleryItem = function (e) {
        textEditField.cleanHtml();
        $("[name='galleryItemDescription']").val(htmlEncode(textEditField.html()));
        var description = htmlEncode(textEditField.html());
        var data = new FormData(document.getElementById("frmGalleryItemEdit"));       
        data.append("id", galleryItemId);
        data.append("description", description);
        $.ajax({
            url: "/Admin/GalleryItemHandler.ashx?mode=save",
            type: 'POST',
            processData: false,
            contentType: false,
            data: data
        })
        .success(function (data) {
            if ("" == galleryItemId) {
                location.href = data;
                return;
            }
            showMessage(true, "Likovno delo je bil uspešno shranjeno");
            var picture = document.getElementById("picture");
            picture.src = picture.src.split("?")[0] + "?id=" + data + "&size=normal";
        })
        .fail(function (data) {
            if (data.status === 409) {
                showMessage(false, "Likovno delo že obstaja");
            } else {
                showMessage(false, "Prišlo je do napake. Strežnik je javil: " + data.status + " " + data.statusText);
            }
        });
    },

    savePage = function (e) {
        textEditField.cleanHtml();
        $("[name='pageContent']").val(htmlEncode(textEditField.html()));
        var content = htmlEncode(textEditField.html());
        $.post("/Admin/PageHandler.ashx?mode=save", {
            id: pageId,
            title: $("[name='pageHeader']").val(),
            content: content,
            footerCategory: $("[name='footerCategory']").val(),
            headerCategory: $("[name='headerCategory']").val(),
            __RequestVerificationToken: document.querySelector("input[name=__RequestVerificationToken]").getAttribute("value")
        })
        .success(function (data) {
            if ("" == pageId) {
                location.href = data;
                return;
            }
            showMessage(true, "Stran je bila uspešno shranjena");
        })
        .fail(function (data) {
            if (data.status === 409) {
                showMessage(false, "Naslov je že v uporabi");
            } else {
                showMessage(false, "Prišlo je do napake. Strežnik je javil: " + data.status + " " + data.statusText);
            }
        });
    },

    deleteCommand = function (itemId, handler) {
        $.post("/Admin/" + handler + ".ashx?mode=delete", {
            id: itemId,
            __RequestVerificationToken: document.querySelector("input[name=__RequestVerificationToken]").getAttribute("value")
        })
        .success(function (data) {
            location.href = data;
            return;
        })
        .fail(function (data) {
            showMessage(false, "Prišlo je do napake. Strežnik je javil: " + data.status + " " + data.statusText);
        });
    }

    deleteAuthor = function (e) {
        if (confirm('Ste prepričani, da želite izbrisati tega avtorja?')) {
            deleteCommand($(e.target).parents('[itemprop~="author"]').attr('data-id'), 'AuthorHandler');
        }
    },

    deleteGalleryItem = function (e) {
        if (confirm('Ste prepričani, da želite izbrisati to likovno delo?')) {
            deleteCommand($(e.target).parents('[itemprop~="about"]').attr('data-id'), 'GalleryItemHandler');
        }
    },

    deleteNewsSubscription = function (e) {
        if (confirm('Ste prepričani, da želite izbrisati prijavo na e-novice?')) {
            deleteCommand($(e.target).parents('[itemprop~="email"]').attr('data-id'), 'NewsSubscriptionHandler');
        }
    },

    deletePage = function (e) {
        if (confirm('Ste prepričani, da želite izbrisati to stran?')) {
            deleteCommand($(e.target).parents('[itemprop~="blogPost"]').attr('data-id'), 'PageHandler');
        }
    },

    htmlEncode = function (value) {
        return $("<div />").text(value).html();
    },

    showMessage = function (success, message) {
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

    toggleFeaturedItem = function (e) {
        var item = e.target;
        _toggleFeaturedItemClass(item);     
        $.post("/Admin/GalleryItemHandler.ashx?mode=toggleFeatured", {
            id: $(e.target).parents('[itemprop~="about"]').attr('data-id'),
            __RequestVerificationToken: document.querySelector("input[name=__RequestVerificationToken]").getAttribute("value")
        })
        .success(function (data) {
            item.title = data;
        })
        .fail(function (data) {
            _toggleFeaturedItemClass(item);
            showMessage(false, "Prišlo je do napake. Strežnik je javil: " + data.status + " " + data.statusText);
        });
    },

    _toggleFeaturedItemClass = function (o) {
        if (-1 == o.className.indexOf('glyphicon-star-empty')) {
            o.className = o.className.replace('glyphicon-star', 'glyphicon-star-empty');
        }
        else {
            o.className = o.className.replace('glyphicon-star-empty', 'glyphicon-star');
        }
    }

    textEditField = $("#editor"),
    textImage = $(".btn-tooltip #txtImage"),
    txtMessage = $("div.alert"),
    pageId = $("[itemprop~='blogPost']").attr("data-id"),
    authorId = $("[itemprop~='author']").attr("data-id"),
    galleryItemId = $("[itemprop~='about']").attr("data-id");
    newsSubscriptionId = $("[itemprop~='email']").attr("data-id");

    $(document).ready(function () {
        textEditField.wysiwyg({ hotKeys: {}, activeToolbarClass: "active" });
        textEditField.focus();
    });

    $('.uploadimage').click(function (e) {
        e.preventDefault();
        $('#txtImage').click();
    });

    $('#frmPageEdit').submit(function (e) {
        e.preventDefault();
        savePage();
        return false;
    });

    $('#frmAuthorEdit').submit(function (e) {
        e.preventDefault();
        saveAuthor();
        return false;
    });

    $('#frmGalleryItemEdit').submit(function (e) {
        e.preventDefault();
        saveGalleryItem();
        return false;
    });

    $('.action-delete-author').click(function (e) {
        e.preventDefault();
        deleteAuthor(e);
    });

    $('.action-delete-galleryitem').click(function (e) {
        e.preventDefault();
        deleteGalleryItem(e);
    });

    $('.action-delete-newssubscription').click(function (e) {
        e.preventDefault();
        deleteNewsSubscription(e);
    });

    $('.action-delete-page').click(function (e) {
        e.preventDefault();
        deletePage(e);
    });

    $('.featured-item').click(function (e) {
        e.preventDefault();
        toggleFeaturedItem(e);
    });

})(jQuery);