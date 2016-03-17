using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using DataObject;
using log4net;


namespace Rest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEmployeeManagementService" in both code and config file together.
    [ServiceContract]
    public interface IEmployeeManagementService
    {

        #region Get Methods


        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetEmployeeList/")]
        List<Employee> Retrieve(int PageSize, int CurrPage, string SortField, string SortDirection, string SearchString);


        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetSingleEmployee/")]
        Employee RetrieveById(string EmployeeID);

        #endregion Get Methods

        #region Post Methods

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DeleteEmployee/")]
        bool Delete(string EmpId);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "CreateEmployee/")]
        bool Create(Employee employee);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "EditEmployee/")]
        bool Edit(Employee employee);


        #endregion Post Methods
     
    }
}
