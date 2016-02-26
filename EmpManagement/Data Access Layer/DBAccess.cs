using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmpManagement.Models;
using System.Web;
using System.IO;
using System.Web.Mvc;


namespace EmpManagement.Data_Access_Layer
{
    public class DBAccess
    {

        public string[] getData(string path)
        {
            List<string> myCollection = new List<string>();
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(path)))
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

        public string[] getEmpData(string path, int count, int currentIndex)
        {
            List<string> myCollection = new List<string>();

            var lines = File.ReadLines(HttpContext.Current.Server.MapPath(path)).Skip(currentIndex).Take(count);
            foreach (string line in lines)
            {
                myCollection.Add(line);
            }

            return myCollection.ToArray();

        }
        public int getPageCount(string path)
        {
            var lineCount = 0;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(path)))
            {

                while ((sr.ReadLine()) != null)
                {
                    lineCount++;
                }
                sr.Close();

            }
            return lineCount;
        }
        public bool saveData(string path, string data)
        {
            using (StreamWriter sr = new StreamWriter(HttpContext.Current.Server.MapPath(path), true))
            {
                sr.WriteLine(data);
                sr.Close();
                return true;
            }
        }

        public bool updateData(string path, string data, string id)
        {
            string n = "";
            var text = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(path));
            foreach (var myString in text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!myString.Contains(id))
                {
                    n += myString + Environment.NewLine;
                }
                else
                    n += data + Environment.NewLine;
            }
            System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath(path), n);
            return true;
        }

        public string getSingleEmpData(string id, string path)
        {
            var text = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(path));
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


        public bool deleteData(string id, string path)
        {

            string n = "";
            var text = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(path));
            foreach (var myString in text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!myString.Contains(id))
                {
                    n += myString + Environment.NewLine;
                }
            }
            System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath(path), n);
            return true;
        }
    }
}