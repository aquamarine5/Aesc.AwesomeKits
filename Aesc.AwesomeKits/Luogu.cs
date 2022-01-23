using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Aesc.AwesomeKits.Net;

namespace Aesc.AwesomeKits.Net.WebStorage
{
    public class LuoguMsgPvder
    {
        public static string GetMessage(string id)
        {
            string url = $"https://www.luogu.com.cn/paste/{ id}";
            string response = WebRequest.CreateHttp(url).SendGet().ReadText();
            string jsonData = Regex.Match(response,
                "window\\._feInjection = JSON\\.parse\\(decodeURIComponent\\(\"(.+)\"\\)\\)").Value;
            var pasteContent = JObject.Parse(jsonData)["currentData"]["paste"]["data"].ToString();
            return pasteContent;
        }
    }
}
