using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DataObject
{
    [DataContract]
    public class User
    {   
            [DataMember]
            public string Email { get; set; }

            [DataMember]
            public string Password { get; set; }

            [DataMember]
            public string Name { get; set; }

            [DataMember]
            public string Contact { get; set; }


     }
    
}
