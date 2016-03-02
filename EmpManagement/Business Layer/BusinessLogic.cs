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
            bool registeredUser = dataLayerObj.saveData("~/App_Data/Users.txt", dataOfUser.ToString());
            return registeredUser;
        }

        //Get the entire list of employees
        public List<EmployeeDetails> getEmployees(int pageSize, int currentPageIndex)
        {

            List<EmployeeDetails> empList = getAllEmployees(pageSize, currentPageIndex);
            if (empList != null)
            {
                return empList;
            }
            return null;
        }

        //Common method to get data from files to be used by getEmployees() and EditSingleEmployee()
        public List<EmployeeDetails> getAllEmployees(int pageSize, int currentPageIndex)
        {
            List<EmployeeDetails> empList = new List<EmployeeDetails>();
            string[] empData = dataLayerObj.getEmpData("~/App_Data/Employees.txt", pageSize, currentPageIndex);
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
                return empList;
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
            bool ret = dataLayerObj.saveData("~/App_Data/Employees.txt", dataOfEmp.ToString());
            return ret;
        }
    }
}