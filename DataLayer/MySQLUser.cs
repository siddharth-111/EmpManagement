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
   public class MySQLUser
   {
       #region Fields
 
        string Connectionstr = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      
       #endregion Fields

       #region Methods

       public bool IsUserValid(User user)
        {
            _log.Info("IsUserValid start");
            MySqlConnection Connection = new MySqlConnection(Connectionstr);
            try
            {
                Connection.Open();

                MySqlCommand Command = new MySqlCommand("udsp_User_IsUserValid", Connection);

                Command.CommandType = CommandType.StoredProcedure;

                Command.Parameters.AddWithValue("@username", user.Email);

                Command.Parameters.AddWithValue("@Pass", user.Password);      
       
                object ReturnValue = Command.ExecuteScalar();
                          
                if((string)ReturnValue == user.Email)
                {

                    return true;

                }
                else
                {

                        return false;

                }
                    

            }
            catch (Exception e)
            {

                _log.Error("IsUserValid error is : " + e.Message);

                return false;

            }
            finally
            {

                Connection.Close();

                _log.Info("IsUserValid stop");

            }
        
        }

       public bool Register(User user)
       {
           _log.Info("SQL Retrieve DeleteSingleEmployeData start ");
           MySqlConnection Con = new MySqlConnection(Connectionstr);
           try
           {
               _log.Debug("SQL Retrieve DeleteSingleEmployeData id : " + Log.SerializeObject(user));

               Con.Open();

               MySqlCommand Cmd = new MySqlCommand("udsp_User_Register", Con);

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

       #endregion Methods

   }
}
