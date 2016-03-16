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
        BusinessLogic BusinessLayerObj = new BusinessLogic();
        Serializer ObjectSerializer = new Serializer();    
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public bool IsUserValid(UserObject login)
        {
            _log.Info("Rest IsUserValid start");
            try
            {                
                _log.Debug("Rest IsUserValid start login data:" + ObjectSerializer.SerializeObject(login));
                
                bool Returnval = BusinessLayerObj.IsUserValid(login);
                
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

        public bool Register(UserObject register)
        {
            _log.Info("Rest service Register start:");
            try
            {               
                _log.Debug("Rest service Register data:" + ObjectSerializer.SerializeObject(register));
                bool IsRegistered = BusinessLayerObj.RegisterUser(register);
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
    }
}
