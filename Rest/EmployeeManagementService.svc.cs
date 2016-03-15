using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BLL;
using System.Dynamic;
using CommonUtility;
using System.Web.Script.Serialization;
using DTObject;
using log4net;
using System.Reflection;

namespace Rest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeManagementService" in code, svc and config file together.
    public class EmployeeManagementService : IEmployeeManagementService
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    
        BusinessLogic BusinessLayerObj = new BusinessLogic();
        public List<EmployeeObject> GetEmployeeList(int pageSize, int currPage, string sortField, string sortDirection, string searchString)
        {
            _log.Info("WCF GetemployeeList method start");
            try
            {
                var ReturnList = new List<dynamic>();
                _log.Debug("WCF GetEmployeeList method data passed :" + pageSize + "," + currPage + "," + sortField + "," + sortDirection + "," + searchString);
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
                    Pagecount = ReturnList[Count - 1].GetType().GetProperty("Pagecount").GetValue(ReturnList[Count - 1], null),
                    TotalRecords = ReturnList[Count - 1].GetType().GetProperty("TotalRecords").GetValue(ReturnList[Count - 1], null),
                });
                _log.Debug("WCF GetEmployeeList method data returned:" + new JavaScriptSerializer().Serialize(JsonEmployee));
                return JsonEmployee;
            }
            catch (Exception e)
            {
                _log.Debug("WCF GetEmployeeList method Error :" + e.Message);
            }
            finally
            {

                _log.Info("WCF GetEmployeeList method mandatory stop");
            }
            return null;
        }

        public bool DeleteEmployee(Guid EmpId)
        {
            _log.Info("WCF DeleteEmployee method start");
            try
            {
                _log.Debug("WCF DeleteEmployee method data passed :" + EmpId);
                bool IsEmployeeDeleted = BusinessLayerObj.DeleteEmployee(EmpId);
                _log.Info("WCF DeleteEmployee method stop");
                return IsEmployeeDeleted;
            }
            catch (Exception e)
            {
                _log.Debug("WCF DeleteEmployee method Error :" + e.Message);
                return false;
            }
            finally
            {
                _log.Info("WCF DeleteEmployee method mandatory stop");
            }

        }


        public EmployeeObject GetSingleEmployee(Guid EmployeeID)
        {
            _log.Info("WCF GetSingleEmployee method start");
            try
            {
                _log.Debug("WCF GetSingleEmployee method data passed :" + EmployeeID);
                dynamic SingleData = BusinessLayerObj.GetSingleEmployee(EmployeeID);
                EmployeeObject ReturnEmp = new EmployeeObject
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
                _log.Debug("WCF GetSingleEmployee method returned data :" + new JavaScriptSerializer().Serialize(ReturnEmp));
                return ReturnEmp;
            }
            catch (Exception e)
            {
                _log.Debug("WCF GetSingleEmployee method Error :" + e.Message);
                return null;
            }
            finally
            {
                _log.Info("WCF GetSingleEmployee method mandatory stop");
            }


        }
        public bool CreateEmployee(EmployeeObject employee)
        {
            _log.Info("WCF CreateEmployee method start");
            try
            {
                dynamic SingleEmp = new
                {                    
                    Email = employee.Email,
                    EmployeeName = employee.EmployeeName,
                    Address = employee.Address,
                    Dept = employee.Dept,
                    DOJ = employee.DOJ,
                    DOB = employee.DOB,
                    Contact = employee.Contact,
                    Salary = employee.Salary
                };
                bool EmployeeCreated = BusinessLayerObj.SaveUser(SingleEmp);
                _log.Debug("WCF CreateEmployee method return data :" + EmployeeCreated);
                _log.Info("WCF CreateEmployee method stop");

                return EmployeeCreated;
            }
            catch (Exception e)
            {
                _log.Debug("WCF CreateEmployee method Error :" + e.Message);
                return false;
            }
            finally
            {
                _log.Info("WCF CreateEmployee method mandatory stop");
            }

        }

        public bool EditEmployee(EmployeeObject employee)
        {
            _log.Info("WCF EditEmployee method start");
            try
            {
                EmployeeObject SingleEmp = new EmployeeObject
                {
                    EmployeeID = employee.EmployeeID,
                    Email = employee.Email,
                    EmployeeName = employee.EmployeeName,
                    Address = employee.Address,
                    Dept = employee.Dept,
                    DOJ = employee.DOJ,
                    DOB = employee.DOB,
                    Contact = employee.Contact,
                    Salary = employee.Salary
                };
                bool EmployeeEdited = BusinessLayerObj.EditSingleEmployee(SingleEmp);
                _log.Debug("WCF EditEmployee method return data :" + EmployeeEdited);
                _log.Info("WCF EditEmployee method stop");
                return EmployeeEdited;
            }
            catch (Exception e)
            {
                _log.Debug("WCF EditEmployee method error :" + e.Message);
                return false;
            }
            finally
            {
                _log.Info("WCF EditEmployee method mandatory stop");
            }

        }
    }
}
