﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmpManagement.Business_Layer;
using EmpManagement.Models;
using PagedList;
using PagedList.Mvc;
using System.Data;

namespace EmpManagement.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
  (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BusinessLogic getEmployeeList = new BusinessLogic();
        public ActionResult Index(string sortOrder, string searchString,int? page,string currentFilter)
        {
            log.Info("Employee Index method start");
            //Sorting parameters
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name" : "";
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "ID" : "";
            ViewBag.AddressSortParm = String.IsNullOrEmpty(sortOrder) ? "Address" : "";
            ViewBag.DOBSortParm = String.IsNullOrEmpty(sortOrder) ? "DOB" : "";
            ViewBag.SalarySortParm = String.IsNullOrEmpty(sortOrder) ? "Salary" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            List<EmployeeDetails> empList = getEmployeeList.getEmployees();
            IQueryable<EmployeeDetails> emp = empList.AsQueryable();
            
            var modEmplist = from s in emp
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                modEmplist = modEmplist.Where(s => s.EmployeeName.ToUpper().Contains(searchString.ToUpper()));
                return View(modEmplist.ToPagedList(pageNumber, pageSize));
            }
            switch (sortOrder)
            {
                case "Name":
                    modEmplist = modEmplist.OrderBy(s => s.EmployeeName);
                  
                    break;
                case "ID":
                    modEmplist = modEmplist.OrderBy(s => s.EmployeeID);
                    break;
                case "Address":
                    modEmplist = modEmplist.OrderBy(s => s.Address);
                    break;
                case "DOB" :
                    modEmplist = modEmplist.OrderBy(s => s.DOB);
                    break;
                case "Salary" :
                    modEmplist = modEmplist.OrderBy(s => s.salary);
                    break;
            }
            
            if (modEmplist!= null){
                log.Info("Employee Index method stop");
                return View(modEmplist.ToPagedList(pageNumber, pageSize));
            }else{
                log.Info("Employee Index method stop,the List of employees is empty");
                return View(); 
            }
        }

        public ActionResult CreateEmployee()
        { 
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmployee(EmployeeDetails newEmployee)
        {
            log.Info("Create Employee method start");
            try
            {
                if (ModelState.IsValid)
                {
                    bool check = getEmployeeList.saveUser(newEmployee);

                    if (check)
                    {
                        log.Info("Create Employee method stop,successfully created employee");
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        log.Info("Create Employee method stop,creating employee unsuccessful");
                        return RedirectToAction("Index", "Employee");
                    }
                      
                }
            }
            catch (DataException ex/* dex */)
            {
                log.Error("Create Employee method error, the error is : " + ex);
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View();
        }

        [HttpPost]
        public ActionResult EditEmployee(EmployeeDetails editedEmp) {
            log.Info("Edit employee method start");
            try
            {
                if (ModelState.IsValid)
                {
                    bool val = getEmployeeList.EditSingleEmployee(editedEmp);
                    if (val)
                    {
                        log.Info("Edit employee method stop,successful");
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
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
    

        

        public ActionResult EditEmployee(int id)
        {
            EmployeeDetails singleEmp = getEmployeeList.getSingleEmployee(id);            
            return View(singleEmp);
        }

        
        
        public ActionResult DeleteEmployee(int id)
        {
            EmployeeDetails singleEmp = getEmployeeList.getSingleEmployee(id);
            return View(singleEmp);
        }

        public ActionResult DeleteUser(EmployeeDetails emp)
        {
            try
            {
                log.Info("Deleting employee method called");
                bool val = getEmployeeList.DeleteEmployee(emp.EmployeeID);
                if (val)
                {
                    log.Info("Delete employee method stop,successful execution!!");
                    return RedirectToAction("Index", "Employee");
                }
                else {
                    log.Info("Delete employee method stop,unsuccessful execution");
                    return RedirectToAction("Index", "Employee");
                }
             
            }
            catch (Exception ex) {
                log.Error("Error in deleting the employee, the error is : " + ex);
            }
            return RedirectToAction("Index", "Employee");
           
        }

    }
}
