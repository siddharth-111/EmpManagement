using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EmpManagement.Models
{
    public class EmployeeDetails
    {
        [Required(ErrorMessage="ID is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer number")]
        [Display(Name="Employee ID")]
        public int EmployeeID { get; set; }
        [Required(ErrorMessage="Name is required")]
        [Display(Name="Employee Name")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage="Address is required")]
        [Display(Name="Address")]
        public string Address { get; set; }
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage="Date of birth is required")]
        public string DOB { get; set; }
        [Display(Name="Employee salary")]
        [Required(ErrorMessage="Salary is required")]
        public int salary { get; set; }

    }
}