using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtility
{
    public class Logger
    {
      private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      public log4net.ILog Log{ get { return _Log; } }
     
    }
}
