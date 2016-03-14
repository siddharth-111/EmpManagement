using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BLL;

namespace Rest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Authentication" in code, svc and config file together.
    public class Authentication : IAuthentication
    {
        BusinessLogic BusinessLayerObj = new BusinessLogic();
       

        public bool IsUserValid(string password, string username)
        {
            try {

                dynamic login = new
                {
                    username = username,
                    password = password
                };

                bool Returnval = BusinessLayerObj.IsUserValid(login);
                return Returnval;
            }
            catch (Exception e) {

                Console.Write(e);
            }
            return false;


        }

        public bool Register(string username, string password, string name, string contact)
        {
            dynamic newUser = new
            {
                  username = username,
                  password = password,
                  name = name,
                  contact = contact
            };

            bool IsRegistered = BusinessLayerObj.RegisterUser(newUser);
            return IsRegistered;
        }
    }
}
