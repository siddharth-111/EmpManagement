using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rest
{
    public class DataObject
    {
        public string username { get; set; }

        public string password { get; set; }
    }
    public class EmployeeObject
    {

        public Guid EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string Address { get; set; }

        public string DOB { get; set; }

        public int salary { get; set; }

        public string email { get; set; }

        public string DOJ { get; set; }

        public string Dept { get; set; }
        public string contact { get; set; }
        public int Pagecount { get; set; }
        public int TotalRecords { get; set; }

    }
    
}