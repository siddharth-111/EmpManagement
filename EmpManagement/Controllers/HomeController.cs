using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using EmpManagement.Models;
using EmpManagement.Business_Layer;

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

        

        public ActionResult Save([Bind(Include = "username,password")] LoginDetails login)
        {
            BusinessLogic callForValidation = new BusinessLogic();
            bool isValid = callForValidation.isUserValid(login);
            if (isValid)
               return RedirectToAction("Index", "Employee");
            else
               return RedirectToAction("Index", "Home");

        }

    }
}
