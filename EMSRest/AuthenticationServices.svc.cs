using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BLL;

namespace EMSRest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserRestServices" in code, svc and config file together.
    public class AuthenticationServices : IAuthenticationServices
    {
        BusinessLogic BusinessLayerObj = new BusinessLogic();
        public List<DataObject> GetUserList()
        {
            var ReturnList = new List<dynamic>();
            List<DataObject> UserList = new List<DataObject>();
            ReturnList = BusinessLayerObj.GetUserList();
            foreach (dynamic Value in ReturnList)
            {
                UserList.Add(new DataObject
                {
                    username = Value.GetType().GetProperty("username").GetValue(Value, null),
                    password = Value.GetType().GetProperty("password").GetValue(Value, null)
                });
            }
            return UserList;
        }

        public bool IsUserValid(string user, string pass)
        {
            DataObject login = new DataObject
            {
                username = user,
                password = pass
            };
          
          bool Returnval = BusinessLayerObj.IsUserValid(login);
            return true;


        }
    }
}
