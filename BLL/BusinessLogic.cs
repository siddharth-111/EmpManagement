using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using DTObject;
using DataLayer;
using System.Web.Script.Serialization;
namespace BLL
{

    public class BusinessLogic
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Check if the Login user is valid
        FileAccess DataLayerObj = new FileAccess();
        SQLRetrieve SqlLayerObj = new SQLRetrieve();
        public bool IsUserValid(dynamic login)
        {
          //  log.Info("Business Layer IsUserValid start, The data--> Username:" + login.username+",Password:"+login.password);
            List<dynamic> NewList = GetSQLUserList();
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

        //Get SQL User list
        public List<dynamic> GetSQLUserList() {
            log.Info("Business Layer Get SQL User list method start:");
            try
            {
                List<UserObject> UserData= SqlLayerObj.GetUserData();
                log.Debug("The User data from SQL proc is :" + new JavaScriptSerializer().Serialize(UserData));
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
                    log.Info("Business Layer Get SQL User list method stop");
                    return UserList;
                }

            }
            catch (Exception e) {
                log.Error("The Error in retrieving data from Business Layer Get SQL User List,The error is :" + e.Message);
            }
            log.Info("Business Layer Get SQL User list method stop with no data");
            return null;
            
        }


        //Get the list of users from file
        public List<dynamic> GetUserList()
        {
            List<UserObject> UserL = SqlLayerObj.GetUserData();
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
            bool Val = SqlLayerObj.DeleteData(id.ToString());
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

                log.Error("Error in saving updated details of the employee, the data is :" + e);
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
            log.Info("Business layer GetallEmployees start");
            try {
                //log.Info("Business Layer GetAllEmployees Called,The data is : searchstring:" + searchString + ",sortDirection:" + sortDirection + ",sortField:" + sortField + ",pageSize:" + pageSize + ",currPage:" + currPage);
           //     List<dynamic> EmpData = DataLayerObj.GetEmployeeData(searchString, sortDirection, sortField, pageSize, currPage);
                List<EMSObject> EmpData = SqlLayerObj.GetEmployeeData();
                List<EmployeeObject> EmpList = new List<EmployeeObject>();
                if (EmpData != null)
                {
                    foreach (EMSObject Emp in EmpData)
                    {
                        

                        EmpList.Add(new EmployeeObject
                        {
                            EmployeeID = Emp.EmployeeID,
                            Email = Emp.Email,
                            EmployeeName = Emp.EmployeeName,
                            Address = Emp.Address,
                            Dept = Emp.Dept,
                            DOJ = Emp.DOJ,
                            DOB = Emp.DOB,
                            Contact = Emp.Contact,
                            Salary = Emp.Salary
                        });
                    }
                    IQueryable<EmployeeObject> emp = EmpList.AsQueryable();
                    var ModEmplist = from s in emp
                                     select s;
                    if (!String.IsNullOrEmpty(searchString))
                    {

                        int checkForNumber;
                        DateTime checkForDate;
                        bool resultOfDateParse = DateTime.TryParse(searchString, out checkForDate);
                        bool isNumeric = int.TryParse(searchString, out checkForNumber);

                        if (resultOfDateParse)
                        {
                            ModEmplist = ModEmplist.Where(s => DateTime.Compare(s.DOB, checkForDate) == 0 || DateTime.Compare(s.DOJ, checkForDate) == 0);

                        }
                        else if (isNumeric)
                        {
                            ModEmplist = ModEmplist.Where(s => s.Salary == Int32.Parse(searchString) || s.Contact.ToUpper().Contains(searchString.ToUpper()));
                        }
                        else
                        {
                            ModEmplist = ModEmplist.Where(s => s.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || s.Address.ToUpper().Contains(searchString.ToUpper()) || s.Contact.ToUpper().Equals(searchString.ToUpper()) || s.Dept.ToUpper().Equals(searchString.ToUpper()) || s.Email.ToUpper().Contains(searchString.ToUpper()));
                        }

                    }
                    if (sortDirection == "ascending")
                    {
                        switch (sortField)
                        {
                            case "Email":
                                ModEmplist = ModEmplist.OrderBy(s => s.Email);
                                break;
                            case "Name":
                                ModEmplist = ModEmplist.OrderBy(s => s.EmployeeName);
                                break;
                            case "Address":
                                ModEmplist = ModEmplist.OrderBy(s => s.Address);
                                break;
                            case "Department":
                                ModEmplist = ModEmplist.OrderBy(s => s.Dept);
                                break;
                            case "DOJ":
                                ModEmplist = ModEmplist.OrderBy(s => s.DOJ);
                                break;
                            case "DOB":
                                ModEmplist = ModEmplist.OrderBy(s => s.DOB);
                                break;
                            case "Salary":
                                ModEmplist = ModEmplist.OrderBy(s => s.Salary);
                                break;
                            case "Contact":
                                ModEmplist = ModEmplist.OrderBy(s => s.Contact);
                                break;
                        }
                    }
                    else
                    {
                        switch (sortField)
                        {
                            case "Email":
                                ModEmplist = ModEmplist.OrderByDescending(s => s.Email);
                                break;
                            case "Name":
                                ModEmplist = ModEmplist.OrderByDescending(s => s.EmployeeName);
                                break;
                            case "Address":
                                ModEmplist = ModEmplist.OrderByDescending(s => s.Address);
                                break;
                            case "Department":
                                ModEmplist = ModEmplist.OrderByDescending(s => s.Dept);
                                break;
                            case "DOJ":
                                ModEmplist = ModEmplist.OrderByDescending(s => s.DOJ);
                                break;
                            case "DOB":
                                ModEmplist = ModEmplist.OrderByDescending(s => s.DOB);
                                break;
                            case "Salary":
                                ModEmplist = ModEmplist.OrderByDescending(s => s.Salary);
                                break;
                            case "Contact":
                                ModEmplist = ModEmplist.OrderByDescending(s => s.Contact);
                                break;
                        }
                    }
                    var EmployeeData = new List<dynamic>();
                    var Count = ModEmplist.Count();
                    var PageCount = Convert.ToInt32(Math.Ceiling((double)((Count + pageSize - 1) / pageSize)));
                    var newEmpList = ModEmplist.ToList().Skip(currPage * pageSize).Take(pageSize);
                    foreach (EmployeeObject temp in newEmpList)
                    {
                        EmployeeData.Add(
                             new
                             {
                                 EmployeeID = temp.EmployeeID,
                                 Email = temp.Email,
                                 EmployeeName = temp.EmployeeName,
                                 Address = temp.Address,
                                 DOB = temp.DOB.Date.ToString("d"),
                                 DOJ = temp.DOJ.Date.ToString("d"),
                                 Dept = temp.Dept,
                                 Salary = temp.Salary,
                                 Contact = temp.Contact,

                             }
                             );
                    }
                    EmployeeData.Add(new
                    {
                        Pagecount = PageCount,
                        TotalRecords = Count
                    });
                    log.Info("File Access GetEmployeeData stop,the returned list is :" + EmployeeData);
                    return EmployeeData;
                }
            }
            catch (Exception e) {

                log.Error("Error in returning employees :" + e);
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
            log.Info("Business Layer GetSingleEmployee Start, the employee Id is :" + id);
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
                log.Info("Business Layer GetSingleEmployee Stop,The Employee data is :" + SingleData);
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
           log.Info("Business Layer SaveUser Start");
            try {
                log.Debug("The Employee data is :" + newEmployee);
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
                log.Debug("Business Layer SaveUser,Is User saved? :" + EmpCreated);
                log.Info("Business Layer SaveUser Stop,Is User saved?");
                return EmpCreated;

            }
            catch (Exception e) {
                log.Error("Business Layer SaveUser Stop err,The error is is :" + e.Message);
            }
            return false;
       }
    }
}
