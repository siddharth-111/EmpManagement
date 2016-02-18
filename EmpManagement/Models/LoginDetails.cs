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

        [StringLength(50, MinimumLength = 3)]
        public string username { get; set; }
        [Required(ErrorMessage = "An Album Title is required")]
        public string password { get; set; }
    }
}