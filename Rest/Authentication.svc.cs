using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DataObject;
using BusinessLayer;
using log4net;
using System.Reflection;
using System.Web.Script.Serialization;
using CommonUtility;

namespace Rest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Authentication" in code, svc and config file together.
    public class Authentication : IAuthentication
    {
        #region Fields

        BLUser BLUser = new BLUser();              
        
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion Fields

        #region Post Methods

        public bool IsUserValid(User login)
        {
            _log.Info("Rest IsUserValid start");
            try
            {
                _log.Debug("Rest IsUserValid start login data:" + Log.SerializeObject(login));

                bool Returnval = BLUser.IsUserValid(login);

                _log.Debug("Rest IsUserValid Returnval:" + Returnval);

                return Returnval;
            }
            catch (Exception e)
            {
                _log.Debug("Rest IsUserValid method error :" + e.Message);
                return false;
            }
            finally
            {
                _log.Info("Rest IsUserValid stop:");
            }

        }

        public bool Register(User register)
        {
            _log.Info("Rest service Register start:");
            try
            {
                _log.Debug("Rest service Register data:" + Log.SerializeObject(register));

                bool IsRegistered = BLUser.Register(register);

                _log.Debug("Rest service Register return data:" + IsRegistered);

                return IsRegistered;
            }
            catch (Exception e)
            {
                _log.Error("Rest service Register error, the exception is : " + e.Message);

                return false;
            }
            finally
            {

                _log.Info("Rest service Register stop");

            }

        }

        #endregion Post Methods        
        
    }
}
