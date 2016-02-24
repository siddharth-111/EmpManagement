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
        [Required(ErrorMessage="Employee Name is required")]
        [Display(Name="Employee Name")]
        [StringLength(50)]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage="Address is required")]
        [Display(Name="Address")]
        [StringLength(100)]
        public string Address { get; set; }
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage="Date of birth is required")]
        public DateTime DOB { get; set; }
        [Display(Name="Employee salary")]
        [Required(ErrorMessage="Salary is required")]
        public int salary { get; set; }

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
        [Display(Name = "Employee salary")]
        [Required(ErrorMessage = "Salary is required")]
        public int salary { get; set; }

    }
}