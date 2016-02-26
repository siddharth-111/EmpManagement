using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpManagement.Models
{
    public class LoginDetails
    {

        [Display(Name = "Email")]
        [Required(ErrorMessage="Please enter an email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string username { get; set; }
        [Required]
        [Display(Name="Password")]
        public string password { get; set; }
        [Display(Name="Username")]
        public string name { get; set; }
        [Display(Name="Mobile")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string phone { get; set; }
    }
}