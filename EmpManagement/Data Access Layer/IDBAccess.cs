using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmpManagement.Data_Access_Layer
{
    interface IDBAccess<out T>
    {
       T getData();
         T saveData();
        T updateData();
        T deleteData();
    }
}
