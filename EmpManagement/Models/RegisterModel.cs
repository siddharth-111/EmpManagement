using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EmpManagement.Models
{
    public class RegisterModel
    {

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter your Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a password")]
        [RegularExpression(@"^.{6,}$", ErrorMessage = "Password should be a minimum of 6 characters")]
        public string Password { get; set; }

        [Display(Name = "Username")]
        public string Name { get; set; }

        [Display(Name = "Mobile")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string Contact { get; set; }

        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
       
    
    }
}