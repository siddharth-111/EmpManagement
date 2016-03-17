using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using DataObject;
using DataLayer;
using System.Web.Script.Serialization;
using CommonUtility;
using log4net;
using System.Reflection;
namespace BusinessLayer
{

    public class BusinessLogic
    {
      //  private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Check if the Login user is valid
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //FileAccess DataLayerObj = new FileAccess();
        SQLRetrieve SqlLayerObj = new SQLRetrieve();
        Serializer ObjectSerializer = new Serializer();    

      

        //Delete Employee 
        public bool DeleteEmployee(string id)
        {
            _log.Info("Business Layer DeletEmployee Start");
            bool Val = SqlLayerObj.DeleteData(id);
            _log.Info("Business Layer DeleteEmployee Stop,is User Deleted? :" + Val);
            _log.Info("Business Layer DeleteEmployee Stop");
            return Val;
        }

        ////Update single employee data
        //public bool EditSingleEmployee(dynamic newEmployee)
        //{
        //    log.Info("Business Layer EditSingleEmployee Start,The data is :" + newEmployee);
        //    StringBuilder DataOfEmp = new StringBuilder();
        //    try {

        //        dynamic TempEmp = new
        //        {
        //            EmployeeID = newEmployee.GetType().GetProperty("EmployeeID").GetValue(newEmployee, null),
        //            Email = newEmployee.GetType().GetProperty("Email").GetValue(newEmployee, null),
        //            EmployeeName = newEmployee.GetType().GetProperty("EmployeeName").GetValue(newEmployee, null),
        //            Address = newEmployee.GetType().GetProperty("Address").GetValue(newEmployee, null),
        //            Dept = newEmployee.GetType().GetProperty("Dept").GetValue(newEmployee, null),
        //            DOJ = (newEmployee.GetType().GetProperty("DOJ").GetValue(newEmployee, null)),
        //            DOB = (newEmployee.GetType().GetProperty("DOB").GetValue(newEmployee, null)),
        //            Contact = newEmployee.GetType().GetProperty("Contact").GetValue(newEmployee, null),
        //            Salary = newEmployee.GetType().GetProperty("Salary").GetValue(newEmployee, null)

        //        };
        //        DataOfEmp.Append(TempEmp.EmployeeID.ToString()).Append("|").Append(TempEmp.Email).Append("|").Append(TempEmp.EmployeeName).Append("|").Append(TempEmp.Address).Append("|").Append((TempEmp.Dept != null) ? TempEmp.Dept : "").Append("|").Append(TempEmp.DOJ.ToString()).Append("|").Append(TempEmp.DOB.ToString()).Append("|").Append(TempEmp.Contact != null ? TempEmp.Contact.ToString() : "").Append("|").Append(TempEmp.Salary.ToString());
        //        bool IsEdited = DataLayerObj.UpdateData(DataOfEmp.ToString(), TempEmp.EmployeeID.ToString());
        //        log.Info("Business Layer EditSingleEmployee Stop,The data is :" + TempEmp);
        //        return IsEdited;
        //    }
        //    catch (Exception e) {

        //        log.Error("Error in saving updated details of the employee, the data is :" + e);
        //    }
        //    return false;
         
        //}


        //Update single employee data
        public bool EditSingleEmployee(EmployeeObject employee)
        {
           _log.Info("Business Layer EditSingleEmployee Start");
            StringBuilder DataOfEmp = new StringBuilder();
            try
            {
                _log.Debug("Business Layer EditSingleEmployee data:" + ObjectSerializer.SerializeObject(employee));             
                bool IsEdited = SqlLayerObj.Edit(employee);
                _log.Debug("Business Layer EditSingleEmployee return data is :" + IsEdited);
                return IsEdited;
            }
            catch (Exception e)
            {

                _log.Error("Error in saving updated details of the employee, the data is :" + e);
            }
            finally
            {
                _log.Info("Business Layer EditSingleEmployee stop");
            }
            return false;

        }
       


        //Get the entire list of employees        



        public List<EmployeeObject> GetAllEmployees(string searchString, string sortDirection, string sortField, int pageSize, int currPage)
        {
            _log.Info("Business layer GetallEmployees start");
            try
            {
                if (sortDirection == "ascending")
                    sortDirection = "ASC";
                else
                    sortDirection = "DESC";
                List <EmployeeObject> JsonEmployee = SqlLayerObj.GetEmployeeData(searchString, sortDirection, sortField, currPage, pageSize);
                _log.Debug("Business Layer GetAllEmployees returned list is :" + JsonEmployee);              
                return JsonEmployee;

            }
            catch (Exception e)
            {

                _log.Error("Business Layer Error in GetAllEmployees,The exception is :" + e);
            }
            finally
            {
                _log.Info("Business Layer GetAllEmployees mandatory stop");
            }
           
            return null;
        }

      

        //Using MysQL 
        public EmployeeObject GetSingleEmployee(string id)
        {
            _log.Info("Business Layer GetSingleEmployee Start");
            try {
                _log.Debug("Business Layer GetSingleEmployee employee Id is :" + id);
                EmployeeObject Employee = SqlLayerObj.GetSingleEmpData(id);              
                   _log.Info("Business Layer GetSingleEmployee Stop,The Employee data is :" + Employee);
                    return Employee;
                }                         
            catch (Exception e)
            {
                _log.Error("Business Layer GetSingleEmployee Exception is :" + e.Message);
                return null;
            }
            finally
            {
                _log.Info("Business Layer GetSingleEmployee Stop");
            }       
        }


        //Registering a new user
        //public bool SaveUser(dynamic newEmployee)
        //{
        //    try {
        //        log.Info("Business Layer SaveUser Start,The Employee data is :" + newEmployee);
        //        string GuidEmp = Guid.NewGuid().ToString();
        //        StringBuilder DataOfEmp = new StringBuilder();
             
        //        dynamic TempEmp = new
        //        {
        //            EmployeeID = newEmployee.GetType().GetProperty("EmployeeID").GetValue(newEmployee, null),
        //            Email = newEmployee.GetType().GetProperty("Email").GetValue(newEmployee, null),
        //            EmployeeName = newEmployee.GetType().GetProperty("EmployeeName").GetValue(newEmployee, null),
        //            Address = newEmployee.GetType().GetProperty("Address").GetValue(newEmployee, null),
        //            Dept = newEmployee.GetType().GetProperty("Dept").GetValue(newEmployee, null),
        //            DOJ = (newEmployee.GetType().GetProperty("DOJ").GetValue(newEmployee, null)),
        //            DOB = (newEmployee.GetType().GetProperty("DOB").GetValue(newEmployee, null)),
        //            Contact = newEmployee.GetType().GetProperty("Contact").GetValue(newEmployee, null),
        //            Salary = newEmployee.GetType().GetProperty("Salary").GetValue(newEmployee, null)

        //        };
        //        DataOfEmp.Append(GuidEmp).Append("|").Append(TempEmp.Email).Append("|").Append(TempEmp.EmployeeName).Append("|").Append(TempEmp.Address).Append("|").Append((TempEmp.Dept != null) ? TempEmp.Dept : "").Append("|").Append(TempEmp.DOJ.ToString()).Append("|").Append(TempEmp.DOB.ToString()).Append("|").Append(TempEmp.Contact != null ? TempEmp.Contact.ToString() : "").Append("|").Append(TempEmp.Salary.ToString());
        //        bool ret = DataLayerObj.SaveData(DataOfEmp.ToString(), TempEmp.Email.ToString(),"Employee.txt");
        //        log.Info("Business Layer SaveUser Stop,Is User saved? :" + ret);
        //        return ret;
            
        //    }
        //    catch (Exception e) {
        //        log.Error("Business Layer SaveUser Stop err,The error is is :" + e.Message);
        //    }
        //    return false;
            
        //}
        //Registering a new user
        public bool SaveUser(EmployeeObject newEmployee)
        {
           _log.Info("Business Layer SaveUser Start");
           try
           {
               _log.Debug("The Employee data is :" + ObjectSerializer.SerializeObject(newEmployee));                     
               bool EmpCreated = SqlLayerObj.CreateData(newEmployee);
               _log.Debug("Business Layer SaveUser,Is User saved? :" + EmpCreated);
               return EmpCreated;

           }
           catch (Exception e)
           {
               _log.Error("Business Layer SaveUser error is :" + e.Message);
           }
           finally
           {
               _log.Info("Business Layer SaveUser Stop");           
           }
            return false;
       }
    }
}
