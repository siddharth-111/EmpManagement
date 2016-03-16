using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmpManagement.Models;
using System.Data;
using System.Dynamic;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Configuration;
using CommonUtility;
using log4net;

namespace EmpManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
       
        #region Fields
        string EmpServiceURL = ConfigurationManager.AppSettings["EmpServiceURL"];

        CommonGetPost ApiCall = new CommonGetPost();

        Serializer ObjectSerializer = new Serializer();

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #endregion Fields

        #region Get Methods 

        //Get the Result of employee object
        public JsonResult Retrieve(CriteriaModel pagingInfo)
        {

            _log.Info("ReturnEmployeeData method start");

            try
            {
                _log.Debug("Retrieve method CriteriaModel info data:" + ObjectSerializer.SerializeObject(pagingInfo));

                string Url = EmpServiceURL + "GetEmployeeList/";

                _log.Debug("Retrieve method Url passed:" + Url);

                var Data = ApiCall.ReturnPost(Url, pagingInfo);

                List<EmployeeModel> ListOfEmployees = JsonConvert.DeserializeObject<List<EmployeeModel>>(Data["GetEmployeeListResult"].ToString());

                _log.Debug("Retrieve method data:" + ObjectSerializer.SerializeObject(ListOfEmployees));

                return Json(ListOfEmployees ?? new List<EmployeeModel>(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                _log.Error("Error in returning Data :" + e);

            }
            finally
            {

                _log.Info("ReturnEmployee Data stop");
            }

            return null;
        }

        //GET : /Employee
        public ActionResult Index()
        {
            _log.Info("Get Employee method start");
            try
            {
                return View();
            }
            catch (Exception e)
            {
                _log.Error("Get Employee method Error :" + e.Message);
            }
            finally
            {
                _log.Info("Get Employee method mandatory stop");
            }

            return View();
        }

        // GET : /Employee/CreateEmployee
        public ActionResult Create()
        {
            _log.Info("Create Employee method start");
            try
            {
                return View();
            }
            catch (Exception e)
            {
                _log.Error("Create Employee method Error :" + e.Message);
            }
            finally
            {
                _log.Info("Create Employee method mandatory stop");
            }

            return View();
        }

        // GET : /Employee/EditEmployee
        public ActionResult Edit(string id)
        {
            _log.Info("Get Edit Employee method start");

            try
            {
                var Url = EmpServiceURL + "GetSingleEmployee/";

                var createEmp = new
                {
                    EmployeeID = id
                };

                _log.Debug("Get Edit Employee URL is : " + Url);

                _log.Debug("Get Edit Employee method data passed : " + id);

                var Data = ApiCall.ReturnPost(Url, createEmp);

                EmployeeModel SingleEmployee = JsonConvert.DeserializeObject<EmployeeModel>(Data["GetSingleEmployeeResult"].ToString());

                _log.Debug("Get Edit Employee method data retrieved : " + ObjectSerializer.SerializeObject(SingleEmployee));

                return View(SingleEmployee);
            }
            catch (Exception ex)
            {

                _log.Error("Get Edit Employee method error, the error is : " + ex);

            }
            finally
            {

                _log.Info("Get Edit Employee method mandatory stop");

            }
            return View();
        }

        #endregion Get Methods

        #region Post Methods

        // POST : /Employee/CreateEmployee
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeModel employee)
        {
            _log.Info("Create Employee method post start");
            try
            {

                if (ModelState.IsValid)
                {

                    string Url = EmpServiceURL + "CreateEmployee/";

                    var RestData = new
                    {
                        employee = employee
                    };

                    _log.Debug("Create Employee Url is :" + Url);

                    _log.Debug("Create Employee method post data :" + ObjectSerializer.SerializeObject(employee));

                    var Data = ApiCall.ReturnPost(Url, RestData);

                    bool IsEmployeeCreated = Convert.ToBoolean(Data);

                    if (IsEmployeeCreated)
                    {
                        TempData["Success"] = "Employee created successfully!!";

                        _log.Debug("Create Employee method post return data :" + IsEmployeeCreated.ToString());

                        return View(employee);
                    }
                    else
                    {
                        _log.Debug("Create Employee method post return data :" + IsEmployeeCreated.ToString());

                        ModelState.AddModelError("", "An employee with the same email ID exists");
                    }
                }

            }
            catch (Exception ex)
            {

                _log.Error("Create Employee method error, the error is : " + ex.Message);

                ModelState.AddModelError("", "Unable to Create Employee.Try again, and if the problem persists, see your system administrator.");
            }
            finally
            {

                _log.Info("Create Employee post method mandatory stop");

            }

            return View();
        }

        // POST : /Employee/EditEmployee
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(EmployeeModel editedEmp)
        {

           _log.Info("Post Edit employee method start");

            try
            {

               _log.Debug("Post Edit employee method data : " + ObjectSerializer.SerializeObject(editedEmp));

                if (ModelState.IsValid)
                {

                    var Url = EmpServiceURL + "EditEmployee/";

                    var RestData = new
                    {
                        employee = editedEmp
                    };

                    var Data = ApiCall.ReturnPost(Url, RestData);  
            
                    bool IsEmployeeEdited = (bool)Data.SelectToken("EditEmployeeResult");

                    if (IsEmployeeEdited)
                    {
                        // _log.Info("Create Employee method stop successful, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary);
                        TempData["Success"] = "Employee Edited successfully!!";
                       _log.Debug("Post Edit employee method return data : " + IsEmployeeEdited);
                      
                        return View(editedEmp);
                    }
                    else
                    {
                       _log.Debug("Post Edit employee method return data : " + IsEmployeeEdited);
                     
                        //    _log.Info("Create Employee method stop unsuccessful, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary); ;
                        ModelState.AddModelError("", "Error in saving the details of employee,Duplicate Email ID");
                    }


                }
            }
            catch (Exception ex)
            {
               _log.Error("Error in editing the employee details, the error is : " + ex);

                ModelState.AddModelError("", "Unable to update details. Try again, and if the problem persists, see your system administrator.");
            }

            finally
            {            
               _log.Debug("Post Edit employee method stop");
            }

            return View(editedEmp);
        }

        // POST : /Employee/DeleteEmployee
        public ActionResult Delete(string id)
        {
            _log.Info("Deleting employee method start");
            try
            {

                string Url = EmpServiceURL + "DeleteEmployee/";

                var EmpObj = new
                {
                    EmpId = id
                };

                _log.Debug("Delete employee method url: " + Url);

                _log.Debug("Delete employee method id is " + id);

                var Data = ApiCall.ReturnPost(Url, EmpObj);

                bool IsEmployeeDeleted = (bool)Data.SelectToken("DeleteEmployeeResult");

                if (IsEmployeeDeleted)
                {

                    TempData["Delete"] = "Deleted the employee successfully!!";

                    _log.Debug("Delete employee return data is " + IsEmployeeDeleted);

                    return RedirectToAction("Index", "Employee");
                }
                else
                {

                    TempData["DeleteFail"] = "Couldn't Delete the Employee";

                    _log.Debug("Delete Employee method stop unsuccessful, return data is:" + IsEmployeeDeleted);

                    ModelState.AddModelError("", "Error in Deleting the Employee");
                }
            }
            catch (Exception ex)
            {

                _log.Error("Error in deleting the employee, the error is : " + ex.Message);

            }
            finally
            {

                _log.Info("Delete Employee method mandatory stop");

            }

            return RedirectToAction("Index", "Employee");
        }

        //Logout
        public ActionResult Logout()
        {
            _log.Info("Logout method start");

            try
            {

                if (Request.Cookies["formsCookie"] != null)
                {

                    var Cookie = new HttpCookie("formsCookie");

                    Cookie.Expires = DateTime.Now.AddDays(-1);

                    Response.Cookies.Add(Cookie);

                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                _log.Info("Logout method error:" + e.Message);
                return RedirectToAction("Index", "Home");
            }
            finally
            {

                _log.Info("Wrapper.Logout method stop");

            }

        }
        #endregion Post Methods
             
    }
}
