using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Rest
{
    
    public class DataObject
    {
        public string username { get; set; }

        public string password { get; set; }
    }

    [DataContract]
    public class EmployeeObject
    {
        [DataMember]
        public Guid EmployeeID { get; set; }
        [DataMember]
        public string EmployeeName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string DOB { get; set; }
        [DataMember]
        public int Salary { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string DOJ { get; set; }
        [DataMember]
        public string Dept { get; set; }
        [DataMember]
        public string Contact { get; set; }
        [DataMember]
        public int Pagecount { get; set; }
        [DataMember]
        public int TotalRecords { get; set; }

    }
    
}