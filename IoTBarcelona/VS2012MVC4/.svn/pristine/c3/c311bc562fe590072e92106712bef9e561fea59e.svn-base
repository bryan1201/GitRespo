$(document).ready(function () {
    $("#browser").treeview();

    $('#tabs').tabs();

    $("#FromDate").datepicker({
        dateFormat: "yy/mm/dd",
        regional: "zh-TW",
        showAnim: "drop"
    });
    $("#EndDate").datepicker({
        dateFormat: "yy/mm/dd",
        regional: "zh-TW",
        showAnim: "drop"
    });

    // Hide all the submenu ul/li s
    $(".cerlmenu ul").show();

    $(".cerlmenu li ul").hide();

    // Hook up mouse over events
    $(".cerlmenu li").hover(
        function () {
            var sibling = $(this).find("a").next();
            sibling.show();
        },
        function () {
            var sibling = $(this).find("a").next();
            sibling.hide();
        });

    $(".jtable th").each(function () {

        $(this).addClass("ui-state-default");

    });
    $(".jtable td").each(function () {

        $(this).addClass("ui-widget-content");

    });
    $(".jtable tr").hover(
        function () {
            $(this).children("td").addClass("ui-state-hover");
        },
        function () {
            $(this).children("td").removeClass("ui-state-hover");
        }
        );
    $(".jtable tr").click(function () {

        $(this).children("td").toggleClass("ui-state-highlight");
    });


});