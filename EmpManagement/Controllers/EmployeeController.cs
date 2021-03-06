﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeManagement.Models;
using System.Data;
using System.Dynamic;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Web.Http.WebHost;
using System.Configuration;
using CommonUtility;
using log4net;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
       
        #region Fields

        string EmpServiceURL = ConfigurationManager.AppSettings["EmpServiceURL"];      

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #endregion Fields

        #region Get Methods 

        //Get the Result of employee object
        public JsonResult Retrieve(CriteriaModel pagingInfo)
        {

            _log.Info("ReturnEmployeeData method start");

            try
            {
                _log.Debug("Retrieve method CriteriaModel info data:" + Log.SerializeObject(pagingInfo));

                string Url = EmpServiceURL + "GetEmployeeList/";

                _log.Debug("Retrieve method Url passed:" + Url);

                var Data = CommonGetPost.Post(Url, pagingInfo);

                List<EmployeeModel> ListOfEmployees = JsonConvert.DeserializeObject<List<EmployeeModel>>(Data["RetrieveResult"].ToString());

                _log.Debug("Retrieve method data:" + Log.SerializeObject(ListOfEmployees));

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
            _log.Info("Index method start");
            try
            {
                return View();
            }
            catch (Exception e)
            {
                _log.Error("Index method Error :" + e.Message);
            }
            finally
            {
                _log.Info("Index method mandatory stop");
            }

            return View();
        }

        // GET : /Employee/CreateEmployee
        public ActionResult Create()
        {
            _log.Info("Create method start");
            try
            {
                return View();
            }
            catch (Exception e)
            {
                _log.Error("Create method Error :" + e.Message);
            }
            finally
            {
                _log.Info("Create method mandatory stop");
            }

            return View();
        }

        // GET : /Employee/EditEmployee
        public ActionResult Edit(string id)
        {
            _log.Info("Get Edit method start");

            try
            {
                var Url = EmpServiceURL + "GetSingleEmployee/";

                var createEmp = new
                {
                    EmployeeID = id
                };

                _log.Debug("Get Edit URL is : " + Url);

                _log.Debug("Get Edit method data passed : " + id);

                var Data = CommonGetPost.Post(Url, createEmp);

                EmployeeModel SingleEmployee = JsonConvert.DeserializeObject<EmployeeModel>(Data["RetrieveByIdResult"].ToString());

                _log.Debug("Get Edit method data retrieved : " + Log.SerializeObject(SingleEmployee));

                return View(SingleEmployee);
            }
            catch (Exception ex)
            {

                _log.Error("Get Edit method error, the error is : " + ex);

            }
            finally
            {

                _log.Info("Get Edit method mandatory stop");

            }
            return View();
        }

        #endregion Get Methods

        #region Post Methods

        // POST : /Employee/CreateEmployee
        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeModel employee)
        {
            _log.Info("Create method post start");
            try
            {

                if (ModelState.IsValid)
                {

                    string Url = EmpServiceURL + "CreateEmployee/";

                    var RestData = new
                    {
                        employee = employee
                    };

                    _log.Debug("Create Url is :" + Url);

                    _log.Debug("Create method post data :" + Log.SerializeObject(employee));

                    var Data = CommonGetPost.Post(Url, RestData);

                    bool IsEmployeeCreated = Convert.ToBoolean(Data);

                    if (IsEmployeeCreated)
                    {
                        TempData["Success"] = "Employee created successfully!!";

                        _log.Debug("Create method post return data :" + IsEmployeeCreated.ToString());

                        return View(employee);
                    }
                    else
                    {
                        _log.Debug("Create method post return data :" + IsEmployeeCreated.ToString());

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

               _log.Debug("Post Edit method data : " + Log.SerializeObject(editedEmp));

                if (ModelState.IsValid)
                {

                    var Url = EmpServiceURL + "EditEmployee/";

                    var RestData = new
                    {
                        employee = editedEmp
                    };

                    var Data = CommonGetPost.Post(Url, RestData);  
            
                    bool IsEmployeeEdited = (bool)Data.SelectToken("EditResult");

                    if (IsEmployeeEdited)
                    {
                        // _log.Info("Create Employee method stop successful, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary);
                        TempData["Success"] = "Employee Edited successfully!!";
                       _log.Debug("Post Edit method return data : " + IsEmployeeEdited);
                      
                        return View(editedEmp);
                    }
                    else
                    {
                       _log.Debug("Post Edit method return data : " + IsEmployeeEdited);
                     
                        //    _log.Info("Create Employee method stop unsuccessful, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary); ;
                        ModelState.AddModelError("", "An employee with the same email address exists");
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

        // POST : /Employee/Delete
        public JsonResult Delete(string id)
        {
            _log.Info("Delete method start");
            try
            {

                string Url = EmpServiceURL + "DeleteEmployee/";

                var EmpObj = new
                {
                    EmpId = id
                };

                _log.Debug("Delete method url: " + Url);

                _log.Debug("Delete method id is " + id);

                var Data = CommonGetPost.Post(Url, EmpObj);

                bool IsEmployeeDeleted = (bool)Data.SelectToken("DeleteResult");

                if (IsEmployeeDeleted)
                {              

                    _log.Debug("Delete return data is " + IsEmployeeDeleted);

                   return Json(IsEmployeeDeleted, JsonRequestBehavior.AllowGet);
                }
                else
                {             
                    _log.Debug("Delete Employee unsuccessful, return data is:" + IsEmployeeDeleted);

                    return Json(IsEmployeeDeleted, JsonRequestBehavior.AllowGet);                 
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet); 

                _log.Error("Error in deleting the employee, the error is : " + ex.Message);

            }
            finally
            {

                _log.Info("Delete method stop");

            }
      
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
