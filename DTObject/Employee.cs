using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;

namespace DataObject
{
    [DataContract]
    public class Employee
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

            
            public List<Employee> Deserialize(DataTable employee)
            {
                List<Employee> EmployeeList = new List<Employee>();
                try 
                { 
                 foreach (DataRow row in employee.Rows)
                    {
                    EmployeeList.Add(new Employee
                    {
                        EmployeeID = Guid.Parse(row["EmployeeID"].ToString()),
                        EmployeeName = Convert.ToString(row["Name"].ToString()),
                        Email = Convert.ToString(row["Email"].ToString()),
                        Address = Convert.ToString(row["Address"].ToString()),
                        Contact = Convert.ToString(row["Contact"].ToString()),
                        Dept = Convert.ToString(row["Dept"].ToString()),
                        DOB = Convert.ToString(row["DOB"].ToString()),
                        DOJ = Convert.ToString(row["DOJ"].ToString()),
                        Salary = Convert.ToInt32(row["Salary"].ToString())
                    });
                 }
                    return EmployeeList;
                }
                catch (Exception e)
                { 
                    return null;
                }
                finally
                { 
          
                }
               
                
                
            }
       
    }
}
