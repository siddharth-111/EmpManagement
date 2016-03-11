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
          //  log.Info("Business Layer IsUserValid start, The data--> Username:" + login.username+",Password:"+login.password);
            List<dynamic> NewList = GetUserList();
            foreach (dynamic List in NewList)
            {
                if (List.username == login.GetType().GetProperty("username").GetValue(login, null) && List.password == login.GetType().GetProperty("password").GetValue(login, null))
                {
                   // log.Info("Business Layer IsUserValid start, The data--> Username:" + login.username + ",Password:" + login.password);
                    return true;
                }

            }
          //  log.Info("Business Layer IsUserValid stop,the user is not valid, The Userdata is :" + login);
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
            try {

                dynamic TempEmp = new
                {
                    EmployeeID = newEmployee.GetType().GetProperty("EmployeeID").GetValue(newEmployee, null),
                    Email = newEmployee.GetType().GetProperty("Email").GetValue(newEmployee, null),
                    EmployeeName = newEmployee.GetType().GetProperty("EmployeeName").GetValue(newEmployee, null),
                    Address = newEmployee.GetType().GetProperty("Address").GetValue(newEmployee, null),
                    Dept = newEmployee.GetType().GetProperty("Dept").GetValue(newEmployee, null),
                    DOJ = (newEmployee.GetType().GetProperty("DOJ").GetValue(newEmployee, null)),
                    DOB = (newEmployee.GetType().GetProperty("DOB").GetValue(newEmployee, null)),
                    Contact = newEmployee.GetType().GetProperty("Contact").GetValue(newEmployee, null),
                    Salary = newEmployee.GetType().GetProperty("Salary").GetValue(newEmployee, null)

                };
                DataOfEmp.Append(TempEmp.EmployeeID.ToString()).Append("|").Append(TempEmp.Email).Append("|").Append(TempEmp.EmployeeName).Append("|").Append(TempEmp.Address).Append("|").Append((TempEmp.Dept != null) ? TempEmp.Dept : "").Append("|").Append(TempEmp.DOJ.ToString()).Append("|").Append(TempEmp.DOB.ToString()).Append("|").Append(TempEmp.Contact != null ? TempEmp.Contact.ToString() : "").Append("|").Append(TempEmp.Salary.ToString());
                bool IsEdited = DataLayerObj.UpdateData(DataOfEmp.ToString(), TempEmp.EmployeeID.ToString());
                log.Info("Business Layer EditSingleEmployee Stop,The data is :" + TempEmp);
                return IsEdited;
            }
            catch (Exception e) { 
            
            
            }
            return false;
         
        }

        //Register new user
        public bool RegisterUser(dynamic register)
        {
            log.Info("Business Layer Register User Start,The data is :" + register);
            List<dynamic> UserList = GetUserList();
            try
            {
                foreach (dynamic User in UserList)
                {
                    if (User.username.Equals(register.GetType().GetProperty("username").GetValue(register, null)))
                        return false;
                }
                StringBuilder DataOfUser = new StringBuilder();
                dynamic TempRegister = new
                {
                    Username = register.GetType().GetProperty("username").GetValue(register, null),
                    Password = register.GetType().GetProperty("password").GetValue(register, null),
                    Contact = register.GetType().GetProperty("contact").GetValue(register, null),
                    Name = register.GetType().GetProperty("name").GetValue(register, null),

                };
                DataOfUser.Append(TempRegister.Username).Append("|").Append(TempRegister.Password).Append("|").Append((TempRegister.Name != null) ? TempRegister.Name : "").Append("|").Append((TempRegister.Contact != null) ? TempRegister.Contact : "");
                bool RegisteredUser = DataLayerObj.SaveData(DataOfUser.ToString(), TempRegister.Username.ToString(),"User.txt");
                log.Info("Business Layer Register User Stop,is user registered? :" + RegisteredUser);
                return RegisteredUser;
            }
            catch (Exception e) {
                log.Error("Error in registering the user, the error is :" + e.Message);
            }
            return false;
          
        }

        //Get the entire list of employees        
        public List<dynamic> GetAllEmployees(string searchString, string sortDirection, string sortField, int pageSize, int currPage)
        {
            try {
                //log.Info("Business Layer GetAllEmployees Called,The data is : searchstring:" + searchString + ",sortDirection:" + sortDirection + ",sortField:" + sortField + ",pageSize:" + pageSize + ",currPage:" + currPage);
                List<dynamic> EmpData = DataLayerObj.GetEmployeeData(searchString, sortDirection, sortField, pageSize, currPage);
                if (EmpData != null)
                {
                    // log.Info("Business Layer GetAllEmployees Stop,The Employee Data is :" + EmpData);
                    return EmpData;
                }
            }
            catch (Exception e) {

                log.Error("Error in returning employees :" + e);
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
                    Email = dataItem[1],
                    EmployeeName = dataItem[2],
                    Address = dataItem[3],
                    Dept = dataItem[4],
                    DOJ = DateTime.Parse(dataItem[5]).Date.ToString("d"),
                    DOB = DateTime.Parse(dataItem[6]).Date.ToString("d"),
                    Contact = dataItem[7],
                    Salary = Int32.Parse(dataItem[8])
                };
                log.Info("Business Layer GetSingleEmployee Stop,The Employee data is :" + SingleData);
                return SingleData;
            }

            return null;
        }

        //Registering a new user
        public bool SaveUser(dynamic newEmployee)
        {
            try {
                log.Info("Business Layer SaveUser Start,The Employee data is :" + newEmployee);
                string GuidEmp = Guid.NewGuid().ToString();
                StringBuilder DataOfEmp = new StringBuilder();
             
                dynamic TempEmp = new
                {
                    EmployeeID = newEmployee.GetType().GetProperty("EmployeeID").GetValue(newEmployee, null),
                    Email = newEmployee.GetType().GetProperty("Email").GetValue(newEmployee, null),
                    EmployeeName = newEmployee.GetType().GetProperty("EmployeeName").GetValue(newEmployee, null),
                    Address = newEmployee.GetType().GetProperty("Address").GetValue(newEmployee, null),
                    Dept = newEmployee.GetType().GetProperty("Dept").GetValue(newEmployee, null),
                    DOJ = (newEmployee.GetType().GetProperty("DOJ").GetValue(newEmployee, null)),
                    DOB = (newEmployee.GetType().GetProperty("DOB").GetValue(newEmployee, null)),
                    Contact = newEmployee.GetType().GetProperty("Contact").GetValue(newEmployee, null),
                    Salary = newEmployee.GetType().GetProperty("Salary").GetValue(newEmployee, null)

                };
                DataOfEmp.Append(GuidEmp).Append("|").Append(TempEmp.Email).Append("|").Append(TempEmp.EmployeeName).Append("|").Append(TempEmp.Address).Append("|").Append((TempEmp.Dept != null) ? TempEmp.Dept : "").Append("|").Append(TempEmp.DOJ.ToString()).Append("|").Append(TempEmp.DOB.ToString()).Append("|").Append(TempEmp.Contact != null ? TempEmp.Contact.ToString() : "").Append("|").Append(TempEmp.Salary.ToString());
                bool ret = DataLayerObj.SaveData(DataOfEmp.ToString(), TempEmp.Email.ToString(),"Employee.txt");
                log.Info("Business Layer SaveUser Stop,Is User saved? :" + ret);
                return ret;
            
            }
            catch (Exception e) {
                log.Error("Business Layer SaveUser Stop err,The error is is :" + e.Message);
            }
            return false;
            
        }
    }
}
