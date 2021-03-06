﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EmpManagement.Models
{
    //Employee Details 
    public class EmployeeDetails
    {
        [Required]
        public Guid EmployeeID { get; set; }
        [Required(ErrorMessage = "Employee Name is required")]
        [Display(Name = "Employee Name")]
        [StringLength(50)]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        [StringLength(100)]
        public string Address { get; set; }
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Date of birth is required")]
        [ValidBirthDate(ErrorMessage =
         "Date of birth cannot lie after the year 2000")] 
        public DateTime DOB { get; set; }
        [Display(Name = "Salary")]
        [Required(ErrorMessage = "Salary is required")]
        public int salary { get; set; }
        [Required(ErrorMessage = "Please enter an email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email")]
        public string email { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Date of Joining is required")]
        [ValidJoinDate(ErrorMessage =
           "Join Date can not be greater than current date")] 
        public DateTime DOJ { get; set; }
        [Display(Name = "Department")]
        public string Dept { get; set; }
        [Display(Name = "Mobile")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string contact { get; set; }

        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
    }

    // View Model for Employee Details
    public class InsertViewModel
    {
        [Required(ErrorMessage = "Employee Name is required")]
        [Display(Name = "Employee Name")]
        [StringLength(50)]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "Address is required")]
       
        [Display(Name = "Address")]
        [StringLength(100)]
        public string Address { get; set; }
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Date of birth is required")]
        [ValidBirthDate(ErrorMessage =
            "Date of birth cannot lie after the year 2000")] 
        public DateTime DOB { get; set; }
        [Display(Name = "Salary")]
        [Required(ErrorMessage = "Salary is required")]
        public int salary { get; set; }
        [Required(ErrorMessage = "Please enter an email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email")]
        public string email { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        
        [Required(ErrorMessage = "Date of Joining is required")]
        [ValidJoinDate(ErrorMessage =
            "Join Date can not be greater than current date")] 
        public DateTime DOJ { get; set; }
        [Display(Name = "Department")]
        public string Dept { get; set; }
        [Display(Name = "Mobile")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string contact { get; set; }

    
    }
    
    // Pagination Model
    public class PaginationInfo {
        public int pageSize { get; set; }
        public int currPage { get; set; }
        public string sortField { get; set; }
        public string sortDirection { get; set; }
        public string searchString { get; set; }
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