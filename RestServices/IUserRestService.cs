using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using RestServices;
using System.ServiceModel.Web;
using System.Web.Script.Services;
namespace RestServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEmployeeRestService" in both code and config file together.
    [ServiceContract]
    [ScriptService]
    public interface IUserRestService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetUserList/")]
        List<DataObject> GetUserList();
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped,ResponseFormat = WebMessageFormat.Json, UriTemplate = "IsUserValid/")]
        bool IsUserValid(string user,string pass);
    }
}
