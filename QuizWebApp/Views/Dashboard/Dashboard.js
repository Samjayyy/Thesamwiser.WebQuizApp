var setupDashBoard = function (view, callback) {
    $(function () {
        $.ajaxSetup({ cache: false });
        var updateView = function () {
            $(".main-content").load(view, callback);
        };

        var conn = $.hubConnection();
        var contextHub = conn.createHubProxy("Context");
        contextHub.on("CurrentStateChanged", updateView);
        contextHub.on("PlayerSelectedOptionIndex", updateView);
        conn.start();

        callback();
    });
};

var fixTable = function (event) {
    var tableEl = document.getElementById("dashboard-table");
    if (!tableEl) return;
    var $table = $(tableEl);
    var $fixedColumn = $table.clone().insertBefore($table).addClass('fixed-column');

    $fixedColumn.find('th:not(.col-fix),td:not(.col-fix)').remove();

    $fixedColumn.find('tr').each(function (i, elem) {
        $(this).height($table.find('tr:eq(' + i + ')').height());
    });
};