// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$('body').on('click', '.btnfeednews', function () {

    var $this = $(this);    
    var $form = $this.closest("form");
    var redirectUrl = $this.data('redirecturl');
    //var url = "https://localhost:44388/Api/RssFeed";
    var rssFeedUrl = $form.find("#rssFeedUrl").val()
    $.ajax({
        type: 'POST',
        cache: false,
        url: $form.attr("action"),
        data: JSON.stringify({
            rssFeedUrl : rssFeedUrl
        }),
        contentType: 'application/json',
        success: function (data) {
            $.ajax({
                type: 'Get',
                cache: false,
                url: redirectUrl,
                data: JSON.stringify({
                    rssFeedUrl: rssFeedUrl
                }),
                contentType: 'application/json',
                success: function (data) {
                    $("#news-items").html(data);
                }
            });
        }
    });
    return false;
});
$('body').on('change', '.ddlSortBy', function () {
    var $this = $(this);
    var $form = $this.closest("form");
    $.ajax({
        type: 'POST',
        cache: false,
        url: $form.attr("action"),
        //data: JSON.stringify({
        //    Name: $form.find("#Name").val(),
        //    HasAutoService: $form.find("#HasAutoService").val(),
        //    UserId: $form.find("#UserId").val(),
        //    Random: Math.random()
        //}),
        contentType: 'application/json',
        success: function (data) {
            //alert(data);
            $("#news-items").html(data);
        }
    });
    return false;
});
