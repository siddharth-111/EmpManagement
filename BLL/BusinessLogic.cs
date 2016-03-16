using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using DataObject;
using DataLayer;
using System.Web.Script.Serialization;
using CommonUtility;
using log4net;
using System.Reflection;
namespace BusinessLayer
{

    public class BusinessLogic
    {
      //  private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Check if the Login user is valid
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //FileAccess DataLayerObj = new FileAccess();
        SQLRetrieve SqlLayerObj = new SQLRetrieve();
        Serializer ObjectSerializer = new Serializer();    

        public bool IsUserValid(UserObject login)
        {
            _log.Info("Business Layer IsUserValid method start ");
            try
            {
                _log.Debug("Business Layer IsUserValid Login Details : " + ObjectSerializer.SerializeObject(login));
                List<UserObject> NewList = GetSQLUserList();
                foreach (UserObject List in NewList)
                {
                    if (List.Email ==login.Email && List.Password == login.Password)
                    {
                        return true;
                    }

                }
                return false;
            }
            catch (Exception e) {
                _log.Error("Business Layer IsUserValid exception,the exception is :" + e.Message);
                return false;
            }
            finally {
                _log.Info("Business Layer IsUserValid method stop");            
            }
          
        }

        //Get SQL User list
        public List<UserObject> GetSQLUserList() {
            _log.Info("Business Layer Get SQL User list method start:");
            try
            {
                List<UserObject> UserData = SqlLayerObj.GetUserData();
                _log.Debug("The User data from SQL proc is :" + ObjectSerializer.SerializeObject(UserData));
                List<UserObject> UserList = new List<UserObject>();
                if (UserData != null)
                {
                    foreach (UserObject SingleUser in UserData)
                    {
                        UserList.Add(new UserObject
                        {
                            Email = SingleUser.Email,
                            Password = SingleUser.Password
                        });
                    }
                    
                    return UserList;
                }

            }
            catch (Exception e)
            {
                _log.Error("The Error in retrieving data from Business Layer Get SQL User List,The error is :" + e.Message);
                   
                return null;
            }
            finally
            {
                _log.Info("Business Layer Get SQL User list method stop");
            }

            return null;
            
        }


        //Get the list of users from file
        //public List<dynamic> GetUserList()
        //{
        //    _log.Info("Business Layer GetUserData start");
        //    try {
        //        List<UserObject> UserL = SqlLayerObj.GetUserData();
        //        string[] UserData = DataLayerObj.GetUserData();
        //        List<dynamic> UserList = new List<dynamic>();
        //        if (UserData != null)
        //        {
        //            foreach (string dataLine in UserData)
        //            {
        //                string[] dataItem = dataLine.Split('|');

        //                UserList.Add(new
        //                {
        //                    username = dataItem[0],
        //                    password = dataItem[1]
        //                });
        //            }
        //            _log.Debug("Business Layer GetUserData :" + ObjectSerializer.SerializeObject(UserList));
        //            return UserList;
        //        }
                
        //    }
        //    catch (Exception e) {
        //        _log.Error("Business Layer GetUserData Exception : " + e.Message);
        //    }
        //    finally {
        //        _log.Info("Business Layer GetUserData mandatory stop :");
        //    }
           
        //    return null;
        //}

        //Delete Employee 
        public bool DeleteEmployee(Guid id)
        {
            _log.Info("Business Layer DeletEmployee Start");
            bool Val = SqlLayerObj.DeleteData(id.ToString());
            _log.Info("Business Layer DeleteEmployee Stop,is User Deleted? :" + Val);
            _log.Info("Business Layer DeleteEmployee Stop");
            return Val;
        }

        ////Update single employee data
        //public bool EditSingleEmployee(dynamic newEmployee)
        //{
        //    log.Info("Business Layer EditSingleEmployee Start,The data is :" + newEmployee);
        //    StringBuilder DataOfEmp = new StringBuilder();
        //    try {

        //        dynamic TempEmp = new
        //        {
        //            EmployeeID = newEmployee.GetType().GetProperty("EmployeeID").GetValue(newEmployee, null),
        //            Email = newEmployee.GetType().GetProperty("Email").GetValue(newEmployee, null),
        //            EmployeeName = newEmployee.GetType().GetProperty("EmployeeName").GetValue(newEmployee, null),
        //            Address = newEmployee.GetType().GetProperty("Address").GetValue(newEmployee, null),
        //            Dept = newEmployee.GetType().GetProperty("Dept").GetValue(newEmployee, null),
        //            DOJ = (newEmployee.GetType().GetProperty("DOJ").GetValue(newEmployee, null)),
        //            DOB = (newEmployee.GetType().GetProperty("DOB").GetValue(newEmployee, null)),
        //            Contact = newEmployee.GetType().GetProperty("Contact").GetValue(newEmployee, null),
        //            Salary = newEmployee.GetType().GetProperty("Salary").GetValue(newEmployee, null)

        //        };
        //        DataOfEmp.Append(TempEmp.EmployeeID.ToString()).Append("|").Append(TempEmp.Email).Append("|").Append(TempEmp.EmployeeName).Append("|").Append(TempEmp.Address).Append("|").Append((TempEmp.Dept != null) ? TempEmp.Dept : "").Append("|").Append(TempEmp.DOJ.ToString()).Append("|").Append(TempEmp.DOB.ToString()).Append("|").Append(TempEmp.Contact != null ? TempEmp.Contact.ToString() : "").Append("|").Append(TempEmp.Salary.ToString());
        //        bool IsEdited = DataLayerObj.UpdateData(DataOfEmp.ToString(), TempEmp.EmployeeID.ToString());
        //        log.Info("Business Layer EditSingleEmployee Stop,The data is :" + TempEmp);
        //        return IsEdited;
        //    }
        //    catch (Exception e) {

        //        log.Error("Error in saving updated details of the employee, the data is :" + e);
        //    }
        //    return false;
         
        //}


        //Update single employee data
        public bool EditSingleEmployee(EmployeeObject employee)
        {
           _log.Info("Business Layer EditSingleEmployee Start");
            StringBuilder DataOfEmp = new StringBuilder();
            try
            {
                _log.Debug("Business Layer EditSingleEmployee data:" + ObjectSerializer.SerializeObject(employee));             
                bool IsEdited = SqlLayerObj.Edit(employee);
                _log.Debug("Business Layer EditSingleEmployee return data is :" + IsEdited);
                return IsEdited;
            }
            catch (Exception e)
            {

                _log.Error("Error in saving updated details of the employee, the data is :" + e);
            }
            finally
            {
                _log.Info("Business Layer EditSingleEmployee stop");
            }
            return false;

        }

        public bool RegisterUser(UserObject user)
        {
            _log.Info("Business Layer Register method start");
            try 
            {
                _log.Debug("Business Layer Register data is :" + ObjectSerializer.SerializeObject(user));
                bool IsUserRegistered = SqlLayerObj.RegisterUser(user);
                _log.Debug("Business Layer Register returned data is :" + IsUserRegistered);
                return IsUserRegistered; 
            }
            catch (Exception e)
            {
                _log.Error("Business Layer RegisterUser error:" + e.Message);
                return false;
            }
            finally
            {
                _log.Info("Business Layer Register method stop");
            }
                           
        }
        //Register new user
        //public bool RegisterUser(dynamic register)
        //{
        //    _log.Info("Business Layer Register User Start,The data is :" + register);
        //    List<dynamic> UserList = GetUserList();
        //    try
        //    {
        //        foreach (dynamic User in UserList)
        //        {
        //            if (User.username.Equals(register.GetType().GetProperty("username").GetValue(register, null)))
        //                return false;
        //        }
        //        StringBuilder DataOfUser = new StringBuilder();
        //        dynamic TempRegister = new
        //        {
        //            Username = register.GetType().GetProperty("username").GetValue(register, null),
        //            Password = register.GetType().GetProperty("password").GetValue(register, null),
        //            Contact = register.GetType().GetProperty("contact").GetValue(register, null),
        //            Name = register.GetType().GetProperty("name").GetValue(register, null),

        //        };
        //        DataOfUser.Append(TempRegister.Username).Append("|").Append(TempRegister.Password).Append("|").Append((TempRegister.Name != null) ? TempRegister.Name : "").Append("|").Append((TempRegister.Contact != null) ? TempRegister.Contact : "");
        //        bool RegisteredUser = DataLayerObj.SaveData(DataOfUser.ToString(), TempRegister.Username.ToString(),"User.txt");
        //        _log.Info("Business Layer Register User Stop,is user registered? :" + RegisteredUser);
        //        return RegisteredUser;
        //    }
        //    catch (Exception e) {
        //        _log.Error("Error in registering the user, the error is :" + e.Message);
        //    }
        //    return false;
          
        //}



        //Get the entire list of employees        
        public List<EmployeeObject> GetAllEmployees(string searchString, string sortDirection, string sortField, int pageSize, int currPage)
        {
            _log.Info("Business layer GetallEmployees start");
            try
            {
                if (sortDirection == "ascending")
                    sortDirection = "ASC";
                else
                    sortDirection = "DESC";
                List <EmployeeObject> JsonEmployee = SqlLayerObj.GetEmployeeData(searchString, sortDirection, sortField, currPage, pageSize);
                _log.Debug("Business Layer GetAllEmployees returned list is :" + JsonEmployee);              
                return JsonEmployee;

            }
            catch (Exception e)
            {

                _log.Error("Business Layer Error in GetAllEmployees,The exception is :" + e);
            }
            finally
            {
                _log.Info("Business Layer GetAllEmployees mandatory stop");
            }
           
            return null;
        }

        //Get individual employee data
        //public dynamic GetSingleEmployee(Guid id)
        //{
        //    log.Info("Business Layer GetSingleEmployee Start, the employee Id is :" + id);
        //    EMSObject SingleUser = SqlLayerObj.GetSingleEmpData(id.ToString());
        //    string EmpData = DataLayerObj.GetSingleEmpData(id.ToString());
        //    dynamic SingleData = new ExpandoObject();
        //    if (EmpData != null)
        //    {

            
        //        SingleData = new
        //        {
        //            EmployeeID = Guid.Parse(dataItem[0]),
        //            Email = dataItem[1],
        //            EmployeeName = dataItem[2],
        //            Address = dataItem[3],
        //            Dept = dataItem[4],
        //            DOJ = DateTime.Parse(dataItem[5]).Date.ToString("d"),
        //            DOB = DateTime.Parse(dataItem[6]).Date.ToString("d"),
        //            Contact = dataItem[7],
        //            Salary = Int32.Parse(dataItem[8])
        //        };
        //        log.Info("Business Layer GetSingleEmployee Stop,The Employee data is :" + SingleData);
        //        return SingleData;
        //    }

        //    return null;
        //}

        //Using MysQL 
        public EmployeeObject GetSingleEmployee(Guid id)
        {
            _log.Info("Business Layer GetSingleEmployee Start");
            try {
                _log.Debug("Business Layer GetSingleEmployee employee Id is :" + id);
                EmployeeObject Employee = SqlLayerObj.GetSingleEmpData(id.ToString());              
                   _log.Info("Business Layer GetSingleEmployee Stop,The Employee data is :" + Employee);
                    return Employee;
                }                         
            catch (Exception e)
            {
                _log.Error("Business Layer GetSingleEmployee Exception is :" + e.Message);
                return null;
            }
            finally
            {
                _log.Info("Business Layer GetSingleEmployee Stop");
            }       
        }


        //Registering a new user
        //public bool SaveUser(dynamic newEmployee)
        //{
        //    try {
        //        log.Info("Business Layer SaveUser Start,The Employee data is :" + newEmployee);
        //        string GuidEmp = Guid.NewGuid().ToString();
        //        StringBuilder DataOfEmp = new StringBuilder();
             
        //        dynamic TempEmp = new
        //        {
        //            EmployeeID = newEmployee.GetType().GetProperty("EmployeeID").GetValue(newEmployee, null),
        //            Email = newEmployee.GetType().GetProperty("Email").GetValue(newEmployee, null),
        //            EmployeeName = newEmployee.GetType().GetProperty("EmployeeName").GetValue(newEmployee, null),
        //            Address = newEmployee.GetType().GetProperty("Address").GetValue(newEmployee, null),
        //            Dept = newEmployee.GetType().GetProperty("Dept").GetValue(newEmployee, null),
        //            DOJ = (newEmployee.GetType().GetProperty("DOJ").GetValue(newEmployee, null)),
        //            DOB = (newEmployee.GetType().GetProperty("DOB").GetValue(newEmployee, null)),
        //            Contact = newEmployee.GetType().GetProperty("Contact").GetValue(newEmployee, null),
        //            Salary = newEmployee.GetType().GetProperty("Salary").GetValue(newEmployee, null)

        //        };
        //        DataOfEmp.Append(GuidEmp).Append("|").Append(TempEmp.Email).Append("|").Append(TempEmp.EmployeeName).Append("|").Append(TempEmp.Address).Append("|").Append((TempEmp.Dept != null) ? TempEmp.Dept : "").Append("|").Append(TempEmp.DOJ.ToString()).Append("|").Append(TempEmp.DOB.ToString()).Append("|").Append(TempEmp.Contact != null ? TempEmp.Contact.ToString() : "").Append("|").Append(TempEmp.Salary.ToString());
        //        bool ret = DataLayerObj.SaveData(DataOfEmp.ToString(), TempEmp.Email.ToString(),"Employee.txt");
        //        log.Info("Business Layer SaveUser Stop,Is User saved? :" + ret);
        //        return ret;
            
        //    }
        //    catch (Exception e) {
        //        log.Error("Business Layer SaveUser Stop err,The error is is :" + e.Message);
        //    }
        //    return false;
            
        //}
        //Registering a new user
        public bool SaveUser(EmployeeObject newEmployee)
        {
           _log.Info("Business Layer SaveUser Start");
           try
           {
               _log.Debug("The Employee data is :" + ObjectSerializer.SerializeObject(newEmployee));                     
               bool EmpCreated = SqlLayerObj.CreateData(newEmployee);
               _log.Debug("Business Layer SaveUser,Is User saved? :" + EmpCreated);
               return EmpCreated;

           }
           catch (Exception e)
           {
               _log.Error("Business Layer SaveUser error is :" + e.Message);
           }
           finally
           {
               _log.Info("Business Layer SaveUser Stop");           
           }
            return false;
       }
    }
}
