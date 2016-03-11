using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace DataLayer
{

    public class FileAccess
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Get UserData
        public string[] GetUserData()
        {
            log.Info("File Access GetUserData start");
            List<string> Collection = new List<string>();
            using (StreamReader sr = new StreamReader("C:\\Users\\Siddharth\\Documents\\Visual Studio 2010\\Projects\\EmpManagement\\DataLayer\\User.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Collection.Add(line);
                }
                sr.Close();
                log.Info("File Access GetUserData start,The data is :" + Collection.ToArray());
                return Collection.ToArray();
            }
        }

        //Return employee data with pagination

        public dynamic GetEmployeeData(string searchString, string sortDirection, string sortField, int pageSize, int currPage)
        {
            log.Info("File Access GetEmployeeData start,the data is :searchString:" + searchString + ",sortDirection:" + sortDirection + ",sortField:" + sortField + ",pageSize:" + pageSize + ",currPage:" + currPage);
            string[] EmpData = GetEmpData();
            List<EmployeeObject> EmpList = new List<EmployeeObject>();
            if (EmpData != null)
            {
                foreach (string dataLine in EmpData)
                {
                    string[] DataItem = dataLine.Split('|');

                    EmpList.Add(new EmployeeObject
                    {
                        EmployeeID = Guid.Parse(DataItem[0]),
                        Email = DataItem[1],
                        EmployeeName = DataItem[2],
                        Address = DataItem[3],
                        Dept = DataItem[4],
                        DOJ = DateTime.Parse(DataItem[5]),
                        DOB = DateTime.Parse(DataItem[6]),
                        Contact = DataItem[7],
                        Salary = Int32.Parse(DataItem[8])
                    });
                }
                IQueryable<EmployeeObject> Emp = EmpList.AsQueryable();
                var ModEmplist = from s in Emp
                                 select s;
                if (!String.IsNullOrEmpty(searchString))
                {

                    int checkForNumber;
                    DateTime checkForDate;
                    bool resultOfDateParse = DateTime.TryParse(searchString, out checkForDate);
                    bool isNumeric = int.TryParse(searchString, out checkForNumber);

                    if (resultOfDateParse)
                    {
                        ModEmplist = ModEmplist.Where(s => DateTime.Compare(s.DOB, checkForDate) == 0 || DateTime.Compare(s.DOJ, checkForDate) == 0);

                    }
                    else if (isNumeric)
                    {
                        ModEmplist = ModEmplist.Where(s => s.Salary == Int32.Parse(searchString) || s.Contact.ToUpper().Contains(searchString.ToUpper()));
                    }
                    else
                    {
                        ModEmplist = ModEmplist.Where(s => s.EmployeeName.ToUpper().Contains(searchString.ToUpper()) || s.Address.ToUpper().Contains(searchString.ToUpper()) || s.Contact.ToUpper().Equals(searchString.ToUpper()) || s.Dept.ToUpper().Equals(searchString.ToUpper()) || s.Email.ToUpper().Contains(searchString.ToUpper()));
                    }

                }
                if (sortDirection == "ascending")
                {
                    switch (sortField)
                    {
                        case "Email":
                            ModEmplist = ModEmplist.OrderBy(s => s.Email);
                            break;
                        case "Name":
                            ModEmplist = ModEmplist.OrderBy(s => s.EmployeeName);
                            break;
                        case "Address":
                            ModEmplist = ModEmplist.OrderBy(s => s.Address);
                            break;
                        case "Department":
                            ModEmplist = ModEmplist.OrderBy(s => s.Dept);
                            break;
                        case "DOJ":
                            ModEmplist = ModEmplist.OrderBy(s => s.DOJ);
                            break;
                        case "DOB":
                            ModEmplist = ModEmplist.OrderBy(s => s.DOB);
                            break;
                        case "Salary":
                            ModEmplist = ModEmplist.OrderBy(s => s.Salary);
                            break;
                        case "Contact":
                            ModEmplist = ModEmplist.OrderBy(s => s.Contact);
                            break;
                    }
                }
                else
                {
                    switch (sortField)
                    {
                        case "Email":
                            ModEmplist = ModEmplist.OrderByDescending(s => s.Email);
                            break;
                        case "Name":
                            ModEmplist = ModEmplist.OrderByDescending(s => s.EmployeeName);
                            break;
                        case "Address":
                            ModEmplist = ModEmplist.OrderByDescending(s => s.Address);
                            break;
                        case "Department":
                            ModEmplist = ModEmplist.OrderByDescending(s => s.Dept);
                            break;
                        case "DOJ":
                            ModEmplist = ModEmplist.OrderByDescending(s => s.DOJ);
                            break;
                        case "DOB":
                            ModEmplist = ModEmplist.OrderByDescending(s => s.DOB);
                            break;
                        case "Salary":
                            ModEmplist = ModEmplist.OrderByDescending(s => s.Salary);
                            break;
                        case "Contact":
                            ModEmplist = ModEmplist.OrderByDescending(s => s.Contact);
                            break;
                    }
                }
                var EmployeeData = new List<dynamic>();
                var Count = ModEmplist.Count();
                var PageCount = Convert.ToInt32(Math.Ceiling((double)((Count + pageSize - 1) / pageSize)));
                var newEmpList = ModEmplist.ToList().Skip(currPage * pageSize).Take(pageSize);
                foreach (EmployeeObject temp in newEmpList)
                {
                    EmployeeData.Add(
                         new
                         {
                             EmployeeID = temp.EmployeeID,
                             Email = temp.Email,
                             EmployeeName = temp.EmployeeName,
                             Address = temp.Address,
                             DOB = temp.DOB.Date.ToString("d"),
                             DOJ = temp.DOJ.Date.ToString("d"),
                             Dept = temp.Dept,
                             Salary = temp.Salary,
                             Contact = temp.Contact,

                         }
                         );
                }
                EmployeeData.Add(new
                {
                    Pagecount = PageCount,
                    TotalRecords = Count
                });
                log.Info("File Access GetEmployeeData stop,the returned list is :" + EmployeeData);
                return EmployeeData;
            }
            return null;
        }

        //Get Employee data from file
        public string[] GetEmpData()
        {
            log.Info("File Access GetEmpData start");
            List<string> Collection = new List<string>();
            using (StreamReader sr = new StreamReader("C:\\Users\\Siddharth\\Documents\\Visual Studio 2010\\Projects\\EmpManagement\\DataLayer\\Employee.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Collection.Add(line);
                }
                sr.Close();
                log.Info("File Access GetEmpData stop,the data is :" + Collection.ToArray());
                return Collection.ToArray();
            }
        }



        //Save Data in file
        public bool SaveData(string data, string email,string path)
        {
            log.Info("File Access SaveData start,the data is  :" + data + ",email is:" + email);
            var FileData = System.IO.File.ReadAllText("C:\\Users\\Siddharth\\Documents\\Visual Studio 2010\\Projects\\EmpManagement\\DataLayer\\"+path);
            foreach (var myString in FileData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (myString.Contains(email))
                {
                    log.Info("File Access SaveData stop,the user is not saved,The duplicate email is :" + email);
                    return false;
                }

            }
            using (StreamWriter sr = new StreamWriter("C:\\Users\\Siddharth\\Documents\\Visual Studio 2010\\Projects\\EmpManagement\\DataLayer\\"+path, true))
            {
                sr.WriteLine(data);
                sr.Close();
                log.Info("File Access SaveData stop,the user is saved :" + data);
                return true;
            }
        }
        //Update data in file
        public bool UpdateData(string data, string id)
        {
            log.Info("File Access UpdateData stop,the data is  :" + data + ", and the id is:" + id);
            string FileData = "";
            var text = System.IO.File.ReadAllText("C:\\Users\\Siddharth\\Documents\\Visual Studio 2010\\Projects\\EmpManagement\\DataLayer\\Employee.txt");
            foreach (var myString in text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!myString.Contains(id))
                {
                    FileData += myString + Environment.NewLine;
                }
                else
                    FileData += data + Environment.NewLine;
            }
            System.IO.File.WriteAllText("C:\\Users\\Siddharth\\Documents\\Visual Studio 2010\\Projects\\EmpManagement\\DataLayer\\Employee.txt", FileData);
            log.Info("File Access UpdateData stop");
            return true;
        }

        //Get single employee data
        public string GetSingleEmpData(string id)
        {
            log.Info("File Access GetSingleEmpData start,the id is:" + id);
            var FileData = System.IO.File.ReadAllText("C:\\Users\\Siddharth\\Documents\\Visual Studio 2010\\Projects\\EmpManagement\\DataLayer\\Employee.txt");
            string EmpData = "";
            foreach (var myString in FileData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (myString.Contains(id))
                {
                    EmpData += myString + Environment.NewLine;
                }

            }
            log.Info("File Access GetSingleEmpData start,the Employee Data is:" + EmpData);
            return EmpData;
        }


        //Delete data from file
        public bool DeleteData(string id)
        {
            log.Info("File Access DeleteData start,the employee id is :" + id);
            string DeleteData = "";
            var FileData = System.IO.File.ReadAllText("C:\\Users\\Siddharth\\Documents\\Visual Studio 2010\\Projects\\EmpManagement\\DataLayer\\Employee.txt");
            foreach (var myString in FileData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!myString.Contains(id))
                {
                    DeleteData += myString + Environment.NewLine;
                }
            }
            System.IO.File.WriteAllText("C:\\Users\\Siddharth\\Documents\\Visual Studio 2010\\Projects\\EmpManagement\\DataLayer\\Employee.txt", DeleteData);
            log.Info("File Access DeleteData stop");
            return true;
        }
    }
}
