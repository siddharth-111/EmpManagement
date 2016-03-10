﻿using System;
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
    }
}
