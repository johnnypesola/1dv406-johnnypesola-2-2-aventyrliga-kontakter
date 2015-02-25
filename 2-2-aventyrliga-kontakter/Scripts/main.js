
window.onload = function () {

    // Add closebutton to messageboxes
    $(".success-message, .error-message").each(function (element) {
        $(this).append('<a class="close" href="#">&#215;</a>').click(function () {
            $(this).fadeOut();
        });
    })
}

function ConfirmDelete(deleteButton) {

    // Get contact name
    var fname = $(deleteButton).parent().parent().find(".fname").text();
    var lname = $(deleteButton).parent().parent().find(".lname").text();

    if(confirm("Vill du verkligen ta bort kontakten " + fname + " " + lname + " ?"))
    {
        return true;
    }
    return false;
}


