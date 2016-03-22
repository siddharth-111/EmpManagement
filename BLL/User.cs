using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataObject;
using log4net;
using DataLayer;
using CommonUtility;
using System.Reflection;

namespace BusinessLayer
{
   public class User
   {

       #region Fields

       private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

       MySQLUser MySQLUser = new MySQLUser();   

       #endregion Fields

       #region Post Methods

       public bool IsUserValid(DataObject.User user)
       {
           _log.Info("IsUserValid method start ");
           try
           {
               _log.Debug("IsUserValid Login Details : " + Log.SerializeObject(user));
               bool ValidUser = MySQLUser.IsUserValid(user);
               return ValidUser;
           }
           catch (Exception e)
           {
               _log.Error("IsUserValid exception,the exception is :" + e.Message);
               return false;
           }
           finally
           {
               _log.Info("IsUserValid method stop");
           }

       }

       public bool Register(DataObject.User user)
       {
           _log.Info("Register method start");
           try
           {
               _log.Debug("Register data is :" + Log.SerializeObject(user));
               bool IsUserRegistered = MySQLUser.Register(user);
               _log.Debug(" Register returned data is :" + IsUserRegistered);
               return IsUserRegistered;
           }
           catch (Exception e)
           {
               _log.Error("RegisterUser error:" + e.Message);
               return false;
           }
           finally
           {
               _log.Info("Register method stop");
           }

       }

       #endregion Post Methods
       
    }
}
