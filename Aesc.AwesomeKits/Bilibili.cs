using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Aesc.AwesomeKits;
using Newtonsoft.Json.Linq;

namespace Aesc.AwesomeKits
{
    public class BiliCommit
    {
        public List<BiliReply> biliReplies;
        public string commitText;
        public BiliCommit(int id)
        {
            var commitTextJson = WebRequest.CreateHttp("https://api.vc.bilibili.com/dynamic_svr/v1/dynamic_svr/get_dynamic_detail?dynamic_id" + id)
                .SendGet().ReadJsonObject()["data"]["card"]["card"].ToString();
            commitText = JObject.Parse(commitTextJson)["item"]["content"].ToString();
            var replyJsonArray = JArray.FromObject(WebRequest.CreateHttp("https://api.bilibili.com/x/v2/reply/main?jsonp=jsonp&next=0&type=17&mode=2&plat=1&oid" + id)
                .SendGet().ReadJsonObject()["data"]["replies"].ToString());
            foreach (var replyJson in replyJsonArray)
            {
                biliReplies.Add(new BiliReply() 
                { 
                    text=replyJson["content"]["message"].ToString()
                });
            }
        }
    }
    public struct BiliReply
    {
        public string text;
    }
}
