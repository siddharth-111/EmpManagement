using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using EmpManagement.Models;
using EmpManagement.Business_Layer;
using System.Data;

namespace EmpManagement.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {  
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginDetails login)
        {          
            try
            {
                if (ModelState.IsValid)
                {
                    BusinessLogic callForValidation = new BusinessLogic();
                    bool isValid = callForValidation.isUserValid(login);
                    if (isValid)
                        return RedirectToAction("Index", "Employee");
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
           
            return View(login);

        }

    }
}
