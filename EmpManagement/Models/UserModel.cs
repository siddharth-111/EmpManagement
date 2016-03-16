using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpManagement.Models
{
    public class UserModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter your Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a password")]
        [RegularExpression(@"^.{6,}$", ErrorMessage = "Password should be a minimum of 6 characters")]
        public string Password { get; set; }
    }
}