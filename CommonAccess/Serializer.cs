using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace CommonUtility
{
    public class Serializer
    {
        public string SerializeObject(object objectToBeSerialized)
        {
            return new JavaScriptSerializer().Serialize(objectToBeSerialized);
        }
    }
}
