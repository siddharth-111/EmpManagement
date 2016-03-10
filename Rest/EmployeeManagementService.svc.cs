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
            var ReturnList = new List<dynamic>();
            List<EmployeeObject> JsonEmployee = new List<EmployeeObject>();
           ReturnList = BusinessLayerObj.GetAllEmployees(searchString, sortDirection,sortField,pageSize,currPage);
           try {
               var Count = ReturnList.Count();
               for (int i = 0; i < Count - 1; i++)
               {
                   JsonEmployee.Add(new EmployeeObject
                   {
                       EmployeeID = ReturnList[i].GetType().GetProperty("EmployeeID").GetValue(ReturnList[i], null),
                       email = ReturnList[i].GetType().GetProperty("email").GetValue(ReturnList[i], null),
                       EmployeeName = ReturnList[i].GetType().GetProperty("EmployeeName").GetValue(ReturnList[i], null),
                       Address = ReturnList[i].GetType().GetProperty("Address").GetValue(ReturnList[i], null),
                       Dept = ReturnList[i].GetType().GetProperty("Dept").GetValue(ReturnList[i], null),
                       DOJ = ReturnList[i].GetType().GetProperty("DOJ").GetValue(ReturnList[i], null),
                       DOB = ReturnList[i].GetType().GetProperty("DOB").GetValue(ReturnList[i], null),
                       contact = ReturnList[i].GetType().GetProperty("contact").GetValue(ReturnList[i], null),
                       salary = ReturnList[i].GetType().GetProperty("salary").GetValue(ReturnList[i], null),    
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
    }
}
