$(function () {
    $.ajaxSetup({ cache: false });

    var updateViewState = function () {
        var currentStateVal = $("#currentState").val();
        if (currentStateVal != 'PleaseWait')
            $("#currentQuestion").attr("disabled", "disabled");
        else
            $("#currentQuestion").removeAttr("disabled");
    };

    updateViewState();

    var conn = $.hubConnection();
    var contextHub = conn.createHubProxy("Context");
    $(document).on("change", "#currentState", function () {
        contextHub.invoke("UpdateCurrentState", $(this).val());
        updateViewState();
    });

    $(document).on("change","#currentQuestion", function () {
        $.post(
            "/Admin/CurrentQuestion",
            { "questionID": $(this).val() },
            function () {
                $(".question-body").load("/Admin/QuestionBody");
            });
    });
    var goPrev = function (dropdown) {
        if ($(dropdown).find("option:first").is(":selected")) {
            $(dropdown).find("option:last").prop("selected", true).change();
            return true;
        } 
        $(dropdown).find(":selected").prev().prop("selected", true).change();
        return false;
    }
    var goNext = function (dropdown) {
        if ($(dropdown).find("option:last").is(":selected")) {
            $(dropdown).find("option:first").prop("selected", true).change();
            return true;
        }
        $(dropdown).find(":selected").next().prop("selected", true).change();
        return false;
    }
    $(document.getElementById("move-prev")).click(function () {
        if (goPrev(document.getElementById("currentState"))) {
            goPrev(document.getElementById("currentQuestion"));
        }
        updateViewState();
    });
    $(document.getElementById("move-next")).click(function () {
        if (goNext(document.getElementById("currentState"))) {
            if (goNext(document.getElementById("currentQuestion"))) {
                alert("NO MORE QUESTIONS");
            }
        }
        updateViewState();
    });
    conn.start();
});