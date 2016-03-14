using System;
using System.Web.Mvc;
using System.Web.Security;
using BLL;
using EmpManagement.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Script.Serialization;
using CommonUtility;

namespace EmpManagement.Controllers
{
    public class HomeController : Controller
    {

        // GET: /Home/
        string TemplateUrl = ConfigurationManager.AppSettings["AuthServiceURL"];
        CommonGetPost ApiCall = new CommonGetPost();
        Logger Wrapper = new Logger();
       // private static readonly log4net.ILog Wrapper.Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {        
            return View();
        }

        // POST : /Home/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Login login)
        {

            Wrapper.Log.Info("Login Method Start");
            try
            {
                Wrapper.Log.Debug("The data passed to Login method : " + new JavaScriptSerializer().Serialize(login));
                if (ModelState.IsValid)
                {

                    var Url = TemplateUrl + "IsUserValid/";
                    var Data = ApiCall.ReturnPost(Url, login);

                    bool IsValid = (bool)Data.SelectToken("IsUserValidResult");
                    if (IsValid)
                    {
                        Wrapper.Log.Debug("The user is valid , the returned data is: " + IsValid);
                        Wrapper.Log.Info("Login method stop");
                        FormsAuthentication.RedirectFromLoginPage(login.username, false);
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        Wrapper.Log.Info("Login Method Stop, Invalid login details");
                        ModelState.AddModelError("", "Invalid username and/or password");
                    }
                }

            }
            catch (Exception ex)
            {
                //Log the error 
                Wrapper.Log.Error("Error in logging in,the error is : " + ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            finally
            {
                Wrapper.Log.Info("Employee Controller login method mandatory stop");
            }

            return View(login);

        }

        //  GET: /Register
        public ActionResult Register()
        {
            Wrapper.Log.Info("Get Register called ");
            return View();
        }


        // POST : /Register
        [HttpPost]
        public ActionResult Register(Register newUser)
        {
            Wrapper.Log.Info("Post Register called , the data is  Username:" + newUser.name+ ",password:"+newUser.password+",Contact:"+newUser.phone);
            try
            {
                if (ModelState.IsValid)
                {

                    var Url = TemplateUrl + "Register/";
                    var Data = ApiCall.ReturnPost(Url, newUser);
                    bool IsValid = (bool)Data.SelectToken("RegisterResult");
                    if (IsValid)
                    {
                        Wrapper.Log.Info("Post Register successful stop , the data is  Username:" + newUser.name + ",password:" + newUser.password + ",Contact:" + newUser.phone);
                        TempData["Success"] = "User Registered successfully!!";
                        return View(newUser);
                    }
                    else
                    {

                        Wrapper.Log.Debug("Post Register unsuccessful, the returned data is +" +IsValid);
                        Wrapper.Log.Info("Post Register method stop");
                        ModelState.AddModelError("", "Cannot register,Duplicate username/email");
                    }

                }

            }
            catch (Exception ex)
            {
                //Log the error 
                Wrapper.Log.Error("Error in logging in,the error is : " + ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            finally
            {
                Wrapper.Log.Info("Register user mandatory stop");
            }
            return View(newUser);

        }

    }
}
