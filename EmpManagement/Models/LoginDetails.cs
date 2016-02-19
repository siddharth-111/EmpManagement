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

        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}