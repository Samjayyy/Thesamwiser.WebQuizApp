
$(function () {
    $.ajaxSetup({ cache: false });

    var conn = $.hubConnection();
    var contextHub = conn.createHubProxy("Context");

    var isChecked = function (radio) {
        return $("input[name='" + radio + "']:checked").val();
    };
    var updateAnswer = function () {
        var answerIndex = isChecked("answerOptions");
        if (!answerIndex) {
            return;
        }
        var assignedValue = isChecked("answerscores");
        if (!assignedValue) {
            $("#answerscores input:radio[name=answerscores]:not(disbled):first").attr('checked', true); // check first element
            assignedValue = isChecked("answerscores");
        }
        var answerId = $(document.getElementById("CurrentAnswerId")).val();
        $.post("/Player/PlayerSelectedOptionIndex",
            {
                "answerId": answerId
                , "answerIndex": answerIndex
                , "assignedValue": assignedValue
            },
            function () {
                contextHub.invoke("PlayerSelectedOptionIndex");
            });
    };
    $(document).on("change", "#questionoptions input[type=radio]", updateAnswer);
    $(document).on("change", "#answerscores input[type=radio]", updateAnswer);

    contextHub.on("CurrentStateChanged", function (newState) {
        $(".main-content").load("/Player/PlayerMainContent");
    });
    conn.start();
});