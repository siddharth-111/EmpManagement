$(document).ready(function () {

    //validation rules
    $.validator.addMethod("dateLowerThanToday", function(value, element) {
         var myDate = value;
        return Date.parse(myDate) < new Date();
        }, "Joining date must be lesser than today's date");

      $.validator.addMethod("dateofBirth", function(value, element) {
         var myDate = value;
         var limitDate = new Date();
         limitDate.setFullYear(2000);
        return Date.parse(myDate) < limitDate;
        }, "Date of birth cannot lie beyond the year 2000");

    $("#createForm").validate({
        //set this to false if you don't what to set focus on the first invalid input

        errorPlacement: function (error, element) {
            error.appendTo(".errors");
        },
        rules: {
            "email": {
                required: true,
                email: true
            },
            "EmployeeName": {
                required: true
            },
            "Address": {
                required: true
            },
            "DOJ": {
                required: true,
                date : true,
            },
            "DOB" : {
                required:true,
                date : true
            },
            "salary" : {
                required : true
            },
            "contact" : {
                minlength : 10,
                number : true
            }
        },
        messages: {
            "email": {
                required: "Email is required",
                email: "Not a valid email"
            },
            "EmployeeName": {
                required: "You must enter Employee name"
            },
            "Address": {
                required: "Address is required"
            },
            "DOJ" : {
                required : "Date of Joining is mandatory",
                date : "Invalid date format"
            },
            "DOB" : {
                required : "Date of Birth is mandatory",
                date : "Invalid date format"
            },
            "salary" : {
                required : "Salary is required"
            },
            "contact" : {
                minlength : "Mobile number should contain more than 10 characters",
                number : "Mobile number should only contain digits"
            }
        },

        showErrors: function (errorMap, errorList) {
            $("createForm").find("input").each(function () {
                $(this).removeClass("error");
            });
            $(".errors").html("");
            if (errorList.length) {                
                $(errorList[0]['element']).addClass("error");
                var elem = $(errorList[0]['element']).next();
                elem.html(errorList[0]['message']);
            }
        }
    });

});