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

        #region Fields

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
       // BusinessLogic BusinessLayerObj = new BusinessLogic();
        BusinessLayer.Employee BLEmployee = new BusinessLayer.Employee();        

        #endregion Fields       

        #region Get Methods

        public List<DataObject.Employee> Retrieve(int PageSize, int CurrPage, string SortField, string SortDirection, string SearchString)
        {
            _log.Info("Rest Retrieve method start");

            try
            {

                _log.Debug("Rest Retrieve method data passed :" + PageSize + "," + CurrPage + "," + SortField + "," + SortDirection + "," + SearchString);

                List<DataObject.Employee> JsonEmployee = BLEmployee.Retrieve(SearchString, SortDirection, SortField, PageSize, CurrPage);

                _log.Debug("Rest Retrieve method data returned:" + Log.SerializeObject(JsonEmployee));

                return JsonEmployee;
            }
            catch (Exception e)
            {

                _log.Debug("Rest Retrieve method Error :" + e.Message);

            }
            finally
            {

                _log.Info("Rest Retrieve method mandatory stop");

            }

            return null;
        }

        public DataObject.Employee RetrieveById(string EmployeeID)
        {

            _log.Info("Rest RetrieveById method start");

            try
            {

                _log.Debug("Rest RetrieveById method data passed :" + EmployeeID);

                DataObject.Employee Employee = BLEmployee.RetrieveById(EmployeeID);

                _log.Debug("Rest RetrieveById method returned data :" + Log.SerializeObject(Employee));

                return Employee;

            }
            catch (Exception e)
            {

                _log.Debug("Rest RetrieveById method Error :" + e.Message);

                return null;

            }
            finally
            {

                _log.Info("Rest RetrieveById method mandatory stop");

            }


        }

        #endregion Get Methods

        #region Post Methods

        public bool Create(DataObject.Employee employee)
        {

            _log.Info("Rest Create method start");

            try
            {

                _log.Debug("Rest Create method returned data :" + Log.SerializeObject(employee));

                bool EmployeeCreated = BLEmployee.Create(employee);

                _log.Debug("Rest Create method return data :" + EmployeeCreated);

                return EmployeeCreated;

            }
            catch (Exception e)
            {
                _log.Debug("Rest Create method Error :" + e.Message);

                return false;

            }
            finally
            {

                _log.Info("Rest Create method stop");

            }

        }

        public bool Edit(DataObject.Employee employee)
        {

            _log.Info("Rest Edit method start");

            try
            {

                _log.Debug("Rest Edit method returned data :" + Log.SerializeObject(employee));

                bool EmployeeEdited = BLEmployee.Edit(employee);

                _log.Debug("Rest Edit method return data :" + EmployeeEdited);

                return EmployeeEdited;

            }
            catch (Exception e)
            {

                _log.Debug("Rest Edit method error :" + e.Message);

                return false;

            }
            finally
            {

                _log.Info("Rest Edit method mandatory stop");

            }

        }

        public bool Delete(string EmpId)
        {

            _log.Info("Rest Delete method start");

            try
            {

                _log.Debug("Rest Delete method data passed :" + EmpId);

                bool IsEmployeeDeleted = BLEmployee.Delete(EmpId);

                return IsEmployeeDeleted;

            }
            catch (Exception e)
            {

                _log.Debug("Rest Delete method Error :" + e.Message);

                return false;

            }
            finally
            {

                _log.Info("Rest Delete method stop");

            }

        }

        #endregion Post Methods
    
    }
}
