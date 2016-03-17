using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using DataObject;

namespace Rest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAuthentication" in both code and config file together.
    [ServiceContract]
    public interface IAuthentication
    {
        #region Post Methods
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "IsUserValid/")]
        bool IsUserValid(User login);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "Register/")]
        bool Register(User register);
        #endregion Post Methods
       
    }
}
