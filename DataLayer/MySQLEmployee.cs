using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtility;
using System.Configuration;
using DataObject;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataLayer
{
    public class MySQLEmployee
    {

        #region Fields

        string Connectionstr = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion Fields

        #region Get Methods

        public List<Employee> Retrieve(string searchString, string sortDirection, string sortField, int currPage, int pageSize)
        {
            _log.Info("Retrieve  start : ");
            List<Employee> Employeelist = new List<Employee>();
            if (searchString == null)
            {
                MySqlConnection Con = new MySqlConnection(Connectionstr);
                try
                {
                    Con.Open();
                    MySqlCommand Cmd = new MySqlCommand("CALL udsp_Employee_Retrieve('" + sortField + "','" + sortDirection + "'," + (currPage * pageSize) + "," + (pageSize) + ",'" + searchString + "')", Con);
                    MySqlDataAdapter Sda = new MySqlDataAdapter();
                    Cmd.Connection = Con;
                    Sda.SelectCommand = Cmd;
                    DataTable Dt = new DataTable();
                    Sda.Fill(Dt);
                    foreach (DataRow row in Dt.Rows)
                    {
                        Employeelist.Add(new Employee
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

                    MySqlCommand cmdCount = new MySqlCommand("CALL udsp_Employee_getCount()", Con);
                    int Count = Convert.ToInt32(cmdCount.ExecuteScalar());
                    int PageCount = Convert.ToInt32(Math.Ceiling((double)((Count + pageSize - 1) / pageSize)));
                    Employeelist.Add(new Employee
                    {
                        Pagecount = PageCount,
                        TotalRecords = Count
                    });
                    _log.Debug("Retrieve Employee Data:" + Log.SerializeObject(Employeelist));
                    return Employeelist;


                }
                catch (Exception e)
                {
                    _log.Error("Retrieve Error,the error is : " + e.Message);

                }
                finally
                {
                    Con.Close();
                    _log.Info("Retrieve stop: ");
                }
            }
            else
            {
                MySqlConnection Con = new MySqlConnection(Connectionstr);
                try
                {
                    Con.Open();
                    MySqlCommand Cmd = new MySqlCommand("udsp_Employee_Retrieve", Con);
                   Cmd.CommandType = CommandType.StoredProcedure;
               //     MySqlCommand Cmd = new MySqlCommand("CALL udsp_Employee_Retrieve('" + sortField + "','" + sortDirection + "'," + (currPage * pageSize) + "," + (pageSize) + ",'" + searchString + "')", Con);
               //     Cmd.CommandText = "CALL udsp_Employee_Retrieve(@sortField,@sortDirection,@minIndex,@maxIndex,@searchString);";
                   Cmd.Parameters.AddWithValue("@field_name", sortField);
                   Cmd.Parameters.AddWithValue("@sortDir", sortDirection);
                   Cmd.Parameters.AddWithValue("@minIndex", (currPage * pageSize));
                   Cmd.Parameters.AddWithValue("@maxIndex", pageSize);
                   Cmd.Parameters.AddWithValue("@search", searchString);
             
                    MySqlDataAdapter Sda = new MySqlDataAdapter();
                    Cmd.Connection = Con;
                    Sda.SelectCommand = Cmd;
                    DataTable Dt = new DataTable();
                    Sda.Fill(Dt);
                    foreach (DataRow row in Dt.Rows)
                    {
                        Employeelist.Add(new Employee
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


                    MySqlCommand CmdCount = new MySqlCommand("CALL udsp_Employee_getSearchCount('" + searchString + "')", Con);
                    int Count = Convert.ToInt32(CmdCount.ExecuteScalar());
                    int PageCount = Convert.ToInt32(Math.Ceiling((double)((Count + pageSize - 1) / pageSize)));
                    Employeelist.Add(new Employee
                    {
                        Pagecount = PageCount,
                        TotalRecords = Count
                    });
                    _log.Debug("Retrieve Employee Data:" + Log.SerializeObject(Employeelist));
                    return Employeelist;


                }
                catch (Exception e)
                {
                    _log.Error("Retrieve Error,the error is : " + e.Message);
                    return null;
                }
                finally
                {
                    Con.Close();
                    _log.Info("Retrieve stop: ");
                }
            }
            return null;
        }

        public DataTable RetrieveById(string id)
        {
            _log.Info(" RetrieveById start : ");
            Employee SingleEmployee = new Employee();
            MySqlConnection Connection = new MySqlConnection(Connectionstr);
            try
            {
                Connection.Open();

                MySqlCommand Command = new MySqlCommand("CALL udsp_Employee_RetrieveById('" + id + "')", Connection);

                MySqlDataAdapter SqlAdapter = new MySqlDataAdapter();

                Command.Connection = Connection;

                SqlAdapter.SelectCommand = Command;

                DataTable EmployeeTable = new DataTable();

                SqlAdapter.Fill(EmployeeTable);
                
                _log.Debug("RetrieveById Employee Data:" + Log.SerializeObject(SingleEmployee));

                return EmployeeTable;

            }
            catch (Exception e)
            {
                _log.Error("RetrieveById Error,the error is : " + e.Message);
                return null;

            }
            finally
            {
                Connection.Close();
                _log.Info("RetrieveById stop: ");
            }

        }

        #endregion Get Methods

        #region Post Methods

        public bool Delete(string id)
        {
            _log.Info("Delete start ");
            MySqlConnection Con = new MySqlConnection(Connectionstr);
            try
            {
                _log.Debug("Delete id : " + id);

                Con.Open();

                MySqlCommand Cmd = new MySqlCommand();

                Cmd.Connection = Con;

                Cmd.CommandText = "CALL udsp_Employee_Delete('" + id + "')";

                Cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception e)
            {
                _log.Error("Delete Error,the error is : " + e.Message);

                return false;
            }
            finally
            {
                Con.Close();

                _log.Info("Delete stop");
            }

        }

        public bool Create(Employee newUser)
        {
            _log.Info("Create start ");
            MySqlConnection Con = new MySqlConnection(Connectionstr);
            try
            {
                Con.Open();
                DateTime DOJ = DateTime.Parse(newUser.DOJ);
                DateTime DOB = DateTime.Parse(newUser.DOB);
                _log.Debug("Create New employee data : " + Log.SerializeObject(newUser));

                MySqlCommand Cmd = new MySqlCommand();
                Cmd.Connection = Con;
                Cmd.CommandText = "CALL udsp_Employee_Create(@EmployeeID,@Name,@Email,@Address,@Dept,@Contact,@DOB,@DOJ,@Salary);";
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
         
                return true;
            }
            catch (Exception e)
            {
                _log.Error("Create exception is : " + e);
                return false;
            }
            finally
            {
                Con.Close();
                _log.Info("Create stop");
            }

        }

        public bool Edit(Employee newUser)
        {
            _log.Info("Edit start ");
            MySqlConnection Con = new MySqlConnection(Connectionstr);
            try
            {
                _log.Debug("SQL Retrieve New employee data : " + Log.SerializeObject(newUser));
                Con.Open();
                MySqlCommand Cmd = new MySqlCommand("udsp_Employee_Edit", Con);
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
                return true;
            }
            catch (Exception e)
            {
                _log.Error("Edit exception is : " + e);
                return false;
            }
            finally
            {
                Con.Close();
                _log.Info("Edit Stop");
            }
        }

        #endregion Post Methods

    }
}
