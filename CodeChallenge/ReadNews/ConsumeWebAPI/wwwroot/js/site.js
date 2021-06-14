 //Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
 //for details on configuring this project to bundle and minify static web assets.

 //Write your JavaScript code.


$('body').on('click', '.btnfeednews', function () {

    var $this = $(this);    
    var $form = $this.closest("form");
    var redirectUrl = $this.data('redirecturl');
    var rssFeedUrl = $form.find("#rssFeedUrl").val()
    $.post($form.attr("action"), {
        rssFeedUrl: rssFeedUrl,
    },
        function (returnedData) {
            var ddlSortByVal = $('.ddlSortBy').val();
            var ddlDescVal = $('.ddlDesc').val();
            $.post(redirectUrl, {
                SortBy: ddlSortByVal,
                Desc: ddlDescVal
            },
                function (returnedData) {
                    $("#news-items").html(returnedData);
                });
        });     
    return false;
});
$('body').on('change', '.ddlSortBy, .ddlDesc', function () {
    var $this = $(this);
    var $form = $this.closest("form");
    var ddlSortByVal = $('.ddlSortBy').val();
    var ddlDescVal = $('.ddlDesc').val();
    $.post($form.attr("action"), {
        SortBy: ddlSortByVal,
        Desc: ddlDescVal,
        Random: Math.random()
    },
        function (returnedData) {
            $("#news-items").html(returnedData);
        });     
    return false;
});
