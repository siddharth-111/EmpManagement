using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonUtility;

namespace EmpManagement.Controllers
{
    public class BaseController : Controller
    {
        Logger Wrapper = new Logger();
        CommonGetPost ApiCall = new CommonGetPost();       
    }
}
