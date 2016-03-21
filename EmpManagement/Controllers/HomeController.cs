using System;
using System.Web.Mvc;
using System.Web.Security;
using EmployeeManagement.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.Http.WebHost;
using CommonUtility;
using log4net;
using System.Reflection;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
       
        #region Fields 
        string AuthServiceURL = ConfigurationManager.AppSettings["AuthServiceURL"];
        //HomeController Home = new HomeController();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        #endregion Fields

        #region Get Methods

        // GET: /Home/
        public ActionResult Index()
        {
            
           _log.Info("Get Index method start");
            
            try
            {
                return View();

            }
            catch (Exception e)
            {              
               _log.Error("Get Index method error :" + e.Message);
                return View();
            }
            finally
            {
               _log.Info("Get Index method stop");
            }
                                 
        }

        //  GET: /Register
        public ActionResult Register()
        {
           _log.Info( "Get Register start ");                        
            try
            {
                return View();

            }
            catch (Exception e)
            {
                _log.Error( "Get Register method error :" + e.Message);
                return View();
            }
            finally
            {
                _log.Info( "Get Register stop");
            }
                 
        }
        #endregion GetMethods

        #region Post Methods

        // POST : /Home/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Index(UserModel user)
        {

            _log.Info( "_log.in Method Start");
            try
            {
                _log.Debug( "The data passed to _log.in method : " + Log.SerializeObject(user));
                if (ModelState.IsValid)
                {

                    string Url = AuthServiceURL + "IsUserValid/";

                    _log.Debug( "The Url Passed is :" + Url);

                    var RestData = new
                    {
                        login = user
                    };
                    _log.Debug( "The Data to be passed is:" + Log.SerializeObject(RestData));

                    var Data = CommonGetPost.Post(Url, RestData);

                    bool IsValid = Convert.ToBoolean(Data);

                    if (IsValid)
                    {
                        _log.Debug( "The user is valid , the returned data is: " + IsValid); 
                
                        FormsAuthentication.RedirectFromLoginPage(user.Email, false);

                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {                   
                        ModelState.AddModelError("", "Invalid username and/or password");
                    }
                }

            }
            catch (Exception ex)
            {

                _log.Error( "Error in _log.ging in,the error is : " + ex.Message);

                ModelState.AddModelError("", "Unable to _log.in. Try again, and if the problem persists, see your system administrator.");
            }
            finally
            {

                _log.Info( "Employee Controller _log.in method mandatory stop");

            }

            return View(user);

        }



        // POST : /Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Register(RegisterModel user)
        {
            _log.Info("Post Register start");

            try
            {
                if (ModelState.IsValid)
                {
                   _log.Debug( "Post Register data is " + Log.SerializeObject(user));

                    var Url = AuthServiceURL + "Register/";

                    var RestData = new
                    {
                        register = user
                    };

                   _log.Debug("Post Register Url Passed is :" + Url);

                   _log.Debug("Post Register data is " + Log.SerializeObject(user));

                    var Data = CommonGetPost.Post(Url, RestData);

                    bool IsCreated= Convert.ToBoolean(Data);

                    if (IsCreated)
                    {

                       _log.Debug( "Post Register successful , the returned data :" + IsCreated);

                        TempData["Success"] = "User Registered successfully!!";

                        return View(user);
                    }
                    else
                    {
                       _log.Debug( "Post Register unsuccessful, the returned data is +" + IsCreated);

                        ModelState.AddModelError("", "Cannot register,Duplicate username/email");
                    }

                }

            }
            catch (Exception ex)
            {
                //_log. the error 
               _log.Error( "Error in _log.ging in,the error is : " + ex);

                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            finally
            {

               _log.Info( "Post Register user stop");

            }
            return View(user);

        }

        #endregion Post Method
              
    }
}
