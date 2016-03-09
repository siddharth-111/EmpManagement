using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmpManagement.Models;
using EmpManagement.Data_Access_Layer;
using System.Text;

namespace EmpManagement.Business_Layer
{
    public class BusinessLogic
    {
        DBAccess dataLayerObj = new DBAccess();
        //Check if the Login user is valid
        public bool isUserValid(LoginDetails login)
        {
            List<LoginDetails> newList = getUserList();
            foreach (LoginDetails list in newList)
            {
                if (list.username == login.username && list.password == login.password)
                    return true;
            }
            return false;
        }

        //Get the list of users from file
        public List<LoginDetails> getUserList()
        {
            string[] userData = dataLayerObj.getData("~/App_Data/Users.txt");
            List<LoginDetails> userList = new List<LoginDetails>();
            if (userData != null)
            {
                foreach (string dataLine in userData)
                {
                    string[] dataItem = dataLine.Split('|');

                    userList.Add(new LoginDetails
                    {
                        username = dataItem[0],
                        password = dataItem[1]
                    });
                }
                return userList;
            }
            return null;
        }

        //Return the page count
        public int getPageCount()
        {
            int pageCount = dataLayerObj.getPageCount("~/App_Data/Employees.txt");
            return pageCount;
        }

        //Delete an employee from the file
        public bool DeleteEmployee(Guid id)
        {
            bool val = dataLayerObj.deleteData(id.ToString(), "~/App_Data/Employees.txt");
            return val;
        }

        //Update single employee data
        public bool EditSingleEmployee(EmployeeDetails newEmployee)
        {
            StringBuilder dataOfEmp = new StringBuilder();
            dataOfEmp.Append(newEmployee.EmployeeID.ToString()).Append("|").Append(newEmployee.email).Append("|").Append(newEmployee.EmployeeName).Append("|").Append(newEmployee.Address).Append("|").Append((newEmployee.Dept != null) ? newEmployee.Dept : "").Append("|").Append(newEmployee.DOJ.ToString()).Append("|").Append(newEmployee.DOB.ToString()).Append("|").Append(newEmployee.contact != null ? newEmployee.contact.ToString() : "").Append("|").Append(newEmployee.salary.ToString());
            bool ret = dataLayerObj.updateData("~/App_Data/Employees.txt", dataOfEmp.ToString(), newEmployee.EmployeeID.ToString());
            return ret;
        }

        //Register new user
        public bool Register(RegisterDetails register)
        {
            List<LoginDetails> userList = getUserList();
            foreach (LoginDetails user in userList)
            {
                if (user.username.Equals(register.username))
                    return false;
            }
            StringBuilder dataOfUser = new StringBuilder();
            dataOfUser.Append(register.username).Append("|").Append(register.password).Append("|").Append((register.name != null) ? register.name : "").Append("|").Append((register.phone != null) ? register.phone : "");
            bool registeredUser = dataLayerObj.saveData("~/App_Data/Users.txt", dataOfUser.ToString() , register.username.ToString());
            return registeredUser;
        }

        //Get the entire list of employees
        public List<dynamic> getEmployees(PaginationInfo pagingInfo)
        {

            List<dynamic> empList = getAllEmployees(pagingInfo);
            if (empList != null)
            {
                return empList;
            }
            return null;
        }

        //Common method to get data from files to be used by getEmployees() and EditSingleEmployee()
        public List<dynamic> getAllEmployees(PaginationInfo pagingInfo)
        {
            List<EmployeeDetails> empList = new List<EmployeeDetails>();
            string[] empData = dataLayerObj.getEmpData("~/App_Data/Employees.txt", pagingInfo);
            if (empData != null)
            {
                foreach (string dataLine in empData)
                {
                    string[] dataItem = dataLine.Split('|');

                    empList.Add(new EmployeeDetails
                    {
                        EmployeeID = Guid.Parse(dataItem[0]),
                        email = dataItem[1],
                        EmployeeName = dataItem[2],
                        Address = dataItem[3],
                        Dept = dataItem[4],
                        DOJ = DateTime.Parse(dataItem[5]),
                        DOB = DateTime.Parse(dataItem[6]),
                        contact = dataItem[7],
                        salary = Int32.Parse(dataItem[8])
                    });
                }
                IQueryable<EmployeeDetails> emp = empList.AsQueryable();
                    var modEmplist = from s in emp
                                    select s;
                    if (!String.IsNullOrEmpty(pagingInfo.searchString))
                    {

                        int checkForNumber;
                        DateTime checkForDate;
                        bool resultOfDateParse = DateTime.TryParse(pagingInfo.searchString, out checkForDate);
                        bool isNumeric = int.TryParse(pagingInfo.searchString, out checkForNumber);

                        if (resultOfDateParse)
                        {
                            modEmplist = modEmplist.Where(s => DateTime.Compare(s.DOB, checkForDate) == 0 || DateTime.Compare(s.DOJ, checkForDate) == 0);

                        }
                        else if (isNumeric)
                        {
                            modEmplist = modEmplist.Where(s => s.salary == Int32.Parse(pagingInfo.searchString) || s.contact.ToUpper().Contains(pagingInfo.searchString.ToUpper()));
                        }
                        else
                        {
                            modEmplist = modEmplist.Where(s => s.EmployeeName.ToUpper().Contains(pagingInfo.searchString.ToUpper()) || s.Address.ToUpper().Contains(pagingInfo.searchString.ToUpper()) || s.contact.ToUpper().Equals(pagingInfo.searchString.ToUpper()) || s.Dept.ToUpper().Equals(pagingInfo.searchString.ToUpper()) || s.email.ToUpper().Contains(pagingInfo.searchString.ToUpper()));
                        }

                    }
                if (pagingInfo.sortDirection == "ascending")
                {
                    switch (pagingInfo.sortField)
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
                }
                else
                {
                    switch (pagingInfo.sortField)
                    {
                        case "Email":
                            modEmplist = modEmplist.OrderByDescending(s => s.email);
                            break;
                        case "Name":
                            modEmplist = modEmplist.OrderByDescending(s => s.EmployeeName);
                            break;
                        case "Address":
                            modEmplist = modEmplist.OrderByDescending(s => s.Address);
                            break;
                        case "Department":
                            modEmplist = modEmplist.OrderByDescending(s => s.Dept);
                            break;
                        case "DOJ":
                            modEmplist = modEmplist.OrderByDescending(s => s.DOJ);
                            break;
                        case "DOB":
                            modEmplist = modEmplist.OrderByDescending(s => s.DOB);
                            break;
                        case "Salary":
                            modEmplist = modEmplist.OrderByDescending(s => s.salary);
                            break;
                        case "Contact":
                            modEmplist = modEmplist.OrderByDescending(s => s.contact);
                            break;
                    }
                }
                var returnList = new List<dynamic>();
                var count = modEmplist.Count();
                var PageCount = Convert.ToInt32(Math.Ceiling((double)((count + pagingInfo.pageSize - 1) / pagingInfo.pageSize)));
                var newEmpList = modEmplist.ToList().Skip(pagingInfo.currPage * pagingInfo.pageSize).Take(pagingInfo.pageSize) ;
                foreach (EmployeeDetails temp in newEmpList)
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
                returnList.Add(new
                {
                    Pagecount = PageCount,
                    TotalRecords = count
                });
                return returnList;
            }
            return null;
        }

        //Get individual employee data
        public EmployeeDetails getSingleEmployee(Guid id)
        {
            string empData = dataLayerObj.getSingleEmpData(id.ToString(), "~/App_Data/Employees.txt");
            if (empData != null)
            {

                string[] dataItem = empData.Split('|');

                return new EmployeeDetails
                     {
                         EmployeeID = Guid.Parse(dataItem[0]),
                         email = dataItem[1],
                         EmployeeName = dataItem[2],
                         Address = dataItem[3],
                         Dept = dataItem[4],
                         DOJ = DateTime.Parse(dataItem[5]),
                         DOB = DateTime.Parse(dataItem[6]),
                         contact = dataItem[7],
                         salary = Int32.Parse(dataItem[8])
                     };
            }
            return null;
        }

        //Registering a new user
        public bool saveUser(InsertViewModel newEmployee)
        {
            string guid = Guid.NewGuid().ToString();
            StringBuilder dataOfEmp = new StringBuilder();
            dataOfEmp.Append(guid).Append("|").Append(newEmployee.email).Append("|").Append(newEmployee.EmployeeName).Append("|").Append(newEmployee.Address).Append("|").Append((newEmployee.Dept != null) ? newEmployee.Dept : "").Append("|").Append(newEmployee.DOJ.ToString()).Append("|").Append(newEmployee.DOB.ToString()).Append("|").Append(newEmployee.contact != null ? newEmployee.contact.ToString() : "").Append("|").Append(newEmployee.salary.ToString());
            bool ret = dataLayerObj.saveData("~/App_Data/Employees.txt", dataOfEmp.ToString() , newEmployee.email.ToString());
            return ret;
        }
    }
}