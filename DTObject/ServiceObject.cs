using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTObject
{    
    public class UserObject {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Contact { get; set; }
    }

    public class EMSObject
    {
        public Guid EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string Address { get; set; }

        public DateTime DOB { get; set; }

        public int Salary { get; set; }

        public string Email { get; set; }

        public DateTime DOJ { get; set; }

        public string Dept { get; set; }

        public string Contact { get; set; }

        public int Pagecount { get; set;}

        public int TotalRecords { get; set;}
      
    }
    public class ServiceObject
    {
    }
}
