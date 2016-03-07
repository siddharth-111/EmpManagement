using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmpManagement.Business_Layer;
using EmpManagement.Models;
using System.Data;

namespace EmpManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
  (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BusinessLogic businessLayerObj = new BusinessLogic();
        public ActionResult Index(string currentFilter, string searchString)
        {
            log.Info("Employee Index method stop");
            return View();
        }

        //Get the Result of employee object
        public JsonResult ReturnEmp(PaginationInfo pagingInfo)
        {
            long count = businessLayerObj.getPageCount();
            var PageCount = Convert.ToInt32(Math.Ceiling((double)((count + pagingInfo.pageSize - 1) / pagingInfo.pageSize)));
            List<EmployeeDetails> empList = businessLayerObj.getEmployees(pagingInfo.pageSize, pagingInfo.currPage * pagingInfo.pageSize);
            var returnList = new List<dynamic>();
            foreach (EmployeeDetails temp in empList)
            {
                returnList.Add(
                    new
                    {
                        EmployeeID = temp.EmployeeID,
                        email = temp.email,
                        EmployeeName = temp.EmployeeName,
                        Address = temp.Address,
                        DOB = temp.DOB.Date.ToString("d"),
                        DOJ = temp.DOJ.Date.ToString("d"),
                        Dept = temp.Dept,
                        salary = temp.salary,
                        contact = temp.contact,

                    }
                    );
            }
            returnList.Add(new { Pagecount = PageCount ,
                            TotalRecords = count
            });

            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        // GET : /Employee/CreateEmployee
        public ActionResult CreateEmployee()
        {
            return View();
        }

        // POST : /Employee/CreateEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmployee(InsertViewModel newEmployee)
        {
            log.Info("Create Employee method start");
            try
            {
                if (ModelState.IsValid)
                {
                    bool check = businessLayerObj.saveUser(newEmployee);

                    if (check)
                    {
                        log.Info("Create Employee method stop,successfully created employee");
                        TempData["Success"] = "Employee created successfully!!";
                        return View(newEmployee);
                    }
                    else
                    {
                        log.Info("Create Employee method stop,creating employee unsuccessful");
                        ModelState.AddModelError("", "An employee with the same email ID exists");
                    }

                }
            }
            catch (DataException ex)
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
            EmployeeDetails singleEmp = businessLayerObj.getSingleEmployee(id);
            return View(singleEmp);
        }

        // POST : /Employee/EditEmployee
        [HttpPost]
        public ActionResult EditEmployee(EmployeeDetails editedEmp)
        {
            log.Info("Edit employee method start");
            try
            {
                if (ModelState.IsValid)
                {
                    bool val = businessLayerObj.EditSingleEmployee(editedEmp);
                    if (val)
                    {
                        log.Info("Edit employee method stop,successful");
                        TempData["Success"] = "Employee edited successfully!!";
                        return View(editedEmp);
                    }
                    else
                    {
                        TempData["Fail"] = "Employee edit failed";
                        log.Info("Edit employee method stop,unsuccessful");
                        return View(editedEmp);
                    }
                }
            }
            catch (DataException ex/* dex */)
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
                EmployeeDetails singleEmp = businessLayerObj.getSingleEmployee(id);
                bool val = businessLayerObj.DeleteEmployee(singleEmp.EmployeeID);
                if (val)
                {
                    TempData["Delete"] = "Deleted the employee successfully!!";
                    log.Info("Delete employee method stop,successful execution!!");
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    TempData["DeleteFail"] = "Couldn't Delete the Employee";
                    log.Info("Delete employee method stop,unsuccessful execution");
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
            if (Request.Cookies["formsCookie"] != null)
            {
                var c = new HttpCookie("formsCookie");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
