$(function () {
    $('#signout').click(function () {
        if (confirm('Do you want to sign out?')) {
            $.post($(this).attr('href'))
                .done(function (data) {
                    location.href = data.url;
                });
        }
        return false;
    });

})