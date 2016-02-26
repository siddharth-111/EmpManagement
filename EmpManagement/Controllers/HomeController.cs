using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using EmpManagement.Models;
using EmpManagement.Business_Layer;
using System.Data;
using log4net;
using System.Diagnostics;
using System.Web.Security;

namespace EmpManagement.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        BusinessLogic businessLayerObj = new BusinessLogic();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            return View();
        }

        // POST : /Home/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginDetails login)
        {

            log.Info("Login Method Start");
            try
            {
                if (ModelState.IsValid)
                {
                    BusinessLogic businessLayerObj = new BusinessLogic();
                    bool isValid = businessLayerObj.isUserValid(login);

                    log.Info("Login Method Stop");
                    if (isValid)
                    {
                        FormsAuthentication.RedirectFromLoginPage(login.username,false);
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                        ModelState.AddModelError("", "Invalid username and/or password");
                }
            }
            catch (DataException ex)
            {
                //Log the error 
                log.Error("Error in logging in,the error is : " + ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(login);

        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(LoginDetails newUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BusinessLogic businessLayerObj = new BusinessLogic();
                    bool isValid = businessLayerObj.Register(newUser);
                    if (isValid)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                        ModelState.AddModelError("", "Cannot register,Duplicate username/email");
                }
            }
            catch (DataException ex)
            {
                //Log the error 
                log.Error("Error in logging in,the error is : " + ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(newUser);
                  
        }

    }
}
