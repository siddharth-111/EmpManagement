using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using DTObject;
using log4net;


namespace Rest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEmployeeManagementService" in both code and config file together.
    [ServiceContract]
    public interface IEmployeeManagementService
    {
       
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetEmployeeList/")]
        List<EmployeeObject> GetEmployeeList(int pageSize, int currPage, string sortField, string sortDirection, string searchString);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DeleteEmployee/")]
        bool DeleteEmployee(Guid EmpId);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "CreateEmployee/")]
        bool CreateEmployee(EmployeeObject employee);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "EditEmployee/")]
        bool EditEmployee(EmployeeObject employee);
   
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetSingleEmployee/")]
        EmployeeObject GetSingleEmployee(Guid EmployeeID);
 


    }
}
