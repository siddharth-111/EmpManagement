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

        public bool DeleteEmployee(Guid id)
        {
            bool val = dataLayerObj.deleteData(id.ToString(), "~/App_Data/Employees.txt");
            return val;
        }

        public bool EditSingleEmployee(EmployeeDetails empObject)
        {
            string dataOfEmp = empObject.EmployeeID.ToString() + "," + empObject.EmployeeName + "," + empObject.Address + "," + empObject.DOB + "," + empObject.salary.ToString();
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
            if (empData != null )
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
                        DOB= DateTime.Parse(dataItem[6]),
                        contact = Int32.Parse(dataItem[7]),
                        salary = Int32.Parse(dataItem[8])
                    });
                }
                return empList;
            }
            return null;
        }

        public EmployeeDetails getSingleEmployee(Guid id)
        {
            List<EmployeeDetails> empList = getAllEmployees();
            foreach (EmployeeDetails emp in empList)
            {
                if (emp.EmployeeID == id)
                    return emp;
            }
            return null;

        }

        public bool saveUser(InsertViewModel newEmployee)
        {
            string guid = Guid.NewGuid().ToString();    
            string dataOfEmp = guid + ","+newEmployee.email+","+ newEmployee.EmployeeName + "," + newEmployee.Address + "," +newEmployee.Dept +","+newEmployee.DOJ.ToString()+","+newEmployee.DOB.ToString() + ","+newEmployee.contact.ToString()+"," + newEmployee.salary.ToString();
            bool ret = dataLayerObj.saveData("~/App_Data/Employees.txt", dataOfEmp);
            return ret;
        }
    }
}