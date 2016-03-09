using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using EmpManagement.Models;
using System.Data;
using System.Dynamic;

namespace EmpManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
      
        // GET: /Employee/
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BusinessLogic BusinessLayerObj  = new BusinessLogic();


        //Get the Result of employee object
        public JsonResult ReturnEmployeeData(PaginationInfo pagingInfo)
        {
            log.Info("ReturnEmployeeData method start");
            var ReturnList = new List<dynamic>();
            ReturnList = BusinessLayerObj.GetAllEmployees(pagingInfo.searchString,pagingInfo.sortDirection,pagingInfo.sortField,pagingInfo.pageSize,pagingInfo.currPage);
            log.Info("ReturnEmployeeData method stop");
            return Json(ReturnList, JsonRequestBehavior.AllowGet);
        }

        //GET : /Employee
        public ActionResult Index() {
            log.Info("Get Employee method start");
            return View();
        }


        // GET : /Employee/CreateEmployee
        public ActionResult CreateEmployee()
        {
            log.Info("Get CreateEmployee method start");
            return View();
        }

        // POST : /Employee/CreateEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmployee(EmpDetails newEmployee)
        {
            log.Info("Create Employee method start");
            try
            {
                if (ModelState.IsValid)
                {
                    dynamic NewEmp = newEmployee;
                    bool Check = BusinessLayerObj.SaveUser(newEmployee);

                    if (Check)
                    {
                        log.Info("Create Employee method stop,successfully created employee, the employee data is :" + newEmployee);
                        TempData["Success"] = "Employee created successfully!!";
                        return View(newEmployee);
                    }
                    else
                    {
                        log.Error("Create Employee method stop,creating employee unsuccessful");
                        ModelState.AddModelError("", "An employee with the same email ID exists");
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
            log.Info("Get Edit Employee method start");
            dynamic DynamicData = new ExpandoObject();
            DynamicData = BusinessLayerObj.GetSingleEmployee(id);

            EmpDetails SingleEmp = new EmpDetails
                        {
                            EmployeeID = DynamicData.GetType().GetProperty("EmployeeID").GetValue(DynamicData, null),
                            email = DynamicData.GetType().GetProperty("email").GetValue(DynamicData, null),
                            EmployeeName = DynamicData.GetType().GetProperty("EmployeeName").GetValue(DynamicData, null),
                            Address = DynamicData.GetType().GetProperty("Address").GetValue(DynamicData, null),
                            Dept = DynamicData.GetType().GetProperty("Dept").GetValue(DynamicData, null),
                            DOJ = DynamicData.GetType().GetProperty("DOJ").GetValue(DynamicData, null),
                            DOB = DynamicData.GetType().GetProperty("DOB").GetValue(DynamicData, null),
                            contact = DynamicData.GetType().GetProperty("contact").GetValue(DynamicData, null),
                            salary = DynamicData.GetType().GetProperty("salary").GetValue(DynamicData, null)
                        };
            log.Info("Edit Employee method start, the data to  Get EditEmployee is :" + SingleEmp); 
            return View(SingleEmp);
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
                    dynamic EditEmp = editedEmp;
                    bool val = BusinessLayerObj.EditSingleEmployee(EditEmp);
                    if (val)
                    {
                        log.Info("Edit employee method stop,successful, the data is :"+EditEmp);
                        TempData["Success"] = "Employee edited successfully!!";
                        return View(editedEmp);
                    }
                    else
                    {
                        TempData["Fail"] = "Employee edit failed";
                        log.Info("Edit employee method stop,unsuccessful, the data is : "+EditEmp);
                        return View(editedEmp);
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
                log.Info("Deleting employee method called");
                var DynamicData = BusinessLayerObj.GetSingleEmployee(id);
                EmpDetails SingleEmp = new EmpDetails {
                    EmployeeID = DynamicData.GetType().GetProperty("EmployeeID").GetValue(DynamicData, null),
                };
                bool Val = BusinessLayerObj .DeleteEmployee(SingleEmp.EmployeeID);
                if (Val)
                {
                    TempData["Delete"] = "Deleted the employee successfully!!";
                    log.Info("Delete employee method stop,successful execution!,the deleted Employee is :" +SingleEmp);
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    TempData["DeleteFail"] = "Couldn't Delete the Employee";
                    log.Info("Delete employee method stop,unsuccessful execution,the employee data is :" + SingleEmp);
                    ModelState.AddModelError("", "Error in Deleting the Employee");
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
