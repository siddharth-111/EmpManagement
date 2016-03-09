using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using EmpManagement.Models;
using System.Data;
using log4net;
using BLL;
using System.Diagnostics;
using System.Web.Security;

namespace EmpManagement.Controllers
{
    public class HomeController : Controller
    {
        
        // GET: /Home/
        BusinessLogic BusinessLayerObj = new BusinessLogic();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            return View();
        }

        // POST : /Home/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Login login)
        {

            log.Info("Login Method Start");
            try
            {
                if (ModelState.IsValid)
                {
                    dynamic LoginDetails = login;
                    bool IsValid = BusinessLayerObj.IsUserValid(LoginDetails);

                    log.Info("Login Method Stop");
                    if (IsValid)
                    {
                        log.Info("Login Method Stop successful,The details are :" +login);
                        FormsAuthentication.RedirectFromLoginPage(login.username, false);
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                        log.Info("Login Method Stop, Invalid login details,The details are :" + login);
                        ModelState.AddModelError("", "Invalid username and/or password");
                }
            }
            catch (Exception ex)
            {
                //Log the error 
                log.Error("Error in logging in,the error is : " + ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(login);

        }

        //  GET: /Register
        public ActionResult Register()
        {
            log.Info("Get Register called ");
            return View();
        }


        // POST : /Register
        [HttpPost]
        public ActionResult Register(Register newUser)
        {
            log.Info("Post Register called , the data is :"+newUser);
            try
            {
                if (ModelState.IsValid)
                {
                    dynamic RegisterUser = newUser;
                    bool IsValid = BusinessLayerObj.RegisterUser(RegisterUser);
                    if (IsValid)
                    {
                        log.Info("Post Register method successful stop,the data is :" + newUser);
                        TempData["Success"] = "User Registered successfully!!";
                        return View(newUser);
                    }
                    else
                        log.Error("Post Register unsuccessful call, the data is : "+newUser);
                        ModelState.AddModelError("", "Cannot register,Duplicate username/email");
                }
            }
            catch (Exception ex)
            {
                //Log the error 
                log.Error("Error in logging in,the error is : " + ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(newUser);

        }

    }
}
