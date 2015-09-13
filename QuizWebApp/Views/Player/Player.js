$(function () {
    $.ajaxSetup({ cache: false });

    var conn = $.hubConnection();
    var contextHub = conn.createHubProxy("Context");

    $(document).on("change", "#questionoptions input[type=radio]", function () {
        var answerIndex = $(this).val();
        $.post("/Player/PlayerSelectedOptionIndex", { "answerIndex": answerIndex },
        function () {
            contextHub.invoke("PlayerSelectedOptionIndex");
        });
    });

    contextHub.on("CurrentStateChanged", function (newState) {
        $(".main-content").load("/Player/PlayerMainContent");
    });
    conn.start();
});