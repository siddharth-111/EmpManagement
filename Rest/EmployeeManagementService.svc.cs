using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BusinessLayer;
using System.Dynamic;
using CommonUtility;
using System.Web.Script.Serialization;
using DataObject;
using log4net;
using System.Reflection;

namespace Rest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeManagementService" in code, svc and config file together.
    public class EmployeeManagementService : IEmployeeManagementService
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        Serializer ObjectSerializer = new Serializer();
        BusinessLogic BusinessLayerObj = new BusinessLogic();
        public List<EmployeeObject> GetEmployeeList(int PageSize, int CurrPage, string SortField, string SortDirection, string SearchString)
        {
            _log.Info("Rest GetemployeeList method start");
            try
            {
                
                _log.Debug("Rest GetEmployeeList method data passed :" + PageSize + "," + CurrPage + "," + SortField + "," + SortDirection + "," + SearchString);
                List<EmployeeObject> JsonEmployee = BusinessLayerObj.GetAllEmployees(SearchString, SortDirection, SortField, PageSize, CurrPage);
             
                _log.Debug("Rest GetEmployeeList method data returned:" + ObjectSerializer.SerializeObject(JsonEmployee));
                return JsonEmployee;
            }
            catch (Exception e)
            {
                _log.Debug("Rest GetEmployeeList method Error :" + e.Message);
            }
            finally
            {

                _log.Info("Rest GetEmployeeList method mandatory stop");
            }
            return null;
        }

        public bool DeleteEmployee(Guid EmpId)
        {
            _log.Info("Rest DeleteEmployee method start");
            try
            {
                _log.Debug("Rest DeleteEmployee method data passed :" + EmpId);
                bool IsEmployeeDeleted = BusinessLayerObj.DeleteEmployee(EmpId);
                return IsEmployeeDeleted;
            }
            catch (Exception e)
            {
                _log.Debug("Rest DeleteEmployee method Error :" + e.Message);
                return false;
            }
            finally
            {
                _log.Info("Rest DeleteEmployee method stop");
            }

        }


        public EmployeeObject GetSingleEmployee(Guid EmployeeID)
        {
            _log.Info("Rest GetSingleEmployee method start");
            try
            {
                _log.Debug("Rest GetSingleEmployee method data passed :" + EmployeeID);
                EmployeeObject Employee = BusinessLayerObj.GetSingleEmployee(EmployeeID);             
                _log.Debug("Rest GetSingleEmployee method returned data :" + ObjectSerializer.SerializeObject(Employee));
                return Employee;
            }
            catch (Exception e)
            {
                _log.Debug("Rest GetSingleEmployee method Error :" + e.Message);
                return null;
            }
            finally
            {
                _log.Info("Rest GetSingleEmployee method mandatory stop");
            }


        }
        public bool CreateEmployee(EmployeeObject employee)
        {
            _log.Info("Rest CreateEmployee method start");
            try
            {
                _log.Debug("Rest CreateEmployee method returned data :" + ObjectSerializer.SerializeObject(employee));
               
                bool EmployeeCreated = BusinessLayerObj.SaveUser(employee);
                _log.Debug("Rest CreateEmployee method return data :" + EmployeeCreated);
                return EmployeeCreated;
            }
            catch (Exception e)
            {
                _log.Debug("Rest CreateEmployee method Error :" + e.Message);
                return false;
            }
            finally
            {
                _log.Info("Rest CreateEmployee method stop");
            }

        }

        public bool EditEmployee(EmployeeObject employee)
        {
            _log.Info("Rest EditEmployee method start");
            try
            {
                _log.Debug("Rest EditEmployee method returned data :" + ObjectSerializer.SerializeObject(employee));
                 bool EmployeeEdited = BusinessLayerObj.EditSingleEmployee(employee);
                _log.Debug("Rest EditEmployee method return data :" + EmployeeEdited);
                return EmployeeEdited;
            }
            catch (Exception e)
            {
                _log.Debug("Rest EditEmployee method error :" + e.Message);
                return false;
            }
            finally
            {
                _log.Info("Rest EditEmployee method mandatory stop");
            }

        }
    }
}
