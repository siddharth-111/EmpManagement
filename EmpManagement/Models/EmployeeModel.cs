﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EmployeeManagement.Models
{
    public class EmployeeModel
    {
        [Required]
        public Guid EmployeeID { get; set; }
        [Required(ErrorMessage = "Employee Name is required")]
        [Display(Name = "Employee Name")]
        [StringLength(50)]
        [AllowHtml]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        [StringLength(200)]
        [AllowHtml]
        public string Address { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Date of birth is required")]
        [ValidBirthDate(ErrorMessage =
         "Date of birth cannot lie after the year 2000")]
        public Nullable<System.DateTime> DOB { get; set; }

        [Display(Name = "Salary")]       
        public int Salary { get; set; }

        [Required(ErrorMessage = "Please enter an email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email")]
        [StringLength(50)]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Date of Joining is required")]
        [ValidJoinDate(ErrorMessage =
           "Join Date can not be greater than current date")]
        public Nullable<System.DateTime> DOJ { get; set; }


        [Display(Name = "Department")]
        [Required(ErrorMessage = "Department is required")]
        public string Dept { get; set; }


        [Display(Name = "Mobile")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string Contact { get; set; }

        public int Pagecount { get; set; }

        public int TotalRecords { get; set; }
    }


    //Validate joining date
    public class ValidJoinDate : ValidationAttribute
    {
        protected override ValidationResult
                IsValid(object value, ValidationContext validationContext)
        {
            DateTime _dateJoin = Convert.ToDateTime(value);
            if (_dateJoin < DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult
                    ("Join date can not be greater than current date.");
            }
        }
    }

    //validate date of birth
    public class ValidBirthDate : ValidationAttribute
    {
        protected override ValidationResult
                IsValid(object value, ValidationContext validationContext)
        {
            DateTime _dateofBirth = Convert.ToDateTime(value);
            if (_dateofBirth < DateTime.Now.AddYears(-16))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult
                    ("Date of birth cannot lie after the year 2000");
            }
        }
    }
}