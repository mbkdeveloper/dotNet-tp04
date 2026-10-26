// Augmenter la quantité d'un produit du panier
$(document).on("click", ".PlusProducts", function (event) {
    event.preventDefault();
    var recordtoupdate = $(this).attr("data-id");
    if (recordtoupdate !== '') {
        $.post("/Panier/PlusProduct", { "id": recordtoupdate }, function (data) {
            if (data.ct == "1") {
                $('#totalapayer').text(data.total);
                $("#quantite_" + recordtoupdate).text(data.quatite);
                $("#rquantite_" + recordtoupdate).text(data.quatite);
                $("#total_" + recordtoupdate).text(data.totalRow);
            }
            // Redirection après la mise à jour
            window.location.href = "/Panier";
        });
    }
});

// Diminuer la quantité d'un produit du panier
$(document).on("click", ".MinProducts", function (event) {
    event.preventDefault();
    var recordtoupdate = $(this).attr("data-id");
    if (recordtoupdate !== '') {
        $.post("/Panier/MinusProduct", { "id": recordtoupdate }, function (data) {
            if (data.ct == "1") {
                $('#totalapayer').text(data.total);
                $("#quantite_" + recordtoupdate).text(data.quatite);
                $("#rquantite_" + recordtoupdate).text(data.quatite);
                $("#total_" + recordtoupdate).text(data.totalRow);
            }
            // Redirection après la mise à jour (le produit a pu être retiré du panier)
            window.location.href = "/Panier";
        });
    }
});

// Supprimer un produit du panier
$(document).on("click", ".RemoveLink", function (event) {
    event.preventDefault();
    // Get the id from the link
    var recordtoupdate = $(this).attr("data-id");
    if (recordtoupdate != '') {
        // Perform the ajax post
        $.post("/Panier/RemoveProduct", { "id": recordtoupdate },
            function (data) {
                $("#row-" + recordtoupdate).fadeOut('slow');
                $('#totalapayer').text(data.total);
                window.location.href = "/Panier";
            });
    }
});
