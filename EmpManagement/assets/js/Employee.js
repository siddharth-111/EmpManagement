$(document).on("click", ".check", function () {
    var empId = $(this).data('id');
    $(".modal-header #empId").val(empId);
});
$(".header").click(function (evt) {
    var sortfield = $(evt.target).data("sortfield");
    if ($("#SortField").val() == sortfield) {
        if ($("#SortDirection").val() == "ascending") {
            $("#SortDirection").val("descending");
        }
        else {
            $("#SortDirection").val("ascending");
        }
    }
    else {
        $("#SortField").val(sortfield);
        $("#SortDirection").val("ascending");
    }
    evt.preventDefault();
    $("form").submit();
});
$(".abc").click(function (evt) {
    var pageindex = $(evt.target).data("pageindex");
    $("#CurrentPageIndex").val(pageindex);
    evt.preventDefault();
    $('.clear-inp').val('');
    $("form").submit();
});    