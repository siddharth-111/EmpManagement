using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using log4net;

namespace CommonUtility
{
    public static class Log
    {
     
      public static string SerializeObject(object objectToBeSerialized)
      {
          return new JavaScriptSerializer().Serialize(objectToBeSerialized);
      }
    }
}
