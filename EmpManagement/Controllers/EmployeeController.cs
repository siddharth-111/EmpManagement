using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmpManagement.Business_Layer;
using EmpManagement.Models;
using PagedList;
using PagedList.Mvc;

namespace EmpManagement.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/
        BusinessLogic getEmployeeList = new BusinessLogic();
        public ActionResult Index(string sortOrder, string searchString,int? page,string currentFilter)
        {
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
                return View(modEmplist.ToPagedList(pageNumber, pageSize));
            }else{ 
                return View(); 
            }
        }

        public ActionResult CreateEmployee()
        {
            
            return View();
        }

        public ActionResult EditUser(EmployeeDetails editedEmp) {
            
            bool val = getEmployeeList.EditSingleEmployee(editedEmp);
            if (val)
                return RedirectToAction("Index", "Employee");
            else
                return null;
        }

        public ActionResult SaveUser(EmployeeDetails newEmployee)
        {
            bool check = getEmployeeList.saveUser(newEmployee);

            return RedirectToAction("Index", "Employee");
        }

        public ActionResult EditEmployee(int id)
        {
            EmployeeDetails singleEmp = getEmployeeList.getSingleEmployee(id.ToString());            
            return View(singleEmp);
        }

        
        public ActionResult DeleteEmployee(int id)
        {
            EmployeeDetails singleEmp = getEmployeeList.getSingleEmployee(id.ToString());
            return View(singleEmp);
    
           // return RedirectToAction("Index", "Employee");
        }

        public ActionResult DeleteUser(EmployeeDetails emp)
        {
            bool val = getEmployeeList.DeleteEmployee(Int32.Parse(emp.EmployeeID));
            return RedirectToAction("Index", "Employee");
        }

    }
}
