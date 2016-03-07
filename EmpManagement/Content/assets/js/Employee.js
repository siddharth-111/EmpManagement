function getPage(currentPage, pageSize) {
    pagingInfo = {};
    pagingInfo.pageSize = pageSize;
    pagingInfo.currPage = currentPage;
    $.ajax({
        url: "/Employee/ReturnEmp",
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
                    trHTML += '<tr><td>' + employee.email + '</td><td>' + employee.EmployeeName + '</td><td>' + employee.Address + '</td><td>' + employee.Dept + '</td><td>' + employee.DOJ + '</td><td>' + employee.DOB + '</td><td>' + employee.contact + '</td><td>' + employee.salary +
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

        },
        error: function (xhr) {
            alert('error');
        }
    });
}

$(document).ready(function () {
    $('#search').keyup(function () {
        searchTable($(this).val());
    });

    pagingInfo = {};
    pagingInfo.pageSize = 4;
    pagingInfo.currPage = 0;
    $.ajax({
        url: "/Employee/ReturnEmp",
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
                    trHTML += '<tr><td>' + employee.email + '</td><td>' + employee.EmployeeName + '</td><td>' + employee.Address + '</td><td>' + employee.Dept + '</td><td>' + employee.DOJ + '</td><td>' + employee.DOB + '</td><td>' + employee.contact + '</td><td>' + employee.salary +
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
                            pages += '<li><a href = "#" onclick ="getPage(' + (i-1) + ',' + pagingInfo.pageSize + ')">' + i + '</a></li>'
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
            $('#paginationDiv').append(pages);
        },
        error: function (xhr) {
            alert('error');
        }
    });


});
$('th').click(function () {
    var table = $(this).parents('table').eq(0)
    table.find('.sorter').remove();
    var rows = table.find('tr:gt(0)').toArray().sort(comparer($(this).index()))

    this.asc = !this.asc
    if (!this.asc) {
        rows = rows.reverse();
        $(this).append($('<span class="pull-right sorter"><i class="glyphicon glyphicon-chevron-down"></i></span>'));
    }
    else {

        $(this).append($('<span class="pull-right sorter"><i class="glyphicon glyphicon-chevron-up"></i></span>'));
    }
    for (var i = 0; i < rows.length; i++) { table.append(rows[i]) }
})
function comparer(index) {
    return function (a, b) {
        var valA = getCellValue(a, index), valB = getCellValue(b, index)
        return $.isNumeric(valA) && $.isNumeric(valB) ? valA - valB : (Date.parse(valA) && Date.parse(valB)) ? new Date(valA).getTime() > new Date(valB).getTime() : valA.localeCompare(valB);
    }
}
function getCellValue(row, index) { return $(row).children('td').eq(index).html() }
$(document).on("click", ".check", function () {
    var empId = $(this).data('id');
    $(".modal-header #empId").val(empId);
});

//Function to get value
function searchTable(inputVal) {
    var table = $('#employeeTable');
    var checkNull = 0;
    var pHTML = '';
    table.find('tr').each(function (index, row) {
        var allCells = $(row).find('td');
        if (allCells.length > 0) {
            var found = false;
            allCells.each(function (index, td) {
                var regExp = new RegExp(inputVal, 'i');
                if (regExp.test($(td).text())) {
                    found = true;
                    return false;
                }
            });
            if (found == true) {
                $(row).show();
                checkNull++;
            }
            else {

                $(row).hide();
            }

        }
    });
    if (checkNull == 0) {
        pHTML += '<tr class="removeClass"><td colspan="9"><div class="text-center"> No employees in the grid</div></td><tr>';
        table.append(pHTML);
    } else
        $('.removeClass').remove();
}

