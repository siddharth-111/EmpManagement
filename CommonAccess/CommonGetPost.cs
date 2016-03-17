using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace CommonUtility
{
    public static class CommonGetPost
    {
    
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        
        public static dynamic Post(string url, dynamic info)
        {
           _log.Info("Common GetPost method start");
            try
            {
                var WbRequest = (HttpWebRequest)WebRequest.Create(url);
               _log.Debug("Common GetPost method data passed :" + new JavaScriptSerializer().Serialize(info));
                WbRequest.ContentType = @"application/json";
                WbRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(WbRequest.GetRequestStream()))
                {
                    JObject pageInfo = (JObject)JToken.FromObject(info);
                    string Json = JsonConvert.SerializeObject(pageInfo);
                    streamWriter.Write(Json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var WbResponse = (HttpWebResponse)WbRequest.GetResponse();
                using (var streamReader = new StreamReader(WbResponse.GetResponseStream()))
                {
                    JavaScriptSerializer Ser = new JavaScriptSerializer();
                    var Result = streamReader.ReadToEnd();
                    if (Result == "true" || Result == "false")
                        return Result;
                    var Data = JsonConvert.DeserializeObject(Result) as JToken;
                    return Data;
                }

            }
            catch (Exception e)
            {
               _log.Error("Error in handling the request,the exception is :" + e.Message);
                return null;
            }
            finally
            {
               _log.Info("Common GetPost mandatory stop");
            }


        }
    }
}

