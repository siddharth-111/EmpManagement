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

namespace EmpManagement.Controllers
{
    public class HomeController : Controller
    {

        // GET: /Home/
        string TemplateUrl = ConfigurationManager.AppSettings["AuthServiceURL"];
      
        private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {        
            return View();
        }

        // POST : /Home/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Login login)
        {

            _Log.Info("Login Method Start");
            try
            {

                if (ModelState.IsValid)
                {

                    var Url = TemplateUrl + "IsUserValid/";
                    var WbRequest = (HttpWebRequest)WebRequest.Create(Url);
                    WbRequest.ContentType = @"application/json";
                    WbRequest.Method = "POST";
                    using (var streamWriter = new StreamWriter(WbRequest.GetRequestStream()))
                    {
                        JObject createEmp = (JObject)JToken.FromObject(login);
                        string Json = JsonConvert.SerializeObject(createEmp);                   
                        streamWriter.Write(Json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var WbResponse = (HttpWebResponse)WbRequest.GetResponse();
                    using (var streamReader = new StreamReader(WbResponse.GetResponseStream()))
                    {
                        var Result = streamReader.ReadToEnd();                       
                        var JsonResult = JObject.Parse(Result);
                        bool IsValid = (bool)JsonResult.SelectToken("IsUserValidResult");
                        if (IsValid)
                        {
                            _Log.Debug("Login Method Stop successful,The details are Username:" + login.username + " and Password:" + login.password);
                            FormsAuthentication.RedirectFromLoginPage(login.username, false);
                            return RedirectToAction("Index", "Employee");
                        }
                        else
                        {
                            _Log.Info("Login Method Stop, Invalid login details,The details are Username:" + login.username + " and Password:" + login.password);
                            ModelState.AddModelError("", "Invalid username and/or password");
                        }
                    }
                    }
                        
                  
            }
            catch (Exception ex)
            {
                //Log the error 
                _Log.Error("Error in logging in,the error is : " + ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(login);

        }

        //  GET: /Register
        public ActionResult Register()
        {
            _Log.Info("Get Register called ");
            return View();
        }


        // POST : /Register
        [HttpPost]
        public ActionResult Register(Register newUser)
        {
            _Log.Info("Post Register called , the data is  Username:" + newUser.name+ ",password:"+newUser.password+",Contact:"+newUser.phone);
            try
            {
                if (ModelState.IsValid)
                {

                    var Url = TemplateUrl + "Register/";
                    var WbRequest = (HttpWebRequest)WebRequest.Create(Url);
                    WbRequest.ContentType = @"application/json";
                    WbRequest.Method = "POST";
                    using (var streamWriter = new StreamWriter(WbRequest.GetRequestStream()))
                    {
                        JObject createEmp = (JObject)JToken.FromObject(newUser);
                        string Json = JsonConvert.SerializeObject(createEmp);
                        streamWriter.Write(Json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var WbResponse = (HttpWebResponse)WbRequest.GetResponse();
                    using (var streamReader = new StreamReader(WbResponse.GetResponseStream()))
                    {
                        var Result = streamReader.ReadToEnd();
                        var JsonResult = JObject.Parse(Result);
                        bool IsValid = (bool)JsonResult.SelectToken("RegisterResult");
                        if (IsValid)
                    {
                        _Log.Info("Post Register successful stop , the data is  Username:" + newUser.name+ ",password:"+newUser.password+",Contact:"+newUser.phone);
                        TempData["Success"] = "User Registered successfully!!";
                        return View(newUser);
                    }
                    else{
                        _Log.Error("Post Register successful stop , the data is  Username:" + newUser.name + ",password:" + newUser.password + ",Contact:" + newUser.phone);
                        ModelState.AddModelError("", "Cannot register,Duplicate username/email");
                    }
                    }

                    }
                                                                                                                                                                                                                                                                                                                                                                                                     
            }
            catch (Exception ex)
            {
                //Log the error 
                _Log.Error("Error in logging in,the error is : " + ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(newUser);

        }

    }
}
