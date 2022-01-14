using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Aesc.AwesomeKits;
using Newtonsoft.Json.Linq;

namespace Aesc.AwesomeKits.Net.WebStorage
{
    public class BiliCommitMsgPvder
    {
        public List<BiliReply> biliReplies = new List<BiliReply>();
        public string commitText;
        public BiliCommitMsgPvder(string id)
        {
            var commitTextJson = WebRequest.CreateHttp("https://api.vc.bilibili.com/dynamic_svr/v1/dynamic_svr/get_dynamic_detail?dynamic_id=" + id)
                .SendGet().ReadJsonObject()["data"]["card"]["card"].ToString();
            commitText = JObject.Parse(commitTextJson)["item"]["content"].ToString();
            var content = WebRequest.CreateHttp("https://api.bilibili.com/x/v2/reply/main?jsonp=jsonp&next=0&type=17&mode=2&plat=1&oid=" + id)
                .SendGet().ReadJsonObject()["data"]["replies"].ToString();
            Console.WriteLine(content);
            var replyJsonArray = JArray.Parse(content);
            foreach (var replyJson in replyJsonArray)
            {
                biliReplies.Add(new BiliReply()
                {
                    text = replyJson["content"]["message"].ToString()
                });
            }
        }
        public BiliCommitMsgPvder(long id) => new BiliCommitMsgPvder(id.ToString());

    }
    public struct BiliReply
    {
        public string text;
    }
}
