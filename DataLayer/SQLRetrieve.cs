using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DTObject;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using CommonUtility;
using System.Web.Script.Serialization;

namespace DataLayer
{
   public class SQLRetrieve
    {
       
       Logger Wrapper = new Logger();
       string Connectionstr = "Data Source=localhost;port=3306;Initial Catalog=EmployeeManagementSystem;User Id=root;password=admin;Convert Zero Datetime=True";
       private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      
       public List<UserObject> GetUserData()
       {
           _log.Info("SQL Retrieve GetUserData start : ");
           List<UserObject> Userlist = new List<UserObject>();
        
           try
           {
               using (MySqlConnection con = new MySqlConnection(Connectionstr))
               {
                   using (MySqlCommand cmd = new MySqlCommand("CALL getUsers()"))
                   {
                       using (MySqlDataAdapter sda = new MySqlDataAdapter())
                       {
                           cmd.Connection = con;
                           sda.SelectCommand = cmd;
                           using (DataTable dt = new DataTable())
                           {
                               sda.Fill(dt);
                               foreach (DataRow row in dt.Rows)
                               {
                                   Userlist.Add(new UserObject
                                   {
                                       Email = Convert.ToString(row["email"].ToString()),
                                       Password = Convert.ToString(row["password"].ToString()),
                                   });
                               }

                           }
                       }
                   }
                   con.Close();
               }
               _log.Debug("SQL Retrieve GetUserData UserData:" + new JavaScriptSerializer().Serialize(Userlist));
               _log.Info("SQL Retrieve GetUserData method successful stop");
               return Userlist;


           }
           catch (Exception e)
           {
               _log.Error("SQL Retrieve GetUserData Error,the error is : " + e.Message);
       
           }
           _log.Info("SQL Retrieve GetUserData stop,null data : ");
           return null;  
       
       }


       public List<EMSObject> GetEmployeeData()
       {
           _log.Info("SQL Retrieve GetEmployeeData start : ");
           List<EMSObject> Employeelist = new List<EMSObject>();

           try
           {
               using (MySqlConnection con = new MySqlConnection(Connectionstr))
               {
                   using (MySqlCommand cmd = new MySqlCommand("CALL getEmployees()"))
                   {
                       using (MySqlDataAdapter sda = new MySqlDataAdapter())
                       {
                           cmd.Connection = con;
                           sda.SelectCommand = cmd;
                           using (DataTable dt = new DataTable())
                           {
                               sda.Fill(dt);
                               foreach (DataRow row in dt.Rows)
                               {
                                   Employeelist.Add(new EMSObject
                                   {
                                       EmployeeID = Guid.Parse(row["EmployeeID"].ToString()),
                                       EmployeeName = Convert.ToString(row["Name"].ToString()),
                                       Email = Convert.ToString(row["Email"].ToString()),
                                       Address = Convert.ToString(row["Address"].ToString()),
                                       Contact = Convert.ToString(row["Contact"].ToString()),
                                       Dept = Convert.ToString(row["Dept"].ToString()),
                                       DOB = DateTime.Parse(row["DOB"].ToString()),
                                       DOJ = DateTime.Parse(row["DOJ"].ToString()),
                                       Salary = Convert.ToInt32(row["Salary"].ToString())
                                   });
                               }

                           }
                       }
                   }
                   con.Close();
               }
               _log.Debug("SQL Retrieve GetEmployeeData Employee Data:" + new JavaScriptSerializer().Serialize(Employeelist));
               _log.Info("SQL Retrieve GetEmployeeeData method successful stop");
               return Employeelist;


           }
           catch (Exception e)
           {
               _log.Error("SQL Retrieve GetEmployeeData Error,the error is : " + e.Message);

           }
           _log.Info("SQL Retrieve GetEmployeData stop,null data: ");
           return null;

       }

       public EMSObject GetSingleEmpData(string id) {
           _log.Info("SQL Retrieve GetSingleEmployeeData start : ");
           EMSObject SingleEmployee = new EMSObject();
           try
           {
               using (MySqlConnection con = new MySqlConnection(Connectionstr))
               {
                   using (MySqlCommand cmd = new MySqlCommand("CALL getSingleEmployee('"+id+"')"))
                   {
                       using (MySqlDataAdapter sda = new MySqlDataAdapter())
                       {
                           cmd.Connection = con;
                           sda.SelectCommand = cmd;
                           using (DataTable dt = new DataTable())
                           {
                              sda.Fill(dt);
                               foreach (DataRow row in dt.Rows)
                               {
                                 SingleEmployee.EmployeeID = Guid.Parse(row["EmployeeID"].ToString());
                           
                                        SingleEmployee.EmployeeName = Convert.ToString(row["Name"].ToString());
                                        SingleEmployee.Email = Convert.ToString(row["Email"].ToString());
                                        SingleEmployee.Address = Convert.ToString(row["Address"].ToString());
                                        SingleEmployee.Contact = Convert.ToString(row["Contact"].ToString());
                                        SingleEmployee.Dept = Convert.ToString(row["Dept"].ToString());
                                        SingleEmployee.DOB = DateTime.Parse(row["DOB"].ToString());
                                        SingleEmployee.DOJ = DateTime.Parse(row["DOJ"].ToString());
                                        SingleEmployee.Salary = Convert.ToInt32(row["Salary"].ToString());   
                               }
                           }
                       }
                   }
                   con.Close();
               }
               _log.Debug("SQL Retrieve GetSingleEmployeeData Employee Data:" + new JavaScriptSerializer().Serialize(SingleEmployee));
               _log.Info("SQL Retrieve GetSingleEmployeeeData method successful stop");
               return SingleEmployee;


           }
           catch (Exception e)
           {
               _log.Error("SQL Retrieve GetSingleEmployeeData Error,the error is : " + e.Message);

           }
           _log.Info("SQL Retrieve GetSingleEmployeData stop,null data: ");
           return null;
       
       
       }


       public bool DeleteData(string id) {
           _log.Info("SQL Retrieve DeleteSingleEmployeData start ");
           try
           {
               _log.Debug("SQL Retrieve DeleteSingleEmployeData id : "+id);
               using (MySqlConnection con = new MySqlConnection(Connectionstr))
               {
                   con.Open();
                   using (MySqlCommand cmd = new MySqlCommand())
                   {
                       cmd.Connection = con;
                       cmd.CommandText = "CALL deleteEmployee('"+id+"')";                       
                       cmd.ExecuteNonQuery();
                   }


               }
               _log.Info("SQL Retrieve DeleteSingleEmployeData stop ");
               return true;
           }
           catch (Exception e)
           {
               _log.Error("SQL Retrieve DeleteEmployeeData Error,the error is : " + e.Message);
               return false;
           }
       
       
       }

       public bool CreateData(EMSObject newUser)
       {
           _log.Info("SQL Retrieve CreateData start ");
           try
           {
               _log.Debug("SQL Retrieve New employee data : " + new JavaScriptSerializer().Serialize(newUser));
               using (MySqlConnection con = new MySqlConnection(Connectionstr))
               {
                   con.Open();
                   using (MySqlCommand cmd = new MySqlCommand())
                   {
                       cmd.Connection = con;
                       cmd.CommandText = "CALL createNewEmployee(@EmployeeID,@Name,@Email,@Address,@Dept,@Contact,@DOB,@DOJ,@Salary);";
                       cmd.Parameters.AddWithValue("@EmployeeID",newUser.EmployeeID);
                       cmd.Parameters.AddWithValue("@Name", newUser.EmployeeName);
                       cmd.Parameters.AddWithValue("@Email", newUser.Email);
                       cmd.Parameters.AddWithValue("@Address", newUser.Address);
                       cmd.Parameters.AddWithValue("@Dept", newUser.Dept);
                       cmd.Parameters.AddWithValue("@Contact", newUser.Contact);
                       cmd.Parameters.AddWithValue("@DOB", newUser.DOB.Date.ToString("yyyy-MM-dd"));
                       cmd.Parameters.AddWithValue("@DOJ", newUser.DOJ.Date.ToString("yyyy-MM-dd"));
                       cmd.Parameters.AddWithValue("@Salary", newUser.Salary);
                       cmd.ExecuteNonQuery();
                   }


               }
                _log.Info("SQL Retrieve Create new employee method stop");
               return true;
           }
           catch (Exception e)
           {
               _log.Error("SQL Retrieve Create new employee exception is : " + e);
               return false;
           }
       
        
       }


    }
}
