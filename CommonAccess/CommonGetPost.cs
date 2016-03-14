﻿using System;
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
    public class CommonGetPost
    {
        Logger Wrapper = new Logger();
        public dynamic ReturnPost(string url, dynamic info)
        {
            Wrapper.Log.Info("Common GetPost method start");
            var WbRequest = (HttpWebRequest)WebRequest.Create(url);
            Wrapper.Log.Debug("Common GetPost method data passed :" + new JavaScriptSerializer().Serialize(info));
            WbRequest.ContentType = @"application/json";
            WbRequest.Method = "POST";
            try
            {
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
                    var Data = JsonConvert.DeserializeObject(Result) as JToken;
                    return Data;
                }

            }
            catch (Exception e)
            {
                Wrapper.Log.Error("Error in handling the request,the exception is :" + e.Message);

            }
            finally
            {
                Wrapper.Log.Info("Common GetPost mandatory stop");
            }
            return null;

        }
    }
}
