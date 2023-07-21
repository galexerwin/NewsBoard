$("#profile_header_img").on("click", function () {
    var display = $("#profile_box").css("display");
    if (display == "none")
        $("#profile_box").show();
    else
        $("#profile_box").hide();
});

$("profile_box_ep").on("click", function () {
    $("#profile_nav").hide();
    window.location = "/editprofile";
});

$("profile_box_so").on("click", function () {
    $("#profile_nav").hide();
    window.location = "/signout";
});