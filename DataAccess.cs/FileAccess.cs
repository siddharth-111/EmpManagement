using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;

namespace DataAccess.cs
{
    public class DataLayer
    {
        //Get userdata from user file
        public string[] getUserData()
        {
            List<string> myCollection = new List<string>();
            using (StreamReader sr = new StreamReader("~/Users.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    myCollection.Add(line);
                }
                sr.Close();
                return myCollection.ToArray();
            }
        }

        //Get employee date from employee file
        public string[] getEmpData()
        {
            List<string> myCollection = new List<string>();
            using (StreamReader sr = new StreamReader("~/Employees.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    myCollection.Add(line);
                }
                sr.Close();
                return myCollection.ToArray();
            }



        }

        ////Get the total number of rows in file
        //public int getPageCount(string path)
        //{
        //    var lineCount = 0;
        //    using (StreamReader sr = new StreamReader()
        //    {

        //        while ((sr.ReadLine()) != null)
        //        {
        //            lineCount++;
        //        }
        //        sr.Close();

        //    }
        //    return lineCount;
        //}

        //Save Data in file
        public bool saveData(string data, string email)
        {

            var text = System.IO.File.ReadAllText("~/Employees.txt");
            foreach (var myString in text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (myString.Contains(email))
                {
                    return false;
                }

            }
            using (StreamWriter sr = new StreamWriter("~/Employees.txt", true))
            {
                sr.WriteLine(data);
                sr.Close();
                return true;
            }
        }
        //Update data in file
        public bool updateData(string data, string id)
        {
            string n = "";
            var text = System.IO.File.ReadAllText("~/Employees.txt");
            foreach (var myString in text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!myString.Contains(id))
                {
                    n += myString + Environment.NewLine;
                }
                else
                    n += data + Environment.NewLine;
            }
            System.IO.File.WriteAllText("~/Employees.txt", n);
            return true;
        }

        //Get single employee data
        public string getSingleEmpData(string id)
        {
            var text = System.IO.File.ReadAllText("~/Employees.txt");
            string empData = "";
            foreach (var myString in text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (myString.Contains(id))
                {
                    empData += myString + Environment.NewLine;
                }

            }
            return empData;
        }


        //Delete data from file
        public bool deleteData(string id)
        {

            string n = "";
            var text = System.IO.File.ReadAllText("~/Employees.txt");
            foreach (var myString in text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!myString.Contains(id))
                {
                    n += myString + Environment.NewLine;
                }
            }
            System.IO.File.WriteAllText("~/Employees.txt", n);
            return true;
        }
    }
}
