var sortField, sortDirection, currentPage, pageSize;
pageSize = 10;
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
        url: "/Employee/ReturnEmployeeData",
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
                    trHTML += '<tr><td>' + employee.EmployeeName + '</td><td>' + employee.Email + '</td><td>' + employee.Address + '</td><td>' + employee.Dept + '</td><td>' + ((new Date(parseInt(employee.DOJ.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOJ.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOJ.substr(6))).getFullYear()) + '</td><td>' + ((new Date(parseInt(employee.DOB.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOB.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOB.substr(6))).getFullYear()) + '</td><td>' + employee.Contact + '</td><td>' + employee.Salary +
                '</td><td style="text-align: center;">' +
                  '<div class="dropdown"><a href = "" data-toggle="dropdown" class="dropdown-toggle"><span class="glyphicon glyphicon-cog"></span></a><ul class="dropdown-menu"><li><a href="/Employee/EditEmployee/' + employee.EmployeeID + '">Edit</a></li><li><a data-toggle="modal" href="" id = "openModal" class = "check" data-target="#myModal" data-id="' + employee.EmployeeID + '"> Delete</a></li> </ul>' + '</div>' + '</td>' + '</tr>';
                }
                else {
                    var pageCount = employee.Pagecount;
                    if ((pagingInfo.pageSize * (pagingInfo.currPage + 1)) > employee.TotalRecords)
                        pages += '<span>Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + employee.TotalRecords + ' out of ' + employee.TotalRecords + '</span>';
                    else
                        pages += '<span>Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + (pagingInfo.pageSize * (pagingInfo.currPage + 1)) + ' out of ' + employee.TotalRecords + '</span>';
                    pages += '<ul class = "pagination pull-right">';
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i - 1) > 0) {
                                pages += '<li><a href = "#" onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">&laquo;</a></li>'
                            }
                        }
                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (currentPage + 1)) {
                            pages += '<li class="active"><a href = "#" onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                        }
                        else {
                            pages += '<li><a href = "#" onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                        }
                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i) < pageCount) {
                                pages += '<li><a href = "#" onclick ="getPage(' + (i) + ',' + pagingInfo.pageSize + ')">&raquo;</a></li>'
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

$(document).ready(function () {

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
    $.ajax({
        url: "/Employee/ReturnEmployeeData",
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
                    trHTML += '<tr><td>' + employee.EmployeeName + '</td><td>' + employee.Email + '</td><td>' + employee.Address + '</td><td>' + employee.Dept + '</td><td>' + ((new Date(parseInt(employee.DOJ.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOJ.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOJ.substr(6))).getFullYear()) + '</td><td>'+((new Date(parseInt(employee.DOB.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOB.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOB.substr(6))).getFullYear()) + '</td><td>' + employee.Contact + '</td><td>' + employee.Salary +
                '</td><td style="text-align: center;">' +
                  '<div class="dropdown"><a  href = "" data-toggle="dropdown" class="dropdown-toggle"><span class="glyphicon glyphicon-cog"></span></a><ul class="dropdown-menu"><li><a href="/Employee/EditEmployee/' + employee.EmployeeID + '">Edit</a></li><li><a data-toggle="modal" href="" id = "openModal" class = "check" data-target="#myModal" data-id="' + employee.EmployeeID + '"> Delete</a></li> </ul>' + '</div>' + '</td>' + '</tr>';
                }
                else {
                    var pageCount = employee.Pagecount;
                    pages += '<span>Records ' + (pagingInfo.currPage + 1) + '-' + pagingInfo.pageSize * (pagingInfo.currPage + 1) + ' out of ' + employee.TotalRecords + '</span>';
                    pages += '<ul class = "pagination pull-right">';

                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i - 1) > 0) {
                                pages += '<li><a href = "#" onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">&laquo;</a></li>'
                            }
                        }
                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            pages += '<li class="active"><a href = "#" onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                        } else {
                            pages += '<li><a href = "#" onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                        }

                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i) < pageCount) {
                                pages += '<li><a href = "#" onclick ="getPage(' + (i) + ',' + pagingInfo.pageSize + ')">&raquo;</a></li>'
                            }
                        }
                    }

                    pages += '</ul>';
                }

            });

            $('#employeeTable').append(trHTML);
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
            $('#paginationDiv').append(pages);
        },
        error: function (xhr) {
            alert('error');
        }
    });
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
        url: "/Employee/ReturnEmployeeData",
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
                          trHTML += '<tr class="removeClass"><td colspan="9"><div class="text-center"> No employees in the grid</div></td><tr>';
                          return false;
                    }
                    trHTML += '<tr><td>' + employee.EmployeeName + '</td><td>' + employee.Email + '</td><td>' + employee.Address + '</td><td>' + employee.Dept + '</td><td>' + ((new Date(parseInt(employee.DOJ.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOJ.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOJ.substr(6))).getFullYear()) + '</td><td>' + ((new Date(parseInt(employee.DOB.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOB.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOB.substr(6))).getFullYear()) + '</td><td>' + employee.Contact + '</td><td>' + employee.Salary +
                '</td><td style="text-align: center;">' +
                  '<div class="dropdown"><a href = "" data-toggle="dropdown" class="dropdown-toggle"><span class="glyphicon glyphicon-cog"></span></a><ul class="dropdown-menu"><li><a href="/Employee/EditEmployee/' + employee.EmployeeID + '">Edit</a></li><li><a data-toggle="modal" href="" id = "openModal" class = "check" data-target="#myModal" data-id="' + employee.EmployeeID + '"> Delete</a></li> </ul>' + '</div>' + '</td>' + '</tr>';
                }
                else {
                    var pageCount = employee.Pagecount;
                    if ((pagingInfo.pageSize * (pagingInfo.currPage + 1)) > employee.TotalRecords)
                        pages += '<span>Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + employee.TotalRecords + ' out of ' + employee.TotalRecords + '</span>';
                    else
                        pages += '<span>Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + (pagingInfo.pageSize * (pagingInfo.currPage + 1)) + ' out of ' + employee.TotalRecords + '</span>';
                    pages += '<ul class = "pagination pull-right">';
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i - 1) > 0) {
                                pages += '<li><a href = "#" onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">&laquo;</a></li>'
                            }
                        }
                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (currentPage + 1)) {
                            pages += '<li class="active"><a href = "#" onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                        }
                        else {
                            pages += '<li><a href = "#" onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                        }
                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i) < pageCount) {
                                pages += '<li><a href = "#" onclick ="getPage(' + (i) + ',' + pagingInfo.pageSize + ')">&raquo;</a></li>'
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
        url: "/Employee/ReturnEmployeeData",
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
                    trHTML += '<tr><td>' + employee.EmployeeName + '</td><td>' + employee.Email + '</td><td>' + employee.Address + '</td><td>' + employee.Dept + '</td><td>' + ((new Date(parseInt(employee.DOJ.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOJ.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOJ.substr(6))).getFullYear()) + '</td><td>' + ((new Date(parseInt(employee.DOB.substr(6))).getMonth() + 1) + '/' + new Date(parseInt(employee.DOB.substr(6))).getDate() + '/' + new Date(parseInt(employee.DOB.substr(6))).getFullYear()) + '</td><td>' + employee.Contact + '</td><td>' + employee.Salary +
                '</td><td style="text-align: center;">' +
                  '<div class="dropdown"><a  href = "" data-toggle="dropdown" class="dropdown-toggle"><span class="glyphicon glyphicon-cog"></span></a><ul class="dropdown-menu"><li><a href="/Employee/EditEmployee/' + employee.EmployeeID + '">Edit</a></li><li><a data-toggle="modal" href="" id = "openModal" class = "check" data-target="#myModal" data-id="' + employee.EmployeeID + '"> Delete</a></li> </ul>' + '</div>' + '</td>' + '</tr>';
                }
                else {
                    var pageCount = employee.Pagecount;
                    if ((pagingInfo.pageSize * (pagingInfo.currPage + 1)) > employee.TotalRecords)
                        pages += '<span>Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + employee.TotalRecords + ' out of ' + employee.TotalRecords + '</span>';
                    else
                        pages += '<span>Records ' + ((pagingInfo.pageSize * pagingInfo.currPage) + 1) + '-' + (pagingInfo.pageSize * (pagingInfo.currPage + 1)) + ' out of ' + employee.TotalRecords + '</span>';
                    pages += '<ul class = "pagination pull-right">';

                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i - 1) > 0) {
                                pages += '<li><a href = "#" onclick ="getPage(' + (i - 2) + ',' + pagingInfo.pageSize + ')">&laquo;</a></li>'
                            }
                        }
                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            pages += '<li class="active"><a href = "#" onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                        } else {
                            pages += '<li><a href = "#" onclick ="getPage(' + (i - 1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
                        }

                    }
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == (pagingInfo.currPage + 1)) {
                            if ((i) < pageCount) {
                                pages += '<li><a href = "#" onclick ="getPage(' + (i) + ',' + pagingInfo.pageSize + ')">&raquo;</a></li>'
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