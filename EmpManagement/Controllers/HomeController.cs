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

namespace EmpManagement.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "username,password")] LoginDetails login)
        {

            log.Info("Login Method Start");
            try
            {
                if (ModelState.IsValid)
                {
                    BusinessLogic callForValidation = new BusinessLogic();
                    bool isValid = callForValidation.isUserValid(login);

                    log.Info("Login Method Stop");
                    if (isValid)
                        return RedirectToAction("Index", "Employee");
                    else
                        ModelState.AddModelError("", "Invalid username and/or password");
                }
            }
            catch (DataException ex/* dex */)
            {
                log.Error("Error in logging in,the error is : " + ex);
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(login);

        }

    }
}
