using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace Rest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEmployeeManagementService" in both code and config file together.
    [ServiceContract]
    public interface IEmployeeManagementService
    {
       
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetEmployeeList/")]
        List<EmployeeObject> GetEmployeeList(string searchString,string sortDirection,string sortField,int pageSize,int currPage);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DeleteEmployee/")]
        bool DeleteEmployee(Guid EmpId);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "CreateEmployee/")]
        bool CreateEmployee(Guid? EmployeeID,string EmployeeName,string Address,string DOB,int Salary,string Email,string DOJ,string Dept,string Contact);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "EditEmployee/")]
        bool EditEmployee(Guid EmployeeID,string Email,string EmployeeName,string Address, string DOB,string DOJ, string Dept , string Contact, int Salary);
   
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetSingleEmployee/")]
        EmployeeObject GetSingleEmployee(Guid EmployeeID);
 


    }
}
