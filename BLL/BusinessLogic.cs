using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using DTObject;
using DataLayer;
using System.Web.Script.Serialization;
using CommonUtility;
namespace BLL
{

    public class BusinessLogic
    {
      //  private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Check if the Login user is valid
        Logger Wrapper = new Logger();
        FileAccess DataLayerObj = new FileAccess();
        SQLRetrieve SqlLayerObj = new SQLRetrieve();



        public bool IsUserValid(dynamic login)
        {
            Wrapper.Log.Info("Business Layer IsUserValid method start ");
            try
            {
                List<dynamic> NewList = GetSQLUserList();
                foreach (dynamic List in NewList)
                {
                    if (List.username == login.GetType().GetProperty("username").GetValue(login, null) && List.password == login.GetType().GetProperty("password").GetValue(login, null))
                    {
                        // log.Info("Business Layer IsUserValid start, The data--> Username:" + login.username + ",Password:" + login.password);
                        return true;
                    }

                }
                return false;
            }
            catch (Exception e) {
                Wrapper.Log.Error("Business Layer IsUserValid exception,the exception is :" + e.Message);
                return false;
            }
            finally {
                Wrapper.Log.Info("Business Layer IsUserValid method mandatory stop");            
            }
          
        }

        //Get SQL User list
        public List<dynamic> GetSQLUserList() {
            Wrapper.Log.Info("Business Layer Get SQL User list method start:");
            try
            {
                List<UserObject> UserData = SqlLayerObj.GetUserData();
                Wrapper.Log.Debug("The User data from SQL proc is :" + new JavaScriptSerializer().Serialize(UserData));
                List<dynamic> UserList = new List<dynamic>();
                if (UserData != null)
                {
                    foreach (UserObject SingleUser in UserData)
                    {
                        UserList.Add(new
                        {
                            username = SingleUser.Email,
                            password = SingleUser.Password
                        });
                    }
                    Wrapper.Log.Info("Business Layer Get SQL User list method stop");
                    return UserList;
                }

            }
            catch (Exception e)
            {
                Wrapper.Log.Error("The Error in retrieving data from Business Layer Get SQL User List,The error is :" + e.Message);
            }
            finally
            {
                Wrapper.Log.Info("Business Layer Get SQL User list method mandatory stop");
            }
           
            return null;
            
        }


        //Get the list of users from file
        public List<dynamic> GetUserList()
        {
            Wrapper.Log.Info("Business Layer GetUserData start");
            try {
                List<UserObject> UserL = SqlLayerObj.GetUserData();
                string[] UserData = DataLayerObj.GetUserData();
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
                    Wrapper.Log.Info("Business Layer GetUserData Stop");
                    return UserList;
                }
                
            }
            catch (Exception e) {
                Wrapper.Log.Error("Business Layer GetUserData Exception : " + e.Message);
            }
            finally {
                Wrapper.Log.Info("Business Layer GetUserData mandatory stop :");
            }
           
            return null;
        }

        //Delete Employee 
        public bool DeleteEmployee(Guid id)
        {
            Wrapper.Log.Info("Business Layer DeletEmployee Start");
            bool Val = SqlLayerObj.DeleteData(id.ToString());
            Wrapper.Log.Info("Business Layer DeleteEmployee Stop,is User Deleted? :" + Val);
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
        public bool EditSingleEmployee(dynamic newEmployee)
        {
           Wrapper.Log.Info("Business Layer EditSingleEmployee Start,The data is :" + newEmployee);
            StringBuilder DataOfEmp = new StringBuilder();
            try
            {

                EMSObject TempEmp = new EMSObject
                {
                    EmployeeID = newEmployee.GetType().GetProperty("EmployeeID").GetValue(newEmployee, null),
                    Email = newEmployee.GetType().GetProperty("Email").GetValue(newEmployee, null),
                    EmployeeName = newEmployee.GetType().GetProperty("EmployeeName").GetValue(newEmployee, null),
                    Address = newEmployee.GetType().GetProperty("Address").GetValue(newEmployee, null),
                    Dept = newEmployee.GetType().GetProperty("Dept").GetValue(newEmployee, null),
                    DOJ = DateTime.Parse(newEmployee.GetType().GetProperty("DOJ").GetValue(newEmployee, null)),
                    DOB = DateTime.Parse(newEmployee.GetType().GetProperty("DOB").GetValue(newEmployee, null)),
                    Contact = newEmployee.GetType().GetProperty("Contact").GetValue(newEmployee, null),
                    Salary = newEmployee.GetType().GetProperty("Salary").GetValue(newEmployee, null)

                };

                bool IsEdited = SqlLayerObj.Edit(TempEmp);
                Wrapper.Log.Info("Business Layer EditSingleEmployee Stop,The data is :" + TempEmp);
                return IsEdited;
            }
            catch (Exception e)
            {

                Wrapper.Log.Error("Error in saving updated details of the employee, the data is :" + e);
            }
            return false;

        }

        //Register new user
        public bool RegisterUser(dynamic register)
        {
            Wrapper.Log.Info("Business Layer Register User Start,The data is :" + register);
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
                Wrapper.Log.Info("Business Layer Register User Stop,is user registered? :" + RegisteredUser);
                return RegisteredUser;
            }
            catch (Exception e) {
                Wrapper.Log.Error("Error in registering the user, the error is :" + e.Message);
            }
            return false;
          
        }



        //Get the entire list of employees        
        public List<dynamic> GetAllEmployees(string searchString, string sortDirection, string sortField, int pageSize, int currPage)
        {
            Wrapper.Log.Info("Business layer GetallEmployees start");
            try {
                if (sortDirection == "ascending")
                    sortDirection = "ASC";
                else
                    sortDirection = "DESC";
                //log.Info("Business Layer GetAllEmployees Called,The data is : searchstring:" + searchString + ",sortDirection:" + sortDirection + ",sortField:" + sortField + ",pageSize:" + pageSize + ",currPage:" + currPage);
           //     List<dynamic> EmpData = DataLayerObj.GetEmployeeData(searchString, sortDirection, sortField, pageSize, currPage);
             //   List<EMSObject> EmpData = SqlLayerObj.GetEmployeeData(searchString,sortDirection,sortField,(currPage*pageSize),pageSize+(currPage*pageSize));
                List<EMSObject> EmpData = SqlLayerObj.GetEmployeeData(searchString, sortDirection, sortField, currPage, pageSize);
                List<dynamic> JsonEmployee = new List<dynamic>();

                var Count = EmpData.Count();
                for (int i = 0; i < Count - 1; i++)
                {
                    JsonEmployee.Add(new 
                    {
                        EmployeeID = EmpData[i].EmployeeID,
                        Email = EmpData[i].Email,
                        EmployeeName = EmpData[i].EmployeeName,
                        Address = EmpData[i].Address,
                        Dept = EmpData[i].Dept,
                        DOJ = EmpData[i].DOJ.ToShortDateString(),
                        DOB = EmpData[i].DOB.ToShortDateString(),
                        Contact = EmpData[i].Contact,
                        Salary = EmpData[i].Salary,
                    });
                }
                JsonEmployee.Add(new 
                {
                    Pagecount = EmpData[Count-1].Pagecount,
                    TotalRecords = EmpData[Count-1].TotalRecords,
                });
                    Wrapper.Log.Info("File Access GetEmployeeData stop,the returned list is :" + JsonEmployee);
                    return JsonEmployee;
                
            }
            catch (Exception e) {

                Wrapper.Log.Error("Error in returning employees :" + e);
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
        public dynamic GetSingleEmployee(Guid id)
        {
            Wrapper.Log.Info("Business Layer GetSingleEmployee Start, the employee Id is :" + id);
            EMSObject EmpData = SqlLayerObj.GetSingleEmpData(id.ToString());;
            dynamic SingleData = new ExpandoObject();
            if (EmpData != null)
            {
                SingleData = new
                {
                    EmployeeID = EmpData.EmployeeID,
                    Email = EmpData.Email,
                    EmployeeName = EmpData.EmployeeName,
                    Address = EmpData.Address,
                    Dept = EmpData.Dept,
                    DOJ = EmpData.DOJ.ToShortDateString(),
                    DOB = EmpData.DOB.ToShortDateString(),
                    Contact = EmpData.Contact,
                    Salary = EmpData.Salary
                };
                Wrapper.Log.Info("Business Layer GetSingleEmployee Stop,The Employee data is :" + SingleData);
                return SingleData;
            }
            return null;
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
        public bool SaveUser(dynamic newEmployee)
        {
           Wrapper.Log.Info("Business Layer SaveUser Start");
            try {
               Wrapper.Log.Debug("The Employee data is :" + newEmployee);
                string GuidEmp = Guid.NewGuid().ToString();
                StringBuilder DataOfEmp = new StringBuilder();

                EMSObject TempEmp = new EMSObject
                {
                    EmployeeID = Guid.NewGuid(),
                    Email = newEmployee.GetType().GetProperty("Email").GetValue(newEmployee, null),
                    EmployeeName = newEmployee.GetType().GetProperty("EmployeeName").GetValue(newEmployee, null),
                    Address = newEmployee.GetType().GetProperty("Address").GetValue(newEmployee, null),
                    Dept = newEmployee.GetType().GetProperty("Dept").GetValue(newEmployee, null),
                    DOJ = DateTime.Parse(newEmployee.GetType().GetProperty("DOJ").GetValue(newEmployee, null)),
                    DOB = DateTime.Parse(newEmployee.GetType().GetProperty("DOB").GetValue(newEmployee, null)),
                    Contact = newEmployee.GetType().GetProperty("Contact").GetValue(newEmployee, null),
                    Salary = newEmployee.GetType().GetProperty("Salary").GetValue(newEmployee, null)

                };
                bool EmpCreated = SqlLayerObj.CreateData(TempEmp);
                Wrapper.Log.Debug("Business Layer SaveUser,Is User saved? :" + EmpCreated);
                Wrapper.Log.Info("Business Layer SaveUser Stop,Is User saved?");
                return EmpCreated;

            }
            catch (Exception e) {
                Wrapper.Log.Error("Business Layer SaveUser Stop err,The error is is :" + e.Message);
            }
            return false;
       }
    }
}
