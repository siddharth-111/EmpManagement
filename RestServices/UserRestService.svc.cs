using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BLL;

namespace RestServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeRestService" in code, svc and config file together.
    public class UserRestService : IUserRestService
    {
        BusinessLogic BusinessLayerObj = new BusinessLogic();
        public List<DataObject> GetUserList()
        {
            var ReturnList = new List<dynamic>();
            List<DataObject> UserList = new List<DataObject>();
            ReturnList = BusinessLayerObj.GetUserList();
            foreach (dynamic Value in ReturnList) {
                UserList.Add(new DataObject {
                    Username = Value.GetType().GetProperty("username").GetValue(Value, null),   
                    Password = Value.GetType().GetProperty("password").GetValue(Value,null)
                });
            }
            return UserList;
        }

        public bool IsUserValid(string user,string pass)
        {
            DataObject login = new DataObject
            {
                Username = user,
                Password = pass
            };
            
            bool Returnval = BusinessLayerObj.IsUserValid(login);
            return Returnval;
        
       
        }
    }
}
