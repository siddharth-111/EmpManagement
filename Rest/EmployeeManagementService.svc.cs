using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BLL;
using System.Dynamic;


namespace Rest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeManagementService" in code, svc and config file together.
    public class EmployeeManagementService : IEmployeeManagementService
    {
        BusinessLogic BusinessLayerObj = new BusinessLogic();
        public List<EmployeeObject> GetEmployeeList(string searchString, string sortDirection, string sortField, int pageSize, int currPage) {
            
           try {
               var ReturnList = new List<dynamic>();
               List<EmployeeObject> JsonEmployee = new List<EmployeeObject>();
               ReturnList = BusinessLayerObj.GetAllEmployees(searchString, sortDirection, sortField, pageSize, currPage);
               var Count = ReturnList.Count();
               for (int i = 0; i < Count - 1; i++)
               {
                   JsonEmployee.Add(new EmployeeObject
                   {
                       EmployeeID = ReturnList[i].GetType().GetProperty("EmployeeID").GetValue(ReturnList[i], null),
                       Email = ReturnList[i].GetType().GetProperty("Email").GetValue(ReturnList[i], null),
                       EmployeeName = ReturnList[i].GetType().GetProperty("EmployeeName").GetValue(ReturnList[i], null),
                       Address = ReturnList[i].GetType().GetProperty("Address").GetValue(ReturnList[i], null),
                       Dept = ReturnList[i].GetType().GetProperty("Dept").GetValue(ReturnList[i], null),
                       DOJ = (ReturnList[i].GetType().GetProperty("DOJ").GetValue(ReturnList[i], null)),
                       DOB = (ReturnList[i].GetType().GetProperty("DOB").GetValue(ReturnList[i], null)),
                       Contact = ReturnList[i].GetType().GetProperty("Contact").GetValue(ReturnList[i], null),
                       Salary = ReturnList[i].GetType().GetProperty("Salary").GetValue(ReturnList[i], null),    
                   });
               }
               JsonEmployee.Add(new EmployeeObject
               {
                   Pagecount = ReturnList[Count-1].GetType().GetProperty("Pagecount").GetValue(ReturnList[Count-1], null),
                   TotalRecords = ReturnList[Count-1].GetType().GetProperty("TotalRecords").GetValue(ReturnList[Count-1], null), 
               });
               return JsonEmployee;
           }
           catch (Exception e) {
               Console.Write(e);         
           }               
            return null;
        }

        public bool DeleteEmployee(Guid EmpId)
        {
            bool IsEmployeeDeleted = BusinessLayerObj.DeleteEmployee(EmpId);
            return IsEmployeeDeleted;
        }
        public EmployeeObject GetSingleEmployee(Guid EmployeeID)
        {
            
            try {
                dynamic SingleData = BusinessLayerObj.GetSingleEmployee(EmployeeID);
                EmployeeObject ReturnEmp =  new EmployeeObject
                {
                    EmployeeID = SingleData.GetType().GetProperty("EmployeeID").GetValue(SingleData, null),
                    Email = SingleData.GetType().GetProperty("Email").GetValue(SingleData, null),
                    EmployeeName = SingleData.GetType().GetProperty("EmployeeName").GetValue(SingleData, null),
                    Address = SingleData.GetType().GetProperty("Address").GetValue(SingleData, null),
                    Dept = SingleData.GetType().GetProperty("Dept").GetValue(SingleData, null),
                    DOJ = (SingleData.GetType().GetProperty("DOJ").GetValue(SingleData, null)),
                    DOB = (SingleData.GetType().GetProperty("DOB").GetValue(SingleData, null)),
                    Contact = SingleData.GetType().GetProperty("Contact").GetValue(SingleData, null),
                    Salary = SingleData.GetType().GetProperty("Salary").GetValue(SingleData, null),

                };
                return ReturnEmp;  
            }
            catch (Exception e) {
                Console.Write(e);  
            }
            return null;
          
        }
       public bool CreateEmployee(Guid? EmployeeID, string EmployeeName, string Address, string DOB, int Salary, string Email, string DOJ, string Dept, string Contact)   
        {
            dynamic SingleEmp = new 
            {
                EmployeeID = EmployeeID,
                Email = Email,
                EmployeeName = EmployeeName,
                Address = Address,
                Dept = Dept,
                DOJ = DOJ,
                DOB = DOB,
                Contact = Contact,
                Salary = Salary
            };
           bool EmployeeCreated = BusinessLayerObj.SaveUser(SingleEmp);
           return EmployeeCreated;           
        }

       public bool EditEmployee(Guid EmployeeID, string Email, string EmployeeName, string Address, string DOB, string DOJ, string Dept, string Contact, int Salary)
       {
            EmployeeObject SingleEmp = new EmployeeObject
           {
               EmployeeID = EmployeeID,
               Email = Email,
               EmployeeName = EmployeeName,
               Address = Address,
               Dept = Dept,
               DOJ = DOJ,
               DOB = DOB,
               Contact = Contact,
               Salary = Salary
           };
           bool EmployeeEdited= BusinessLayerObj.EditSingleEmployee(SingleEmp);
           return EmployeeEdited;
       }
    }
}
