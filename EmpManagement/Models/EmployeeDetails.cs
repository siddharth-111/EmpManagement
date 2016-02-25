using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EmpManagement.Models
{

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
        public DateTime DOJ { get; set; }
        [Display(Name = "Department")]
        public string Dept { get; set; }
        [Display(Name = "Mobile")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string contact { get; set; }

    }

    
}