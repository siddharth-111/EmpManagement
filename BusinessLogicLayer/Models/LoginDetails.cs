using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EmpManagement.Models
{

    //Model for login
    public class LoginDetails
    {

        [Display(Name = "Email")]
        [Required(ErrorMessage="Please enter your Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string username { get; set; }
        [Display(Name="Password")]
        [Required(ErrorMessage = "Please enter a password")]
        [RegularExpression(@"^.{6,}$", ErrorMessage = "Password should be a minimum of 6 characters")]
        public string password { get; set; }
       
    }

    //Model for registration
    public class RegisterDetails {

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter your Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string username { get; set; }
        [Required]
        [Display(Name = "Password")]
        [RegularExpression(@"^.{6,}$", ErrorMessage = "Password should be a minimum of 6 characters")]
        public string password { get; set; }
        [Display(Name = "Username")]
        public string name { get; set; }
        [Display(Name = "Mobile")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string phone { get; set; }
        [Compare("password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    
    }
}