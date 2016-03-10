using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace BLL
{

    public class BusinessLogic
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Check if the Login user is valid
        DataLayer.FileAccess DataLayerObj = new DataLayer.FileAccess();
        public bool IsUserValid(dynamic login)
        {
            log.Info("Business Layer IsUserValid start, The data--> Username:" + login.username+",Password:"+login.password);
            List<dynamic> NewList = GetUserList();
            foreach (dynamic List in NewList)
            {
                if (List.username == login.GetType().GetProperty("username").GetValue(login, null) && List.password == login.GetType().GetProperty("password").GetValue(login, null))
                {
                    log.Info("Business Layer IsUserValid start, The data--> Username:" + login.username + ",Password:" + login.password);
                    return true;
                }

            }
            log.Info("Business Layer IsUserValid stop,the user is not valid, The Userdata is :" + login);
            return false;
        }

        //Get the list of users from file
        public List<dynamic> GetUserList()
        {
            string[] UserData = DataLayerObj.GetUserData();
            log.Info("Business Layer GetUserData start");
            List<dynamic> UserList = new List<dynamic>();
            if (UserData != null)
            {
                foreach (string dataLine in UserData)
                {
                    string[] dataItem = dataLine.Split('|');

                    UserList.Add(new
                    {
                        username = dataItem[0],
                        password = dataItem[1]
                    });
                }
                log.Info("Business Layer GetUserData Stop");
                return UserList;
            }
            return null;
        }

        //Delete Employee 
        public bool DeleteEmployee(Guid id)
        {
            log.Info("Business Layer DeletEmployee Start");
            bool Val = DataLayerObj.DeleteData(id.ToString());
            log.Info("Business Layer DeleteEmployee Stop,is User Deleted? :" + Val);
            return Val;
        }

        //Update single employee data
        public bool EditSingleEmployee(dynamic newEmployee)
        {
            log.Info("Business Layer EditSingleEmployee Start,The data is :" + newEmployee);
            StringBuilder DataOfEmp = new StringBuilder();
            DataOfEmp.Append(newEmployee.EmployeeID.ToString()).Append("|").Append(newEmployee.email).Append("|").Append(newEmployee.EmployeeName).Append("|").Append(newEmployee.Address).Append("|").Append((newEmployee.Dept != null) ? newEmployee.Dept : "").Append("|").Append(newEmployee.DOJ.ToString()).Append("|").Append(newEmployee.DOB.ToString()).Append("|").Append(newEmployee.contact != null ? newEmployee.contact.ToString() : "").Append("|").Append(newEmployee.salary.ToString());
            bool IsEdited = DataLayerObj.UpdateData(DataOfEmp.ToString(), newEmployee.EmployeeID.ToString());
            log.Info("Business Layer EditSingleEmployee Stop,The data is :" + newEmployee);
            return IsEdited;
        }

        //Register new user
        public bool RegisterUser(dynamic register)
        {
            log.Info("Business Layer Register User Start,The data is :" + register);
            List<dynamic> UserList = GetUserList();
            foreach (dynamic User in UserList)
            {
                if (User.username.Equals(register.username))
                    return false;
            }
            StringBuilder DataOfUser = new StringBuilder();
            DataOfUser.Append(register.username).Append("|").Append(register.password).Append("|").Append((register.name != null) ? register.name : "").Append("|").Append((register.phone != null) ? register.phone : "");
            bool RegisteredUser = DataLayerObj.SaveData(DataOfUser.ToString(), register.username.ToString());
            log.Info("Business Layer Register User Stop,is user registered? :" + RegisteredUser);
            return RegisteredUser;
        }

        //Get the entire list of employees        
        public List<dynamic> GetAllEmployees(string searchString, string sortDirection, string sortField, int pageSize, int currPage)
        {
            log.Info("Business Layer GetAllEmployees Called,The data is : searchstring:" + searchString + ",sortDirection:" + sortDirection + ",sortField:" + sortField + ",pageSize:" + pageSize + ",currPage:" + currPage);
            List<dynamic> EmpData = DataLayerObj.GetEmployeeData(searchString, sortDirection, sortField, pageSize, currPage);
            if (EmpData != null)
            {
                log.Info("Business Layer GetAllEmployees Stop,The Employee Data is :" + EmpData);
                return EmpData;
            }
            return null;
        }

        //Get individual employee data
        public dynamic GetSingleEmployee(Guid id)
        {
            log.Info("Business Layer GetSingleEmployee Start, the employee Id is :" + id);
            string EmpData = DataLayerObj.GetSingleEmpData(id.ToString());
            dynamic SingleData = new ExpandoObject();
            if (EmpData != null)
            {

                string[] dataItem = EmpData.Split('|');
                SingleData = new
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
                log.Info("Business Layer GetSingleEmployee Stop,The Employee data is :" + SingleData);
                return SingleData;
            }

            return null;
        }

        //Registering a new user
        public bool SaveUser(dynamic newEmployee)
        {
            log.Info("Business Layer SaveUser Start,The Employee data is :" + newEmployee);
            string GuidEmp = Guid.NewGuid().ToString();
            StringBuilder DataOfEmp = new StringBuilder();
            DataOfEmp.Append(GuidEmp).Append("|").Append(newEmployee.email).Append("|").Append(newEmployee.EmployeeName).Append("|").Append(newEmployee.Address).Append("|").Append((newEmployee.Dept != null) ? newEmployee.Dept : "").Append("|").Append(newEmployee.DOJ.ToString()).Append("|").Append(newEmployee.DOB.ToString()).Append("|").Append(newEmployee.contact != null ? newEmployee.contact.ToString() : "").Append("|").Append(newEmployee.salary.ToString());
            bool ret = DataLayerObj.SaveData(DataOfEmp.ToString(), newEmployee.email.ToString());
            log.Info("Business Layer SaveUser Stop,Is User saved? :" + newEmployee);
            return ret;
        }
    }
}
