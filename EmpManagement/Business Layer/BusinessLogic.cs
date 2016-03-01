using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmpManagement.Models;
using EmpManagement.Data_Access_Layer;

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
            string[] userData = dataLayerObj.getData("~/App_Data/data.txt");
            List<LoginDetails> userList = new List<LoginDetails>();
            if (userData != null)
            {
                foreach (string dataLine in userData)
                {
                    string[] dataItem = dataLine.Split(',');

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
            string dataOfEmp = newEmployee.EmployeeID.ToString() + "," + newEmployee.email + "," + newEmployee.EmployeeName + "," + newEmployee.Address + "," + ((newEmployee.Dept != null) ? newEmployee.Dept : "") + "," + newEmployee.DOJ.ToString() + "," + newEmployee.DOB.ToString() + "," + (newEmployee.contact != null ? newEmployee.contact.ToString() : "") + "," + newEmployee.salary.ToString();
            bool ret = dataLayerObj.updateData("~/App_Data/Employees.txt", dataOfEmp, newEmployee.EmployeeID.ToString());
            return ret;
        }

        //Register new user
        public bool Register(RegisterDetails register)
        {
            List<LoginDetails> userList = getUserList();
            foreach (LoginDetails user in userList)
            { 
                    if(user.username.Equals(register.username))
                     return false;
            }
            string dataOfUser = register.username + "," + register.password+","+ ((register.name != null) ? register.name : "") + ","+((register.phone !=null) ? register.phone : "");
            bool registeredUser = dataLayerObj.saveData("~/App_Data/data.txt",dataOfUser);
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
                    string[] dataItem = dataLine.Split(',');

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

                string[] dataItem = empData.Split(',');

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
            string dataOfEmp = guid + "," + newEmployee.email + "," + newEmployee.EmployeeName + "," + newEmployee.Address + "," + ((newEmployee.Dept != null) ? newEmployee.Dept : "") + "," + newEmployee.DOJ.ToString() + "," + newEmployee.DOB.ToString() + "," + (newEmployee.contact != null ? newEmployee.contact.ToString() : "") + "," + newEmployee.salary.ToString();
            bool ret = dataLayerObj.saveData("~/App_Data/Employees.txt", dataOfEmp);
            return ret;
        }
    }
}