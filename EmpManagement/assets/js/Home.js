﻿$(document).ready(function () {

    //validation rules
    $("#createForm").validate({
        //set this to false if you don't what to set focus on the first invalid input

        errorPlacement: function (error, element) {
            error.appendTo(".errors");
        },
        rules: {
            "username": {
                required: true,
                email: true
            },
            "password" : {
                minlength : 6,
                required : true
            }
        },
        messages: {
            "username": {
                required: "Email is required",
                email: "Not a valid Email"
            },
            "password" : {
                minlength : "Password should contain minimum 6 characters",
                required : "Password is mandatory"
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