var sortField, sortDirection, currentPage, pageSize;
pageSize = 25;
function getPage(currPage, pSize) {
    pagingInfo = {};
    currentPage = currPage;
    pageSize = pSize;
    pagingInfo.pageSize = pageSize;
    pagingInfo.currPage = currentPage;
    pagingInfo.sortField = sortField;
    pagingInfo.sortDirection = sortDirection;
    pagingInfo.searchString = $("#search").val();
    $.ajax({
        url: "/Employee/Retrieve",
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(pagingInfo),
        async: true,
        processData: false,
        success: function (data) {
            var trHTML = '';
            var pages = '';
            $.each(data, function (key, employee) {
                if (!employee.Pagecount) {
                    trHTML += '<tr><td>' + '<span>' + safe_tags_replace(employee.EmployeeName) + '<span>' + '</td><td>' + safe_tags_replace(employee.Email) + '</td><td>' + safe_tags_replace(employee.Address) + '</td><td>' + employee.Dept + '</td><td>' + ((new Date(parseInt(employee.DOJ.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOJ.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOJ.substr(6))).getFullYear()) + '</td><td>' + ((new Date(parseInt(employee.DOB.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOB.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOB.substr(6))).getFullYear()) + '</td><td style="text-align: right;">' + employee.Contact + '</td><td style="text-align: right;">' + employee.Salary +
                '</td><td style="text-align: center;">' +
                  '<div class="dropdown"><a href = "" data-toggle="dropdown" class="dropdown-toggle"><span class="glyphicon glyphicon-cog"></span></a><ul class="dropdown-menu"><li><a href="/Employee/Edit/' + employee.EmployeeID + '">Update</a></li><li><a data-toggle="modal" href="" id = "openModal" class = "check" data-target="#myModal" data-id="' + employee.EmployeeID + '"> Delete</a></li> </ul>' + '</div>' + '</td>' + '</tr>';
                }
                else {
                    var pageCount = employee.Pagecount;
                    if ((pagingInfo.pageSize * (pagingInfo.currPage + 1)) > employee.TotalRecords)
                        pages += '<span style = "display: inline-block;padding-top:2px;">Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + employee.TotalRecords + ' out of ' + employee.TotalRecords + '</span>';
                    else
                        pages += '<span style = "display: inline-block;padding-top:2px;">Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + (pagingInfo.pageSize * (pagingInfo.currPage + 1)) + ' out of ' + employee.TotalRecords + '</span>';
                    pages += '<ul class = "pagination pull-right" style = "margin: -5px 0 40px;">';
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i - 1) > 0) {
                                pages += '<li><a href = "#" onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">&laquo;</a></li>'
                            }
                        }
                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (currentPage + 1)) {
                            if (i == 1) {
                                if (pageCount > 4) {
                                    pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"   onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                                    pages += '<li><a href = "#"  onclick ="getPage(' + (i) + ',' + pagingInfo.pageSize + ')">' + (i + 1) + '</a></li>'
                                    pages += '<li><a >...</a></li>'
                                    pages += '<li><a href = "#"  onclick ="getPage(' + (pageCount - 2) + ',' + pagingInfo.pageSize + ')">' + (pageCount - 1) + '</a></li>'
                                    pages += '<li><a href = "#"  onclick ="getPage(' + (pageCount - 1) + ',' + pagingInfo.pageSize + ')">' + (pageCount) + '</a></li>'
                                    break;
                                }
                                else {

                                    pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"  onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                                }

                            }
                            else
                                if (i == (pageCount - 1) && (pageCount > 5)) {
                                    pages += '<li><a href = "#"  onclick ="getPage(' + (pageCount - 2) + ',' + pagingInfo.pageSize + ')">' + (2) + '</a></li>'
                                    pages += '<li><a href = "#"  onclick ="getPage(' + (0) + ',' + pagingInfo.pageSize + ')">' + (1) + '</a></li>'
                                    pages += '<li><a >...</a></li>'
                                    pages += '<li><a href = "#"  onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">' + (i - 1) + '</a></li>'
                                    pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"   onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'

                                }
                                else
                                    pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"  onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                            }
                            else {
                                if (pageCount - i > 4) {
                                    while (i <= pageCount) {
                                        if (i == (currentPage + 1)) {


                                            if (i == (pageCount)) {

                                                pages += '<li><a href = "#"  onclick ="getPage(' + (0) + ',' + pagingInfo.pageSize + ')">' + (1) + '</a></li>'
                                                pages += '<li><a href = "#"  onclick ="getPage(' + (1) + ',' + pagingInfo.pageSize + ')">' + (2) + '</a></li>'
                                                pages += '<li><a >...</a></li>'
                                                pages += '<li><a href = "#"  onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">' + (i - 1) + '</a></li>'
                                                pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"   onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'

                                            } else {
                                                if ((i - 2) >= 1) {
                                                    pages += '<li><a href = "#"  onclick ="getPage(' + (0) + ',' + pagingInfo.pageSize + ')">' + (1) + '</a></li>'
                                                    pages += '<li><a >...</a></li>'
                                                }
                                                pages += '<li><a href = "#"  onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">' + (i - 1) + '</a></li>'
                                                pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"  onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                                                pages += '<li><a href = "#"  onclick ="getPage(' + (i) + ',' + pagingInfo.pageSize + ')">' + (i + 1) + '</a></li>'
                                                if (i != (pageCount - 1)) {
                                                    pages += '<li><a >...</a></li>'
                                                    pages += '<li><a href = "#"  onclick ="getPage(' + (pageCount - 1) + ',' + pagingInfo.pageSize + ')">' + (pageCount) + '</a></li>'
                                                }

                                                i += pageCount - 3;
                                            }
                                        }
                                        i++;
                                    }
                                }
                                else {

                                    pages += '<li><a href = "#"  onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'

                                }
                            }
                        }
                        for (var i = 1; i <= pageCount; i++) {
                            if (i == (pagingInfo.currPage + 1)) {
                                if ((i) < pageCount) {
                                    pages += '<li><a href = "#"   onclick ="getPage(' + (i) + ',' + pagingInfo.pageSize + ')">&raquo;</a></li>'
                                }
                            }
                        }
                        pages += '</ul>';
                    }
                });

                $("#employeeTable").find("tr:gt(0)").remove();
                $('#employeeTable').append(trHTML);
                var table = $('#employeeTable');
                table.find('.sorter').remove();
                $('#paginationDiv').empty();
                $('#paginationDiv').append(pages);
                $("#search").trigger("keyup");
                var inc = 0;
                $(".clicked").each(function () {
                    if ($(this).data("sortfield") == sortField) {
                        if (sortDirection == "ascending") {
                            $(this).append($('<span class="pull-right sorter"><i class="glyphicon glyphicon-chevron-up"></i></span>'));
                        } else {
                            $(this).append($('<span class="pull-right sorter"><i class="glyphicon glyphicon-chevron-down"></i></span>'));
                        }
                        return false;
                    }
                    inc++;
                });

            },
            error: function (xhr) {
                alert('error');
            }
        });
}
var tagsToReplace = {
    '&': '&amp;',
    '<': '&lt;',
    '>': '&gt;'
};

function replaceTag(tag) {
    return tagsToReplace[tag] || tag;
}

function safe_tags_replace(str) {
    return str.replace(/[&<>]/g, replaceTag);
}

function initialLoad() {
    $(".prompt-reload").delay(2000).fadeOut(300);
    $.ajax({
        url: "/Employee/Retrieve",
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(pagingInfo),
        success: function (data) {
            var trHTML = '';
            var pages = '';

            var returnedData = data.dataObject;
            $.each(data, function (key, employee) {
                if (!employee.Pagecount) {
                    trHTML += '<tr><td>' + '<span>' + safe_tags_replace(employee.EmployeeName) + '</span>' + '</td><td>' + safe_tags_replace(employee.Email) + '</td><td>' + safe_tags_replace(employee.Address) + '</td><td>' + safe_tags_replace(employee.Dept) + '</td><td>' + ((new Date(parseInt(employee.DOJ.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOJ.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOJ.substr(6))).getFullYear()) + '</td><td>' + ((new Date(parseInt(employee.DOB.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOB.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOB.substr(6))).getFullYear()) + '</td><td style="text-align: right;">' + employee.Contact + '</td><td style="text-align: right;">' + employee.Salary +
                '</td><td style="text-align: center;">' +
                  '<div class="dropdown"><a  href = "" data-toggle="dropdown" class="dropdown-toggle"><span class="glyphicon glyphicon-cog"></span></a><ul class="dropdown-menu"><li><a href="/Employee/Edit/' + employee.EmployeeID + '">Update</a></li><li><a data-toggle="modal" href="" id = "openModal" class = "check" data-target="#myModal" data-id="' + employee.EmployeeID + '"> Delete</a></li> </ul>' + '</div>' + '</td>' + '</tr>';
                }
                else {
                    var pageCount = employee.Pagecount;
                    if ((pagingInfo.pageSize * (pagingInfo.currPage + 1)) > employee.TotalRecords)
                        pages += '<span style = "display: inline-block;padding-top:2px;">Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + employee.TotalRecords + ' out of ' + employee.TotalRecords + '</span>';
                    else
                        pages += '<span style = "display: inline-block;padding-top:2px;">Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + (pagingInfo.pageSize * (pagingInfo.currPage + 1)) + ' out of ' + employee.TotalRecords + '</span>';
                    pages += '<ul class = "pagination pull-right" style = "margin: -5px 0 40px;">';

                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i - 1) > 0) {
                                pages += '<li><a href = "#" onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">&laquo;</a></li>'
                            }
                        }
                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"   onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                        } else {
                            if (pageCount - i >= 4) {
                                pages += '<li><a href = "#"  onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                                pages += '<li><a >...</a></li>'
                                i += pageCount - 3;
                            }

                            pages += '<li><a href = "#"  onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                        }

                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i) < pageCount) {
                                pages += '<li><a href = "#"  onclick ="getPage(' + (i) + ',' + pagingInfo.pageSize + ')">&raquo;</a></li>'
                            }
                        }
                    }

                    pages += '</ul>';
                }

            });

            $("#employeeTable").find("tr:gt(0)").remove();
            $('#employeeTable').append(trHTML);
            var table = $('#employeeTable');
            table.find('.sorter').remove();
            $('#paginationDiv').empty();
            $('#paginationDiv').append(pages);
            $("#search").trigger("keyup");
            var inc = 0;
            $(".clicked").each(function () {
                if ($(this).data("sortfield") == sortField) {
                    if (sortDirection == "ascending") {
                        $(this).append($('<span class="pull-right sorter"><i class="glyphicon glyphicon-chevron-up"></i></span>'));
                    } else {
                        $(this).append($('<span class="pull-right sorter"><i class="glyphicon glyphicon-chevron-down"></i></span>'));
                    }
                    return false;
                }
                inc++;
            });


//            $('#employeeTable').append(trHTML);
//            var inc = 0;
//            $(".clicked").each(function () {
//                if ($(this).data("sortfield") == sortField) {
//                    if (sortDirection == "ascending") {
//                        $(this).append($('<span class="pull-right sorter"><i class="glyphicon glyphicon-chevron-up"></i></span>'));
//                    } else {
//                        $(this).append($('<span class="pull-right sorter"><i class="glyphicon glyphicon-chevron-down"></i></span>'));
//                    }
//                    return false;
//                }
//                inc++;
//            });
//            $('#paginationDiv').append(pages);
        },
        error: function (xhr) {
            alert('error');
        }
    });

}

function deleteEmployee(id) {
    var obj = {
        id: id
    };
    $.ajax({
        url: "/Employee/Delete",
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(obj),
        success: function (data) {
            if (data == true) {          
                $(".prompt-reload").css("display", "block");              
                initialLoad();
            }
            else {                 
                $(".prompt-reloadFail").css("display", "block");
                initialLoad();
            }
        },
        error: function (xhr) {
            alert('error');
        }
    });
};

$(document).ready(function () {

    $('#search').keydown(function (event) {
        var keyCode = (event.keyCode ? event.keyCode : event.which);
        if (keyCode == 13) {
            $('.getSearchValue').trigger('click');
        }
    });
    $(".prompt-reload").delay(2000).fadeOut(300);
    sortField = "Name";
    sortDirection = "ascending";
    currentPage = 0;
    var inc = 0;
    pagingInfo = {};
    pagingInfo.pageSize = pageSize;
    pagingInfo.currPage = currentPage;
    pagingInfo.sortField = sortField;
    pagingInfo.sortDirection = sortDirection;
    pagingInfo.searchString = $("#search").val();
    initialLoad();
});

$(".getSearchValue").click(function (evt) {
    pagingInfo = {};
    currentPage = 0;

    pagingInfo.pageSize = pageSize;
    pagingInfo.currPage = currentPage;
    pagingInfo.sortField = sortField;
    pagingInfo.sortDirection = sortDirection;
    pagingInfo.searchString = $("#search").val();
    $.ajax({
        url: "/Employee/Retrieve",
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(pagingInfo),
        async: true,
        processData: false,
        success: function (data) {
            var trHTML = '';
            var pages = '';
            var length = data.length;
            var increment = 0;
            $.each(data, function (key, employee) {
                if (!employee.Pagecount) {
                    if (employee.Pagecount == 0 && increment==length-1) {
                          trHTML += '<tr class="removeClass"><td colspan="9"><div class="text-center">No records found</div></td><tr>';
                          return false;
                    }
                      trHTML += '<tr><td>' + '<span>' + safe_tags_replace(employee.EmployeeName) + '<span>' + '</td><td>' + safe_tags_replace(employee.Email) + '</td><td>' + safe_tags_replace(employee.Address) + '</td><td>' + employee.Dept + '</td><td>' + ((new Date(parseInt(employee.DOJ.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOJ.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOJ.substr(6))).getFullYear()) + '</td><td>' + ((new Date(parseInt(employee.DOB.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOB.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOB.substr(6))).getFullYear()) + '</td><td style="text-align: right;">' + employee.Contact + '</td><td style="text-align: right;">' + employee.Salary +
                '</td><td style="text-align: center;">' +
                  '<div class="dropdown"><a href = "" data-toggle="dropdown" class="dropdown-toggle"><span class="glyphicon glyphicon-cog"></span></a><ul class="dropdown-menu"><li><a href="/Employee/Edit/' + employee.EmployeeID + '">Update</a></li><li><a data-toggle="modal" href="" id = "openModal" class = "check" data-target="#myModal" data-id="' + employee.EmployeeID + '"> Delete</a></li> </ul>' + '</div>' + '</td>' + '</tr>';
                }
                else {
                    var pageCount = employee.Pagecount;
                    if ((pagingInfo.pageSize * (pagingInfo.currPage + 1)) > employee.TotalRecords)
                        pages += '<span style = "display: inline-block;padding-top:2px;">Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + employee.TotalRecords + ' out of ' + employee.TotalRecords + '</span>';
                    else
                        pages += '<span style = "display: inline-block;padding-top:2px;">Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + (pagingInfo.pageSize * (pagingInfo.currPage + 1)) + ' out of ' + employee.TotalRecords + '</span>';
                    pages += '<ul class = "pagination pull-right" style = "margin: -5px 0 40px;" >';
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i - 1) > 0) {
                                pages += '<li><a href = "#"  onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">&laquo;</a></li>'
                            }
                        }
                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (currentPage + 1)) {
                            pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"   onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                        }
                        else {
                            if (pageCount - i >= 4) {
                                pages += '<li><a href = "#"  onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                                pages += '<li><a >...</a></li>'
                                i += pageCount - 3;
                            }
                            pages += '<li><a href = "#"   onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                        }
                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i) < pageCount) {
                                pages += '<li><a href = "#"  onclick ="getPage(' + (i) + ',' + pagingInfo.pageSize + ')">&raquo;</a></li>'
                            }
                        }
                    }
                    pages += '</ul>';
                }
                increment++;
            });

            $("#employeeTable").find("tr:gt(0)").remove();
            $('#employeeTable').append(trHTML);
            var table = $('#employeeTable');
            table.find('.sorter').remove();
            $('#paginationDiv').empty();
            $('#paginationDiv').append(pages);
            $("#search").trigger("keyup");
            var inc = 0;
            $(".clicked").each(function () {
                if ($(this).data("sortfield") == sortField) {
                    if (sortDirection == "ascending") {
                        $(this).append($('<span class="pull-right sorter"><i class="glyphicon glyphicon-chevron-up"></i></span>'));
                    } else {
                        $(this).append($('<span class="pull-right sorter"><i class="glyphicon glyphicon-chevron-down"></i></span>'));
                    }
                    return false;
                }
                inc++;
            });

        },
        error: function (xhr) {
            alert('error');
        }
    });

});
$('.clicked').click(function (evt) {

    if ($("#SortDirection").val() == "ascending") {
        $("#SortDirection").val("descending");
    }
    else {
        $("#SortDirection").val("ascending");
    }
    sortDirection = $("#SortDirection").val();
    sortField = $(evt.target).data("sortfield");

    pagingInfo = {};
    pagingInfo.pageSize = pageSize;

    pagingInfo.currPage = currentPage;
    pagingInfo.sortField = sortField;
    pagingInfo.sortDirection = sortDirection;
    pagingInfo.searchString = $("#search").val();
    console.log(pagingInfo);
    $.ajax({
        url: "/Employee/Retrieve",
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(pagingInfo),
        async: true,
        processData: false,
        success: function (data) {
            var trHTML = '';
            var pages = '';

            $.each(data, function (key, employee) {

                if (!employee.Pagecount) {
                    trHTML += '<tr><td>' + safe_tags_replace(employee.EmployeeName) + '</td><td>' + safe_tags_replace(employee.Email) + '</td><td>' + safe_tags_replace(employee.Address) + '</td><td>' + safe_tags_replace(employee.Dept) + '</td><td>' + ((new Date(parseInt(employee.DOJ.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOJ.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOJ.substr(6))).getFullYear()) + '</td><td>' + ((new Date(parseInt(employee.DOB.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOB.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOB.substr(6))).getFullYear()) + '</td><td style="text-align: right;">' + employee.Contact + '</td><td style="text-align: right;">' + employee.Salary +
                '</td><td style="text-align: center;">' +
                  '<div class="dropdown"><a  href = "" data-toggle="dropdown" class="dropdown-toggle"><span class="glyphicon glyphicon-cog"></span></a><ul class="dropdown-menu"><li><a href="/Employee/Edit/' + employee.EmployeeID + '">Update</a></li><li><a data-toggle="modal" href="" id = "openModal" class = "check" data-target="#myModal" data-id="' + employee.EmployeeID + '"> Delete</a></li> </ul>' + '</div>' + '</td>' + '</tr>';
                }
                else {
                    var pageCount = employee.Pagecount;
                    if ((pagingInfo.pageSize * (pagingInfo.currPage + 1)) > employee.TotalRecords)
                        pages += '<span style = "display: inline-block;padding-top:2px;">Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + employee.TotalRecords + ' out of ' + employee.TotalRecords + '</span>';
                    else
                        pages += '<span style = "display: inline-block;padding-top:2px;">Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + (pagingInfo.pageSize * (pagingInfo.currPage + 1)) + ' out of ' + employee.TotalRecords + '</span>';
                    pages += '<ul class = "pagination pull-right" style = "margin: -5px 0 40px;">';

                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i - 1) > 0) {
                                pages += '<li><a href = "#"   onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">&laquo;</a></li>'
                            }
                        }
                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (currentPage + 1)) {
                            if (i == 1) {
                                if (pageCount > 4) {
                                    pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"   onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                                    pages += '<li><a href = "#"  onclick ="getPage(' + (i) + ',' + pagingInfo.pageSize + ')">' + (i + 1) + '</a></li>'
                                    pages += '<li><a >...</a></li>'
                                    pages += '<li><a href = "#"  onclick ="getPage(' + (pageCount - 2) + ',' + pagingInfo.pageSize + ')">' + (pageCount - 1) + '</a></li>'
                                    pages += '<li><a href = "#"  onclick ="getPage(' + (pageCount - 1) + ',' + pagingInfo.pageSize + ')">' + (pageCount) + '</a></li>'
                                    break;
                                }
                                else {

                                    pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"  onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                                }

                            }
                            else
                                if (i == (pageCount - 1) && (pageCount > 5)) {
                                    pages += '<li><a href = "#"  onclick ="getPage(' + (pageCount - 2) + ',' + pagingInfo.pageSize + ')">' + (2) + '</a></li>'
                                    pages += '<li><a href = "#"  onclick ="getPage(' + (0) + ',' + pagingInfo.pageSize + ')">' + (1) + '</a></li>'
                                    pages += '<li><a >...</a></li>'
                                    pages += '<li><a href = "#"  onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">' + (i - 1) + '</a></li>'
                                    pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"   onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'

                                }
                                else
                                    pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"  onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                            }
                            else {
                                if (pageCount - i > 4) {
                                    while (i <= pageCount) {
                                        if (i == (currentPage + 1)) {


                                            if (i == (pageCount)) {

                                                pages += '<li><a href = "#"  onclick ="getPage(' + (0) + ',' + pagingInfo.pageSize + ')">' + (1) + '</a></li>'
                                                pages += '<li><a href = "#"  onclick ="getPage(' + (1) + ',' + pagingInfo.pageSize + ')">' + (2) + '</a></li>'
                                                pages += '<li><a >...</a></li>'
                                                pages += '<li><a href = "#"  onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">' + (i - 1) + '</a></li>'
                                                pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"   onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'

                                            } else {
                                                if ((i - 2) >= 1) {
                                                    pages += '<li><a href = "#"  onclick ="getPage(' + (0) + ',' + pagingInfo.pageSize + ')">' + (1) + '</a></li>'
                                                    pages += '<li><a >...</a></li>'
                                                }
                                                pages += '<li><a href = "#"  onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">' + (i - 1) + '</a></li>'
                                                pages += '<li class="active"><a href = "#" style = "z-index: 0;margin-left: 0px;"  onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                                                pages += '<li><a href = "#"  onclick ="getPage(' + (i) + ',' + pagingInfo.pageSize + ')">' + (i + 1) + '</a></li>'
                                                if (i != (pageCount - 1)) {
                                                    pages += '<li><a >...</a></li>'
                                                    pages += '<li><a href = "#"  onclick ="getPage(' + (pageCount - 1) + ',' + pagingInfo.pageSize + ')">' + (pageCount) + '</a></li>'
                                                }

                                                i += pageCount - 3;
                                            }
                                        }
                                        i++;
                                    }
                                }
                                else {

                                    pages += '<li><a href = "#"  onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'

                                }
                            }
                        }
                        for (var i = 1; i <= pageCount; i++) {
                            if (i == (pagingInfo.currPage + 1)) {
                                if ((i) < pageCount) {
                                    pages += '<li><a href = "#"   onclick ="getPage(' + (i) + ',' + pagingInfo.pageSize + ')">&raquo;</a></li>'
                                }
                            }
                        }

                        pages += '</ul>';
                    }

                });

                $("#employeeTable").find("tr:gt(0)").remove();
                $('#employeeTable').append(trHTML);
                var table = $('#employeeTable');
                table.find('.sorter').remove();
                $('#paginationDiv').empty();
                $('#paginationDiv').append(pages);


                var inc = 0;
                sortDirection = $("#SortDirection").val();
                $(".clicked").each(function () {
                    if ($(this).data("sortfield") == sortField) {
                        if (sortDirection == "ascending") {
                            $(this).append($('<span class="pull-right sorter"><i class="glyphicon glyphicon-chevron-up"></i></span>'));
                        } else {
                            $(this).append($('<span class="pull-right sorter"><i class="glyphicon glyphicon-chevron-down"></i></span>'));
                        }
                        return false;
                    }
                    inc++;
                });

            },
            error: function (xhr) {
                alert('error');
            }
        });

    })

$(document).on("click", ".check", function () {
    var empId = $(this).data('id');
    $(".modal-header #empId").val(empId);
});

$(".abc").click(function (evt) {
    var pageindex = $(evt.target).data("pageindex");
    $("#CurrentPageIndex").val(pageindex);
    evt.preventDefault();
    $('.clear-inp').val('');
    $("form").submit();
});    