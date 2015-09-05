$(function () {
    $.ajaxSetup({ cache: false });

    var conn = $.hubConnection();
    var contextHub = conn.createHubProxy("Context");

    $(document).on("change",".options input[type=radio]",function () {
        contextHub.invoke("PlayerSelectedOptionIndex", $(this).val());
    });

    contextHub.on("CurrentStateChanged", function (newState) {
        $(".main-content").load("/Player/PlayerMainContent");
    });
    conn.start();
});