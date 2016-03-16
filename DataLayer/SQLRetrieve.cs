using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DataObject;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using CommonUtility;
using System.Web.Script.Serialization;

namespace DataLayer
{
    public class SQLRetrieve
    {
        Serializer ObjectSerializer = new Serializer();
        string Connectionstr = "Data Source=localhost;port=3306;Initial Catalog=EmployeeManagementSystem;User Id=root;password=admin;Convert Zero Datetime=True";
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<UserObject> GetUserData()
        {
            _log.Info("SQL Retrieve GetUserData start : ");
            List<UserObject> Userlist = new List<UserObject>();
            MySqlConnection Con = new MySqlConnection(Connectionstr);
            try
            {

                MySqlCommand Cmd = new MySqlCommand("CALL getUsers()", Con);
                MySqlDataAdapter Sda = new MySqlDataAdapter();
                Cmd.Connection = Con;
                Sda.SelectCommand = Cmd;
                DataTable dt = new DataTable();
                Sda.Fill(dt);
                
                foreach (DataRow row in dt.Rows)
                {
                    Userlist.Add(new UserObject
                  {
                      Email = Convert.ToString(row["email"].ToString()),
                      Password = Convert.ToString(row["password"].ToString()),
                  });
                }
                _log.Debug("SQL Retrieve GetUserData UserData:" + ObjectSerializer.SerializeObject(Userlist));
                _log.Info("SQL Retrieve GetUserData method successful stop");
                return Userlist;

            }
            
            catch (Exception e)
            {
                _log.Error("SQL Retrieve GetUserData Error,the error is : " + e.Message);
                return null;
            }
            finally
            {
                Con.Close();
                _log.Info("SQL Retrieve GetUserData stop");
            }
        }


        public List<EmployeeObject> GetEmployeeData(string searchString, string sortDirection, string sortField, int currPage, int pageSize)
        {
            _log.Info("SQL Retrieve GetEmployeeData start : ");
            List<EmployeeObject> Employeelist = new List<EmployeeObject>();
            if (searchString == null)
            {
                MySqlConnection Con = new MySqlConnection(Connectionstr);
                try
                {
                    Con.Open();
                    MySqlCommand Cmd = new MySqlCommand("CALL getTotalEmployees('" + sortField + "','" + sortDirection + "'," + (currPage * pageSize) + "," + (pageSize) + ")", Con);
                    MySqlDataAdapter Sda = new MySqlDataAdapter();
                    Cmd.Connection = Con;
                    Sda.SelectCommand = Cmd;
                    DataTable Dt = new DataTable();
                    Sda.Fill(Dt);
                    foreach (DataRow row in Dt.Rows)
                    {
                        Employeelist.Add(new EmployeeObject
                        {
                            EmployeeID = Guid.Parse(row["EmployeeID"].ToString()),
                            EmployeeName = Convert.ToString(row["Name"].ToString()),
                            Email = Convert.ToString(row["Email"].ToString()),
                            Address = Convert.ToString(row["Address"].ToString()),
                            Contact = Convert.ToString(row["Contact"].ToString()),
                            Dept = Convert.ToString(row["Dept"].ToString()),
                            DOB = Convert.ToString(row["DOB"].ToString()),
                            DOJ = Convert.ToString(row["DOJ"].ToString()),
                            Salary = Convert.ToInt32(row["Salary"].ToString())
                        });
                    }

                    MySqlCommand cmdCount = new MySqlCommand("CALL getCount()", Con);
                    int Count = Convert.ToInt32(cmdCount.ExecuteScalar());
                    int PageCount = Convert.ToInt32(Math.Ceiling((double)((Count + pageSize - 1) / pageSize)));
                    Employeelist.Add(new EmployeeObject
                    {
                        Pagecount = PageCount,
                        TotalRecords = Count
                    });
                    _log.Debug("SQL Retrieve GetEmployeeData Employee Data:" + ObjectSerializer.SerializeObject(Employeelist));
                    return Employeelist;


                }
                catch (Exception e)
                {
                    _log.Error("SQL Retrieve GetEmployeeData Error,the error is : " + e.Message);

                }
                finally
                {
                    Con.Close();
                    _log.Info("SQL Retrieve GetEmployeData mandatory stop: ");
                }
            }
            else
            {
                MySqlConnection Con = new MySqlConnection(Connectionstr);
                try
                {
                    Con.Open();
                    MySqlCommand Cmd = new MySqlCommand("CALL GetEmployees('" + sortField + "','" + sortDirection + "'," + (currPage * pageSize) + "," + (pageSize) + ",'" + searchString + "')", Con);
                    MySqlDataAdapter Sda = new MySqlDataAdapter();
                    Cmd.Connection = Con;
                    Sda.SelectCommand = Cmd;
                    DataTable Dt = new DataTable();
                    Sda.Fill(Dt);
                    foreach (DataRow row in Dt.Rows)
                    {
                        Employeelist.Add(new EmployeeObject
                        {
                            EmployeeID = Guid.Parse(row["EmployeeID"].ToString()),
                            EmployeeName = Convert.ToString(row["Name"].ToString()),
                            Email = Convert.ToString(row["Email"].ToString()),
                            Address = Convert.ToString(row["Address"].ToString()),
                            Contact = Convert.ToString(row["Contact"].ToString()),
                            Dept = Convert.ToString(row["Dept"].ToString()),
                            DOB = Convert.ToString(row["DOB"].ToString()),
                            DOJ = Convert.ToString(row["DOJ"].ToString()),
                            Salary = Convert.ToInt32(row["Salary"].ToString())
                        });
                    }


                    MySqlCommand CmdCount = new MySqlCommand("CALL getSearchCount('" + searchString + "')", Con);
                    int Count = Convert.ToInt32(CmdCount.ExecuteScalar());
                    int PageCount = Convert.ToInt32(Math.Ceiling((double)((Count + pageSize - 1) / pageSize)));
                    Employeelist.Add(new EmployeeObject
                    {
                        Pagecount = PageCount,
                        TotalRecords = Count
                    });
                    _log.Debug("SQL Retrieve GetEmployeeData Employee Data:" + ObjectSerializer.SerializeObject(Employeelist));
                    return Employeelist;


                }
                catch (Exception e)
                {
                    _log.Error("SQL Retrieve GetEmployeeData Error,the error is : " + e.Message);
                    return null;
                }
                finally
                {
                    Con.Close();
                    _log.Info("SQL Retrieve GetEmployeData mandatory stop: ");
                }
            }
            return null;
        }

        public EmployeeObject GetSingleEmpData(string id)
        {
            _log.Info("SQL Retrieve GetSingleEmployeeData start : ");
            EmployeeObject SingleEmployee = new EmployeeObject();
            MySqlConnection Con = new MySqlConnection(Connectionstr);
            try
            {
                Con.Open();
                MySqlCommand Cmd = new MySqlCommand("CALL getSingleEmployee('" + id + "')", Con);
                MySqlDataAdapter Sda = new MySqlDataAdapter();

                Cmd.Connection = Con;
                Sda.SelectCommand = Cmd;
                DataTable dt = new DataTable();
                Sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    SingleEmployee.EmployeeID = Guid.Parse(row["EmployeeID"].ToString());
                    SingleEmployee.EmployeeName = Convert.ToString(row["Name"].ToString());
                    SingleEmployee.Email = Convert.ToString(row["Email"].ToString());
                    SingleEmployee.Address = Convert.ToString(row["Address"].ToString());
                    SingleEmployee.Contact = Convert.ToString(row["Contact"].ToString());
                    SingleEmployee.Dept = Convert.ToString(row["Dept"].ToString());
                    SingleEmployee.DOB = Convert.ToString(row["DOB"].ToString());
                    SingleEmployee.DOJ = Convert.ToString(row["DOJ"].ToString());
                    SingleEmployee.Salary = Convert.ToInt32(row["Salary"].ToString());
                }

                _log.Debug("SQL Retrieve GetSingleEmployeeData Employee Data:" + ObjectSerializer.SerializeObject(SingleEmployee));
                return SingleEmployee;

            }
            catch (Exception e)
            {
                _log.Error("SQL Retrieve GetSingleEmployeeData Error,the error is : " + e.Message);
                return null;

            }
            finally
            {
                Con.Close();
                _log.Info("SQL Retrieve GetSingleEmployeData mandatory stop: ");
            }

        }


        public bool DeleteData(string id)
        {
            _log.Info("SQL Retrieve DeleteSingleEmployeData start ");
            MySqlConnection Con = new MySqlConnection(Connectionstr);
            try
            {
                _log.Debug("SQL Retrieve DeleteSingleEmployeData id : " + id);
                Con.Open();
                MySqlCommand Cmd = new MySqlCommand();

                Cmd.Connection = Con;
                Cmd.CommandText = "CALL deleteEmployee('" + id + "')";
                Cmd.ExecuteNonQuery();

                _log.Info("SQL Retrieve DeleteSingleEmployeData stop ");
                return true;
            }
            catch (Exception e)
            {
                _log.Error("SQL Retrieve DeleteEmployeeData Error,the error is : " + e.Message);
                return false;
            }
            finally
            {
                Con.Close();
                _log.Info("SQL Retrieve DeleteEmployeeData mandatory stop");
            }

        }

        public bool CreateData(EmployeeObject newUser)
        {
            _log.Info("SQL Retrieve CreateData start ");
            MySqlConnection Con = new MySqlConnection(Connectionstr);
            try
            {
                Con.Open();
                DateTime DOJ = DateTime.Parse(newUser.DOJ);
                DateTime DOB = DateTime.Parse(newUser.DOB);
                _log.Debug("SQL Retrieve New employee data : " + ObjectSerializer.SerializeObject(newUser));

                MySqlCommand Cmd = new MySqlCommand();
                Cmd.Connection = Con;
                Cmd.CommandText = "CALL createNewEmployee(@EmployeeID,@Name,@Email,@Address,@Dept,@Contact,@DOB,@DOJ,@Salary);";
                Cmd.Parameters.AddWithValue("@EmployeeID", newUser.EmployeeID);
                Cmd.Parameters.AddWithValue("@Name", newUser.EmployeeName);
                Cmd.Parameters.AddWithValue("@Email", newUser.Email);
                Cmd.Parameters.AddWithValue("@Address", newUser.Address);
                Cmd.Parameters.AddWithValue("@Dept", newUser.Dept);
                Cmd.Parameters.AddWithValue("@Contact", newUser.Contact);
                Cmd.Parameters.AddWithValue("@DOB", DOB.Date.ToString("yyyy-MM-dd"));
                Cmd.Parameters.AddWithValue("@DOJ", DOJ.Date.ToString("yyyy-MM-dd"));
                Cmd.Parameters.AddWithValue("@Salary", newUser.Salary);
                Cmd.ExecuteNonQuery();

                _log.Info("SQL Retrieve Create new employee method stop");
                return true;
            }
            catch (Exception e)
            {
                _log.Error("SQL Retrieve Create new employee exception is : " + e);
                return false;
            }
            finally
            {
                Con.Close();
                _log.Info("SQL Retrieve Create new employee stop");
            }

        }


        public bool Edit(EmployeeObject newUser)
        {
            _log.Info("SQL Retrieve CreateData start ");
            MySqlConnection Con = new MySqlConnection(Connectionstr);
            try
            {
                _log.Debug("SQL Retrieve New employee data : " + ObjectSerializer.SerializeObject(newUser));
                Con.Open();
                MySqlCommand Cmd = new MySqlCommand("editEmployee", Con);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@EmpId", newUser.EmployeeID);
                Cmd.Parameters.AddWithValue("@Name", newUser.EmployeeName);
                Cmd.Parameters.AddWithValue("@Email", newUser.Email);
                Cmd.Parameters.AddWithValue("@Address", newUser.Address);
                Cmd.Parameters.AddWithValue("@Dept", newUser.Dept);
                Cmd.Parameters.AddWithValue("@Contact", newUser.Contact);
                Cmd.Parameters.AddWithValue("@DOB", newUser.DOB);
                Cmd.Parameters.AddWithValue("@DOJ", newUser.DOJ);
                Cmd.Parameters.AddWithValue("@Salary", newUser.Salary);
                Cmd.ExecuteNonQuery();
                _log.Info("SQL Retrieve Create new employee method stop");
                return true;
            }
            catch (Exception e)
            {
                _log.Error("SQL Retrieve Create new employee exception is : " + e);
                return false;
            }
            finally
            {
                Con.Close();
                _log.Info("SQL Retrieve Create new employee mandatory stop");
            }
        }

        public bool RegisterUser(UserObject user)
        {
            _log.Info("SQL Retrieve DeleteSingleEmployeData start ");
            MySqlConnection Con = new MySqlConnection(Connectionstr);
            try
            {
                _log.Debug("SQL Retrieve DeleteSingleEmployeData id : " + ObjectSerializer.SerializeObject(user));
                Con.Open();
                MySqlCommand Cmd = new MySqlCommand("Register", Con);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@email", user.Email);
                Cmd.Parameters.AddWithValue("@pass", user.Password);
                Cmd.Parameters.AddWithValue("@name", user.Name);
                Cmd.Parameters.AddWithValue("@contact", user.Contact);
                Cmd.ExecuteNonQuery();
                return true;

            }
            catch (Exception e)
            {
                _log.Error("SQL Retrieve RegisterUser Error,the error is : " + e.Message);
                return false;
            }
            finally
            {
                Con.Close();
                _log.Info("SQL Retrieve RegisterUser mandatory stop");
            }

        }
    }

}
