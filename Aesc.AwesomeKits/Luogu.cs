using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Aquc.AquaKits.Net;

namespace Aquc.AquaKits.Net.WebStorage
{
    public class LuoguMsgPvder
    {
        public static string GetMessage(string id)
        {
            string url = $"https://www.luogu.com.cn/paste/{ id}";
            string response = WebRequest.CreateHttp(url).SendGet().ReadText();
            string jsonData = Regex.Match(response,
                "window\\._feInjection = JSON\\.parse\\(decodeURIComponent\\(\"(.+)\"\\)\\)").Groups[1].Value;
            var pasteContent = JObject.Parse(HttpUtility.UrlDecode(jsonData))["currentData"]["paste"]["data"].ToString();
            return pasteContent;
        }
    }
}
