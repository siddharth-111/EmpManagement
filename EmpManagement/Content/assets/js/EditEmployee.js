$(document).ready(function () {
    $('#DOJ').addClass('requireDOJ dateLowerThanToday create');
    $('#DOB').addClass('requireDOB dateofBirth create');
    if ($(".prompt-reload")[0]) {
        // Do something if class exists

        setTimeout(function () {
            window.location.href = "/Employee";
        }, 2000);
    }
    //validation rules
    $.validator.addMethod("dateLowerThanToday", function (value, element) {
        var myDate = value;
        return Date.parse(myDate) < new Date();
    }, "Joining date must be lesser than today's date");

    $.validator.addMethod("dateofBirth", function (value, element) {
        var myDate = value;
        var limitDate = new Date(1999, 11, 31);
        return Date.parse(myDate) <= limitDate;
    }, "Date of birth cannot lie beyond the year 1999");


    $.validator.addMethod("requireDOJ", function (value, element) {
        return (value != "")
    }, "Date of joining is required");

    $.validator.addMethod("requireDOB", function (value, element) {
        return (value != "")
    }, "Date of birth is required");



    $("#createForm").validate({
        //set this to false if you don't what to set focus on the first invalid input
        onkeyup: function (element, event) {
            if (event.which === 9 && this.elementValue(element) === "") {
                return;
            } else if (element.name in this.submitted || element === this.lastElement) {
                this.element(element);
            }
        },
        errorPlacement: function (error, element) {
            error.appendTo(".errors");
        },
        rules: {
            "Email": {
                required: true,
                email: true,
                maxlength: 50
            },
            "EmployeeName": {
                required: true,
                maxlength: 50
            },
            "Address": {
                required: true,
                maxlength: 200
            },
            "DOJ": {
                required: true,
                date: true,
                dateLowerThanToday: true
            },
            "DOB": {
                required: true,
                date: true,
                dateofBirth: true
            },
            "Salary": {
                required: true,
                number: true
            },
            "Dept": {
                required: true
            },
            "Contact": {
                minlength: 10,
                maxlength: 20,
                number: true
            }
        },
        messages: {
            "Email": {
                required: "Email Address is required",
                email: "Not a valid Email address",
                maxlength: "Email Address cannot exceed 50 characters"
            },
            "EmployeeName": {
                required: "Name is Required",
                maxlength: "Name cannot exceed 50 characters"
            },
            "Address": {
                required: "Address is required",
                maxlength: "Address cannot exceed 200 characters"
            },
            "DOJ": {
                required: "Date of Joining is required",
                date: "Invalid date format"
            },
            "DOB": {
                required: "Date of Birth is required",
                date: "Invalid date format"
            },
            "Salary": {
                required: "Salary is required",
                number: "Salary should only contain numbers"
            },
            "Dept": {
                required: "Department is required"
            },
            "Contact": {
                minlength: "Mobile number should contain more than 10 characters",
                maxlength: "Mobile number cannot exceed 20 characters",
                number: "Mobile number should only contain digits"
            }
        },

        showErrors: function (errorMap, errorList) {
            $("createForm").find("input").each(function () {
                $(this).removeClass("error");
            });
            $(".errors").html("");
            if (errorList.length) {
                $(errorList[0]['element']).addClass("error");
                var elem = $(errorList[0]['element']).siblings(".errors").eq(0);
                elem.html(errorList[0]['message']);
            }
        }
    });

});