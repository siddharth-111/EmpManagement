using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
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

        // GET: /Employee/
        
        
        string EmpServiceURL = ConfigurationManager.AppSettings["EmpServiceURL"];
        CommonGetPost ApiCall = new CommonGetPost();
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Get the Result of employee object
        public JsonResult ReturnEmployeeData(PaginationInfo pagingInfo)
        {
           
           _log.Info("ReturnEmployeeData method start");
            try
            {
               _log.Debug("ReturnEmployeeData method pagination info data:" + new JavaScriptSerializer().Serialize(pagingInfo));
                var Url = EmpServiceURL + "GetEmployeeList/";
                var Data = ApiCall.ReturnPost(Url, pagingInfo);
                List<EmpDetails> ListOfEmployees = new List<EmpDetails>();
                ListOfEmployees = JsonConvert.DeserializeObject<List<EmpDetails>>(Data["GetEmployeeListResult"].ToString());
               _log.Info("ReturnEmployeeData method stop");
                return Json(ListOfEmployees, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
               _log.Error("Error in returning Data :" + e);
            }
            finally
            {
               _log.Info("ReturnEmployee Data mandatory stop");
            
            }
            return null;
        }

        //GET : /Employee
        public ActionResult Index()
        {
           _log.Info("Get Employee method start");
            try
            {
               _log.Info("Get Employee method stop");
                return View();
            }
            catch (Exception e)
            {
               _log.Error("Get Employee method Error :" +e.Message);
            }
            finally 
            {
               _log.Info("Get Employee method mandatory stop");
            }
          
            return View();
        }


        // GET : /Employee/CreateEmployee
        public ActionResult CreateEmployee()
        {
           _log.Info("Create Employee method start");
            try
            {
               _log.Info("Create Employee method stop");
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

        // POST : /Employee/CreateEmployee
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmployee(EmpDetails newEmployee)
        {
           _log.Info("Create Employee method post start");
            try
            {
               _log.Debug("Create Employee method post data :" + new JavaScriptSerializer().Serialize(newEmployee));
                if (ModelState.IsValid)
                {
                    var Url = EmpServiceURL + "CreateEmployee/";
                    var RestData = new
                    {
                        employee = newEmployee
                    };
                    var Data = ApiCall.ReturnPost(Url, RestData);
                    bool IsEmployeeCreated = Convert.ToBoolean(Data);
                    if (IsEmployeeCreated)
                    {                       
                        TempData["Success"] = "Employee created successfully!!";
                       _log.Debug("Create Employee method post return data :" + IsEmployeeCreated.ToString());
                       _log.Info("Create Employee method stop");
                        return View(newEmployee);
                    }
                    else
                    {
                       _log.Debug("Create Employee method post return data :" + IsEmployeeCreated.ToString());
                       _log.Info("Create Employee method stop");                        
                        ModelState.AddModelError("", "An employee with the same email ID exists");
                    }
                }

            }
            catch (Exception ex)
            {                
               _log.Error("Create Employee method error, the error is : " + ex.Message);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            finally 
            {
               _log.Info("Create Employee post method mandatory stop");
            }

            return View();
        }


        // GET : /Employee/EditEmployee
        public ActionResult EditEmployee(Guid id)
        {
           _log.Info("Get Edit Employee method start");

            try
            {
               _log.Debug("Get Edit Employee method data passed : " + id.ToString());
                var Url = EmpServiceURL + "GetSingleEmployee/";
                var createEmp = new
                {
                    EmployeeID = id
                };
                var Data = ApiCall.ReturnPost(Url, createEmp);
                EmpDetails SingleEmployee = JsonConvert.DeserializeObject<EmpDetails>(Data["GetSingleEmployeeResult"].ToString());
               _log.Debug("Get Edit Employee method data retrieved : " + new JavaScriptSerializer().Serialize(SingleEmployee));
               _log.Info("Get Edit Employee method stop");
                return View(SingleEmployee);
            }
            catch (Exception ex)
            {
                //Wrapper.Log the error
               _log.Error("Get Edit Employee method error, the error is : " + ex);

            }
            finally
            {
               _log.Info("Get Edit Employee method mandatory stop");
            }
            return View();
        }

        // POST : /Employee/EditEmployee
        [HttpPost]
        public ActionResult EditEmployee(EmpDetails editedEmp)
        {
           _log.Info("Post Edit employee method start");
            try
            {
               _log.Debug("Post Edit employee method data : " + new JavaScriptSerializer().Serialize(editedEmp));
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
                       _log.Debug("Post Edit employee method stop");
                        return View(editedEmp);
                    }
                    else
                    {
                       _log.Debug("Post Edit employee method return data : " + IsEmployeeEdited);
                       _log.Debug("Post Edit employee method stop");
                        //    _log.Info("Create Employee method stop unsuccessful, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary); ;
                        ModelState.AddModelError("", "Error in saving the details of employee,Duplicate Email ID");
                    }


                }
            }
            catch (Exception ex)
            {
               _log.Error("Error in editing the employee details, the error is : " + ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            finally
            {            
               _log.Debug("Post Edit employee method mandatory stop");
            }
            return View(editedEmp);
        }

        // POST : /Employee/DeleteEmployee
        public ActionResult DeleteEmployee(Guid id)
        {
           _log.Info("Deleting employee method start");
            try
            {
               _log.Debug("Deleting employee data, the id is " + id);
                var Url = EmpServiceURL + "DeleteEmployee/";
                var EmpObj = new
                {
                    EmpId = id
                };
                var Data = ApiCall.ReturnPost(Url, EmpObj);
                bool IsEmployeeDeleted = (bool)Data.SelectToken("DeleteEmployeeResult");
                if (IsEmployeeDeleted)
                {
                    TempData["Delete"] = "Deleted the employee successfully!!";
                   _log.Debug("Deleting employee return data is " + IsEmployeeDeleted);
                   _log.Info("Deleting employee method stop");
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    TempData["DeleteFail"] = "Couldn't Delete the Employee";
                   _log.Debug("Delete Employee method stop unsuccessful, return data is:" + IsEmployeeDeleted);
                   _log.Info("Delete employee method stop");
                    ModelState.AddModelError("", "Error in Deleting the Employee");
                }
            }
            catch (Exception ex)
            {
               _log.Error("Error in deleting the employee, the error is : " + ex);
            }
            finally
            {
               _log.Info("Delete Employee method mandatory stop");
            }
            return RedirectToAction("Index", "Employee");
        }

        //Wrapper.Logout
        public ActionResult Logout()
        {
           _log.Info("Wrapper.Logout method start");
            if (Request.Cookies["formsCookie"] != null)
            {
                var Cookie = new HttpCookie("formsCookie");
                Cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Cookie);
            }
           _log.Info("Wrapper.Logout method stop");
            return RedirectToAction("Index", "Home");
        }

    }
}
