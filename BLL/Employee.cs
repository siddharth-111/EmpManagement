using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using DataLayer;
using CommonUtility;
using System.Reflection;
using DataObject;
using System.Data;

namespace BusinessLayer
{
    public class Employee
    {

        #region Fields

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        MySQLEmployee MySQLEmployee = new MySQLEmployee();     

        #endregion Fields

        #region Get Methods 

        public List<DataObject.Employee> Retrieve(string searchString, string sortDirection, string sortField, int pageSize, int currPage)
        {
            _log.Info("Retrieve start");
            try
            {
                if (sortDirection == "ascending")
                    sortDirection = "ASC";
                else
                    sortDirection = "DESC";
                List<DataObject.Employee> JsonEmployee = MySQLEmployee.Retrieve(searchString, sortDirection, sortField, currPage, pageSize);
                _log.Debug("Retrieve returned list is :" + JsonEmployee);
                return JsonEmployee;

            }
            catch (Exception e)
            {

                _log.Error("Error in Retrieve,The exception is :" + e);
            }
            finally
            {
                _log.Info("Retrieve mandatory stop");
            }

            return null;
        }

        public DataObject.Employee RetrieveById(string id)
        {
            _log.Info("RetrieveById Start");
            try {
                _log.Debug("RetrieveById employee Id is :" + id);
                
                DataObject.Employee Employee = MySQLEmployee.RetrieveById(id);
            ////    List<Employee> EmployeeList = new Employee().Deserialize(EmployeeTable);
            //    if (EmployeeList.Count == 1)
            //    {
            //        _log.Info("RetrieveById The Employee data is :" + EmployeeList[0]);
            //        return EmployeeList[0];
            //    }
            //    else
            //        return null;
            //    }   
                return Employee;
            }  
            catch (Exception e)
            {
                _log.Error("RetrieveById Exception is :" + e.Message);
                return null;
            }
            finally
            {
                _log.Info("RetrieveById Stop");
            }       
        }

        #endregion Get Methods
        
        #region Post Methods

        public bool Edit(DataObject.Employee employee)
        {
            _log.Info("Edit Start");         
            try
            {
                _log.Debug("Edit data:" + Log.SerializeObject(employee));             
                bool IsEdited = MySQLEmployee.Edit(employee);
                _log.Debug("Edit return data is :" + IsEdited);
                return IsEdited;
            }
            catch (Exception e)
            {

                _log.Error("Error in Edit, the data is :" + e);
            }
            finally
            {
                _log.Info("Edit stop");
            }
            return false;

        }

        public bool Create(DataObject.Employee newEmployee)
        {
            _log.Info("Create Start");
           try
           {
               _log.Debug(" Create, The Employee data is :" + Log.SerializeObject(newEmployee));                     
               bool EmpCreated = MySQLEmployee.Create(newEmployee);
               _log.Debug("Create,Is User saved? :" + EmpCreated);
               return EmpCreated;

           }
           catch (Exception e)
           {
               _log.Error("Create error is :" + e.Message);
           }
           finally
           {
               _log.Info("Create Stop");           
           }
            return false;
       }

        public bool Delete(string id)
        {
            _log.Info("Business Layer Delete Start");
            try             
            {
                bool Val = MySQLEmployee.Delete(id);

                _log.Debug("Business Layer Delete,is User Deleted? :" + Val);
                
                return Val;
            }
            catch (Exception e)
            {
                _log.Error("Business Layer Delete exception :" + e.Message);
                return false;
            }
            finally
            {
                _log.Info("Business Layer Delete Stop");
            }
        
         
        }

        #endregion Post Methods

    }
}
