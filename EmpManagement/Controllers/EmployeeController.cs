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

namespace EmpManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {

        // GET: /Employee/
        
        
        string EmpServiceURL = ConfigurationManager.AppSettings["EmpServiceURL"];
        CommonGetPost ApiCall = new CommonGetPost();
        Logger Wrapper = new Logger();
        

        //Get the Result of employee object
        public JsonResult ReturnEmployeeData(PaginationInfo pagingInfo)
        {
           
            Wrapper.Log.Info("ReturnEmployeeData method start");
            try
            {
                Wrapper.Log.Debug("ReturnEmployeeData method pagination info data:" + new JavaScriptSerializer().Serialize(pagingInfo));
                var Url = EmpServiceURL + "GetEmployeeList/";
                var Data = ApiCall.ReturnPost(Url, pagingInfo);
                List<EmpDetails> ListOfEmployees = new List<EmpDetails>();
                ListOfEmployees = JsonConvert.DeserializeObject<List<EmpDetails>>(Data["GetEmployeeListResult"].ToString());
                Wrapper.Log.Info("ReturnEmployeeData method stop");
                return Json(ListOfEmployees, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Wrapper.Log.Error("Error in returning Data :" + e);
            }
            finally
            {
                Wrapper.Log.Info("ReturnEmployee Data mandatory stop");
            
            }
            return null;
        }

        //GET : /Employee
        public ActionResult Index()
        {
            Wrapper.Log.Info("Get Employee method start");
            try
            {
                Wrapper.Log.Info("Get Employee method stop");
                return View();
            }
            catch (Exception e)
            {
                Wrapper.Log.Error("Get Employee method Error :" +e.Message);
            }
            finally 
            {
                Wrapper.Log.Info("Get Employee method mandatory stop");
            }
          
            return View();
        }


        // GET : /Employee/CreateEmployee
        public ActionResult CreateEmployee()
        {
            Wrapper.Log.Info("Create Employee method start");
            try
            {
                Wrapper.Log.Info("Create Employee method stop");
                return View();
            }
            catch (Exception e)
            {
                Wrapper.Log.Error("Create Employee method Error :" + e.Message);
            }
            finally
            {
                Wrapper.Log.Info("Create Employee method mandatory stop");
            }

            return View();
        }

        // POST : /Employee/CreateEmployee
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmployee(EmpDetails newEmployee)
        {
            Wrapper.Log.Info("Create Employee method post start");
            try
            {
                Wrapper.Log.Debug("Create Employee method post data :" + new JavaScriptSerializer().Serialize(newEmployee));
                if (ModelState.IsValid)
                {
                    var Url = EmpServiceURL + "CreateEmployee/";
                    var Data = ApiCall.ReturnPost(Url, newEmployee);
                    bool IsEmployeeCreated = (bool)Data.SelectToken("CreateEmployeeResult");
                    if (IsEmployeeCreated)
                    {
                        //     Wrapper.Log.Info("Create Employee method stop successful, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary);
                        TempData["Success"] = "Employee created successfully!!";
                        Wrapper.Log.Debug("Create Employee method post return data :" + IsEmployeeCreated.ToString());
                        Wrapper.Log.Info("Create Employee method stop");
                        return View(newEmployee);
                    }
                    else
                    {
                        Wrapper.Log.Debug("Create Employee method post return data :" + IsEmployeeCreated.ToString());
                        Wrapper.Log.Info("Create Employee method stop");
                        //       Wrapper.Log.Info("Create Employee method stop unsuccessful, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary); ;
                        ModelState.AddModelError("", "An employee with the same email ID exists");
                    }
                }

            }
            catch (Exception ex)
            {
                //Wrapper.Log the error
                Wrapper.Log.Error("Create Employee method error, the error is : " + ex.Message);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            finally 
            {
                Wrapper.Log.Info("Create Employee post method mandatory stop");
            }

            return View();
        }


        // GET : /Employee/EditEmployee
        public ActionResult EditEmployee(Guid id)
        {
            Wrapper.Log.Info("Get Edit Employee method start");

            try
            {
                Wrapper.Log.Debug("Get Edit Employee method data passed : " + id.ToString());
                var Url = EmpServiceURL + "GetSingleEmployee/";
                var createEmp = new
                {
                    EmployeeID = id
                };
                var Data = ApiCall.ReturnPost(Url, createEmp);
                EmpDetails SingleEmployee = JsonConvert.DeserializeObject<EmpDetails>(Data["GetSingleEmployeeResult"].ToString());
                Wrapper.Log.Debug("Get Edit Employee method data retrieved : " + new JavaScriptSerializer().Serialize(SingleEmployee));
                Wrapper.Log.Info("Get Edit Employee method stop");
                return View(SingleEmployee);
            }
            catch (Exception ex)
            {
                //Wrapper.Log the error
                Wrapper.Log.Error("Get Edit Employee method error, the error is : " + ex);

            }
            finally
            {
                Wrapper.Log.Info("Get Edit Employee method mandatory stop");
            }
            return View();
        }

        // POST : /Employee/EditEmployee
        [HttpPost]
        public ActionResult EditEmployee(EmpDetails editedEmp)
        {
            Wrapper.Log.Info("Post Edit employee method start");
            try
            {
                Wrapper.Log.Debug("Post Edit employee method data : " + new JavaScriptSerializer().Serialize(editedEmp));
                if (ModelState.IsValid)
                {
                    var Url = EmpServiceURL + "EditEmployee/";
                    var Data = ApiCall.ReturnPost(Url, editedEmp);

                    bool IsEmployeeEdited = (bool)Data.SelectToken("EditEmployeeResult");
                    if (IsEmployeeEdited)
                    {
                        //  Wrapper.Log.Info("Create Employee method stop successful, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary);
                        TempData["Success"] = "Employee Edited successfully!!";
                        Wrapper.Log.Debug("Post Edit employee method return data : " + IsEmployeeEdited);
                        Wrapper.Log.Debug("Post Edit employee method stop");
                        return View(editedEmp);
                    }
                    else
                    {
                        Wrapper.Log.Debug("Post Edit employee method return data : " + IsEmployeeEdited);
                        Wrapper.Log.Debug("Post Edit employee method stop");
                        //     Wrapper.Log.Info("Create Employee method stop unsuccessful, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary); ;
                        ModelState.AddModelError("", "Error in saving the details of employee");
                    }


                }
            }
            catch (Exception ex)
            {
                Wrapper.Log.Error("Error in editing the employee details, the error is : " + ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            finally
            {            
                Wrapper.Log.Debug("Post Edit employee method mandatory stop");
            }
            return View(editedEmp);
        }

        // POST : /Employee/DeleteEmployee
        public ActionResult DeleteEmployee(Guid id)
        {
            Wrapper.Log.Info("Deleting employee method start");
            try
            {
                Wrapper.Log.Debug("Deleting employee data, the id is " + id);
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
                    Wrapper.Log.Debug("Deleting employee return data is " + IsEmployeeDeleted);
                    Wrapper.Log.Info("Deleting employee method stop");
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    TempData["DeleteFail"] = "Couldn't Delete the Employee";
                    Wrapper.Log.Debug("Delete Employee method stop unsuccessful, return data is:" + IsEmployeeDeleted);
                    Wrapper.Log.Info("Delete employee method stop");
                    ModelState.AddModelError("", "Error in Deleting the Employee");
                }
            }
            catch (Exception ex)
            {
                Wrapper.Log.Error("Error in deleting the employee, the error is : " + ex);
            }
            finally
            {
                Wrapper.Log.Info("Delete Employee method mandatory stop");
            }
            return RedirectToAction("Index", "Employee");
        }

        //Wrapper.Logout
        public ActionResult Logout()
        {
            Wrapper.Log.Info("Wrapper.Logout method start");
            if (Request.Cookies["formsCookie"] != null)
            {
                var Cookie = new HttpCookie("formsCookie");
                Cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Cookie);
            }
            Wrapper.Log.Info("Wrapper.Logout method stop");
            return RedirectToAction("Index", "Home");
        }

    }
}
