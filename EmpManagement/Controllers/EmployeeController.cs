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
           
            ViewBag.SearchString = searchString;
            log.Info("Employee Index method start");
            //Sorting parameters       
            EmployeeDetails info = new EmployeeDetails();

            info.SortField = "Name";
            info.SortDirection = "ascending";
            ViewBag.PagingInfo = info;      
            info.CurrentPageIndex = 0;
            List<EmployeeDetails> empList = getEmployeesInSets(info);

            IQueryable<EmployeeDetails> emp = empList.AsQueryable();
            var modEmplist = from s in emp
                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {

                int checkForNumber;
                DateTime checkForDate;
                bool resultOfDateParse = DateTime.TryParse(searchString, out checkForDate);
                bool isNumeric = int.TryParse(searchString, out checkForNumber);

                if (resultOfDateParse)
                {
                    modEmplist = modEmplist.Where(s => DateTime.Compare(s.DOB, checkForDate) == 0 || DateTime.Compare(s.DOJ, checkForDate) == 0);

                }
                else if (isNumeric)
                {
                    modEmplist = modEmplist.Where(s => s.salary == Int32.Parse(searchString) || s.contact.ToUpper().Contains(searchString.ToUpper()));
                }
                else
                {
                    modEmplist = modEmplist.Where(s => s.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || s.Address.ToUpper().Contains(searchString.ToUpper()) || s.contact.ToUpper().Equals(searchString.ToUpper()) || s.Dept.ToUpper().Equals(searchString.ToUpper()) || s.email.ToUpper().Contains(searchString.ToUpper()));
                }

            }
            switch (info.SortField)
            {
                case "Email":
                    modEmplist = modEmplist.OrderBy(s => s.email);
                    break;
                case "Name":
                    modEmplist = modEmplist.OrderBy(s => s.EmployeeName);
                    break;
                case "Address":
                    modEmplist = modEmplist.OrderBy(s => s.Address);
                    break;
                case "Department":
                    modEmplist = modEmplist.OrderBy(s => s.Dept);
                    break;
                case "DOJ":
                    modEmplist = modEmplist.OrderBy(s => s.DOJ);
                    break;
                case "DOB":
                    modEmplist = modEmplist.OrderBy(s => s.DOB);
                    break;
                case "Salary":
                    modEmplist = modEmplist.OrderBy(s => s.salary);
                    break;
                case "Contact":
                    modEmplist = modEmplist.OrderBy(s => s.contact);
                    break;
            }
            TempData["search"] = searchString;
            if (modEmplist != null)
            {
                if (modEmplist.Any())
                {
                    log.Info("Employee Index method stop");
                    return View(modEmplist);
                }
                else
                {
                    log.Info("Employee Index method stop");
                    ViewBag.EmptyResult = "There are no employees in this grid";
                    return View(modEmplist);
                }
               
            }
            else
            {
                log.Info("Employee Index method stop");
            
                return View();
            }
        }

        public List<EmployeeDetails> getEmployeesInSets(EmployeeDetails info)
        {

            long count = businessLayerObj.getPageCount();
            info.PageSize = 3;
            info.PageCount = Convert.ToInt32(Math.Ceiling((double)((count + info.PageSize - 1) / info.PageSize)));
            List<EmployeeDetails> empList = businessLayerObj.getEmployees(info.PageSize, info.CurrentPageIndex * info.PageSize);
            IQueryable<EmployeeDetails> emp = empList.AsQueryable();
            return empList;
        }

        [HttpPost]
        //POST : /Employee
        public ActionResult Index(EmployeeDetails info, string searchString)
        {
            ViewBag.PagingInfo = info;
            ViewBag.SearchString = TempData["search"];
            List<EmployeeDetails> empList = getEmployeesInSets(info);
            searchString = ViewBag.SearchString;

            IQueryable<EmployeeDetails> emp = empList.AsQueryable();

            var modEmplist = from s in emp
                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {
               
              int checkForNumber;
              DateTime checkForDate;
              bool resultOfDateParse = DateTime.TryParse(searchString, out checkForDate);
              bool isNumeric = int.TryParse(searchString, out checkForNumber);
              
              if (resultOfDateParse)
              {
                  modEmplist = modEmplist.Where(s => DateTime.Compare(s.DOB, checkForDate) == 0 || DateTime.Compare(s.DOJ, checkForDate) == 0);

              }
              else if (isNumeric)
              {
                  modEmplist = modEmplist.Where(s => s.salary == Int32.Parse(searchString) || s.contact.ToUpper().Contains(searchString.ToUpper()));
              }
              else
              {
                  modEmplist = modEmplist.Where(s => s.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || s.Address.ToUpper().Contains(searchString.ToUpper()) || s.contact.ToUpper().Equals(searchString.ToUpper()) || s.Dept.ToUpper().Equals(searchString.ToUpper()) || s.email.ToUpper().Contains(searchString.ToUpper()));
              }

            }
            switch (info.SortField)
            {
                case "Email":
                    modEmplist = modEmplist.OrderBy(s => s.email);
                    break;
                case "Name":
                    modEmplist = modEmplist.OrderBy(s => s.EmployeeName);
                    break;
                case "Address":
                    modEmplist = modEmplist.OrderBy(s => s.Address);
                    break;
                case "Department":
                    modEmplist = modEmplist.OrderBy(s => s.Dept);
                    break;
                case "DOJ":
                    modEmplist = modEmplist.OrderBy(s => s.DOJ);
                    break;
                case "DOB":
                    modEmplist = modEmplist.OrderBy(s => s.DOB);
                    break;
                case "Salary":
                    modEmplist = modEmplist.OrderBy(s => s.salary);
                    break;
                case "Contact":
                    modEmplist = modEmplist.OrderBy(s => s.contact);
                    break;
            }
            TempData["search"] = ViewBag.SearchString;
            if (modEmplist != null)
            {

                if (modEmplist.Any())
                {
                    log.Info("Employee Index method stop");
                    return View(modEmplist);
                }
                else
                {
                    log.Info("Employee Index method stop");
                    ViewBag.EmptyResult = "There are no employees in this grid";
                    return View(modEmplist);
                }
            }
            else
            {
                log.Info("Employee Index method stop");
                ViewBag.EmptyResult = "There is no matching employee.";
                return View();
            }


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
                        ModelState.AddModelError("", "An employee with the same employee ID exists");
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

        public ActionResult DeleteEmployee(Guid id)
        {
            try
            {
                log.Info("Deleting employee method called");
                EmployeeDetails singleEmp = businessLayerObj.getSingleEmployee(id);
                bool val = businessLayerObj.DeleteEmployee(singleEmp.EmployeeID);
                if (val)
                {
                    log.Info("Delete employee method stop,successful execution!!");
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
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
