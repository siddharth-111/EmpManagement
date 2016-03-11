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

namespace EmpManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {

        // GET: /Employee/
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public List<EmpDetails> ListOfEmployees;
        string TemplateUrl = ConfigurationManager.AppSettings["EmpServiceURL"];
        //Get the Result of employee object
        public JsonResult ReturnEmployeeData(PaginationInfo pagingInfo)
        {
            log.Info("ReturnEmployeeData method start");
            try
            {

                var Url = TemplateUrl + "GetEmployeeList/";
                var WbRequest = (HttpWebRequest)WebRequest.Create(Url);
                WbRequest.ContentType = @"application/json";
                WbRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(WbRequest.GetRequestStream()))
                {
                    string Json = "{\"searchString\":\"" + pagingInfo.searchString + "\"," + "\"sortDirection\":\"" + pagingInfo.sortDirection + "\"," + "\"sortField\":\"" + pagingInfo.sortField + "\"," + "\"pageSize\":\"" + pagingInfo.pageSize + "\"," + "\"currPage\":\"" + pagingInfo.currPage + "\"}";
                    streamWriter.Write(Json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var WbResponse = (HttpWebResponse)WbRequest.GetResponse();
                using (var streamReader = new StreamReader(WbResponse.GetResponseStream()))
                {
                    JavaScriptSerializer Ser = new JavaScriptSerializer();
                    var Result = streamReader.ReadToEnd();
                    var Data = JsonConvert.DeserializeObject(Result) as JToken;
                    ListOfEmployees = JsonConvert.DeserializeObject<List<EmpDetails>>(Data["GetEmployeeListResult"].ToString());

                    return Json(ListOfEmployees, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                log.Error("Error in returning Data :" + e);
            }
            return null;
        }

        //GET : /Employee
        public ActionResult Index()
        {
            log.Info("Get Employee method start");
            log.Info("Get Employee method stop");
            return View();
        }


        // GET : /Employee/CreateEmployee
        public ActionResult CreateEmployee()
        {
            log.Info("Get CreateEmployee method start");
            log.Info("Get CreateEmployee method stop");
            return View();
        }

        // POST : /Employee/CreateEmployee
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmployee(EmpDetails newEmployee)
        {
           // log.Info("Create Employee method start, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary);
            try
            {
                if (ModelState.IsValid)
                {
                    var Url = TemplateUrl + "CreateEmployee/";
                    var WbRequest = (HttpWebRequest)WebRequest.Create(Url);
                    WbRequest.ContentType = @"application/json";
                    WbRequest.Method = "POST";
                    using (var streamWriter = new StreamWriter(WbRequest.GetRequestStream()))
                    {
                        JObject createEmp = (JObject)JToken.FromObject(newEmployee);

                        string Json = JsonConvert.SerializeObject(createEmp);
                        streamWriter.Write(Json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var WbResponse = (HttpWebResponse)WbRequest.GetResponse();
                    using (var streamReader = new StreamReader(WbResponse.GetResponseStream()))
                    {
                        JavaScriptSerializer Ser = new JavaScriptSerializer();
                        var Result = streamReader.ReadToEnd();
                        var JsonResult = JObject.Parse(Result);
                        bool IsEmployeeCreated = (bool)JsonResult.SelectToken("CreateEmployeeResult");
                        if (IsEmployeeCreated)
                        {
                       //     log.Info("Create Employee method stop successful, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary);
                            TempData["Success"] = "Employee created successfully!!";
                            return View(newEmployee);
                        }
                        else
                        {
                     //       log.Info("Create Employee method stop unsuccessful, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary); ;
                            ModelState.AddModelError("", "An employee with the same email ID exists");
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //Log the error
                log.Error("Create Employee method error, the error is : " + ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View();
        }


        // GET : /Employee/EditEmployee
        public ActionResult EditEmployee(Guid id)
        {
            log.Info("Get Edit Employee method start, the employee id is :" + id);
            
            try
            {

                var Url = TemplateUrl + "GetSingleEmployee/";
                var WbRequest = (HttpWebRequest)WebRequest.Create(Url);
                WbRequest.ContentType = @"application/json";
                WbRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(WbRequest.GetRequestStream()))
                {
                    var createEmp = new
                    {
                        EmployeeID = id
                    };

                    string Json = JsonConvert.SerializeObject(createEmp);
                    streamWriter.Write(Json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var WbResponse = (HttpWebResponse)WbRequest.GetResponse();
                using (var streamReader = new StreamReader(WbResponse.GetResponseStream()))
                {
                    JavaScriptSerializer Ser = new JavaScriptSerializer();
                    var Result = streamReader.ReadToEnd();
                    var Data = JsonConvert.DeserializeObject(Result) as JToken;
                    EmpDetails SingleEmployee = JsonConvert.DeserializeObject<EmpDetails>(Data["GetSingleEmployeeResult"].ToString());
                    return View(SingleEmployee);
                }
            }
            catch (Exception ex)
            {
                //Log the error
                log.Error("Create Employee method error, the error is : " + ex);

            }
            return View();
        }

        // POST : /Employee/EditEmployee
        [HttpPost]
        public ActionResult EditEmployee(EmpDetails editedEmp)
        {
            log.Info("Edit employee method start");
            try
            {
                if (ModelState.IsValid)
                {
                    var Url = TemplateUrl + "EditEmployee/";
                    var WbRequest = (HttpWebRequest)WebRequest.Create(Url);
                    WbRequest.ContentType = @"application/json";
                    WbRequest.Method = "POST";
                    using (var streamWriter = new StreamWriter(WbRequest.GetRequestStream()))
                    {
                        JObject createEmp = (JObject)JToken.FromObject(editedEmp);
                        string Json = JsonConvert.SerializeObject(createEmp);
                        streamWriter.Write(Json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var WbResponse = (HttpWebResponse)WbRequest.GetResponse();
                    using (var streamReader = new StreamReader(WbResponse.GetResponseStream()))
                    {
                        JavaScriptSerializer Ser = new JavaScriptSerializer();
                        var Result = streamReader.ReadToEnd();
                        var JsonResult = JObject.Parse(Result);
                        bool IsEmployeeCreated = (bool)JsonResult.SelectToken("EditEmployeeResult");
                        if (IsEmployeeCreated)
                        {
                            //  log.Info("Create Employee method stop successful, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary);
                            TempData["Success"] = "Employee created successfully!!";
                            return View(editedEmp);
                        }
                        else
                        {
                            //     log.Info("Create Employee method stop unsuccessful, The Employee details are--> Username:" + newEmployee.EmployeeName + " Email:" + newEmployee.email + " DOB:" + newEmployee.DOB + " DOJ:" + newEmployee.DOJ + " Address:" + newEmployee.Address + " Salary:" + newEmployee.salary); ;
                            ModelState.AddModelError("", "An employee with the same email ID exists");
                        }

                    }

                }
            }
            catch (Exception ex/* dex */)
            {
                log.Error("Error in editing the employee details, the error is : " + ex);
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(editedEmp);
        }

        // POST : /Employee/DeleteEmployee
        public ActionResult DeleteEmployee(Guid id)
        {
            try
            {
                log.Info("Deleting employee method called, the id is " + id);
                var Url = TemplateUrl + "DeleteEmployee/";
                var WbRequest = (HttpWebRequest)WebRequest.Create(Url);
                WbRequest.ContentType = @"application/json";
                WbRequest.Method = "POST";


                using (var streamWriter = new StreamWriter(WbRequest.GetRequestStream()))
                {
                    var EmpObj = new
                    {
                        EmpId = id
                    };
                    string Json = JsonConvert.SerializeObject(EmpObj);
                    streamWriter.Write(Json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var WbResponse = (HttpWebResponse)WbRequest.GetResponse();
                using (var streamReader = new StreamReader(WbResponse.GetResponseStream()))
                {
                    JavaScriptSerializer Ser = new JavaScriptSerializer();
                    var Result = streamReader.ReadToEnd();
                    var JsonResult = JObject.Parse(Result);
                    bool IsEmployeeDeleted = (bool)JsonResult.SelectToken("DeleteEmployeeResult");
                    if (IsEmployeeDeleted)
                    {
                        TempData["Delete"] = "Deleted the employee successfully!!";
                        log.Info("Delete Employee method stop successful, the employee id is : " + id);
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        TempData["DeleteFail"] = "Couldn't Delete the Employee";
                        log.Info("Delete Employee method stop unsuccessful, The Employee id is :" + id);
                        ModelState.AddModelError("", "Error in Deleting the Employee");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in deleting the employee, the error is : " + ex);
            }
            return RedirectToAction("Index", "Employee");
        }

        //Logout
        public ActionResult Logout()
        {
            log.Info("Logout method start");
            if (Request.Cookies["formsCookie"] != null)
            {
                var Cookie = new HttpCookie("formsCookie");
                Cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Cookie);
            }
            log.Info("Logout method stop");
            return RedirectToAction("Index", "Home");
        }

    }
}
