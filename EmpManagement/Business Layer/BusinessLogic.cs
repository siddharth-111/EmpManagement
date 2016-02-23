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

        public List<LoginDetails> getUserList()
        {
            string[] userData = dataLayerObj.getData("~/App_Data/data.txt");
            List<LoginDetails> userList = new List<LoginDetails>();
            if (userData != null)
            {
                foreach (string dataLine in userData)
                {
                    string[] dataItem = dataLine.Split(' ');

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

        public bool DeleteEmployee(int id)
        {
            bool val = dataLayerObj.deleteData(id.ToString(), "~/App_Data/Employees.txt");
            return val;
        }

        public bool EditSingleEmployee(EmployeeDetails empObject)
        {
            string dataOfEmp = empObject.EmployeeID.ToString() + " " + empObject.EmployeeName + " " + empObject.Address + " " + empObject.DOB + " " + empObject.salary.ToString();
            bool ret = dataLayerObj.updateData("~/App_Data/Employees.txt", dataOfEmp, empObject.EmployeeID.ToString());
            return ret;
        }

        public List<EmployeeDetails> getEmployees()
        {

            List<EmployeeDetails> empList = getAllEmployees();
            if (empList != null)
            {
                return empList;
            }
            return null;
        }
        public List<EmployeeDetails> getAllEmployees()
        {
            List<EmployeeDetails> empList = new List<EmployeeDetails>();
            string[] empData = dataLayerObj.getData("~/App_Data/Employees.txt");
            if (empData != null)
            {
                foreach (string dataLine in empData)
                {
                    string[] dataItem = dataLine.Split(' ');

                    empList.Add(new EmployeeDetails
                    {
                        EmployeeID = Int32.Parse(dataItem[0]),
                        EmployeeName = dataItem[1],
                        Address = dataItem[2],
                        DOB = dataItem[3],
                        salary = Int32.Parse(dataItem[4])
                    });
                }
                return empList;
            }
            return null;
        }

        public EmployeeDetails getSingleEmployee(int id)
        {
            List<EmployeeDetails> empList = getAllEmployees();
            foreach (EmployeeDetails emp in empList)
            {
                if (emp.EmployeeID == id)
                    return emp;
            }
            return null;

        }

        public bool saveUser(EmployeeDetails newEmployee)
        {
            List<EmployeeDetails> empList = getAllEmployees();
            foreach (EmployeeDetails emp in empList)
            {
                if (emp.EmployeeID == newEmployee.EmployeeID)
                    return false;
            }
            string dataOfEmp = newEmployee.EmployeeID.ToString() + " " + newEmployee.EmployeeName + " " + newEmployee.Address + " " + newEmployee.DOB + " " + newEmployee.salary.ToString();
            bool ret = dataLayerObj.saveData("~/App_Data/Employees.txt", dataOfEmp);
            return ret;
        }
    }
}