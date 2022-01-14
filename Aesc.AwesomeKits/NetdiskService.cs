using System;
using System.Collections.Generic;
using System.Text;
using Qiniu.Storage;
using Qiniu.Util;
using Qiniu.Http;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Aesc.AwesomeKits.Net.WebStorage
{
    public interface IMessageProvider
    {
        public void GetMessage(string body);
    }
    public interface IImageHost
    {
        public void Upload(string imageFilepath);
    }
    public interface INetdiskUploader
    {
        public void Upload(string filename);
    }
    public interface INetdiskDownloader
    {
        public void Download(string data, string filepath);
    }
    public class Huang1111Netdisk : INetdiskDownloader, INetdiskUploader
    {
        string cookies = "";
        public Huang1111Netdisk() { }
        public Huang1111Netdisk(string cookies)
        {
            this.cookies = cookies;
        }
        public string GetCookies()
        {
            if (cookies != "") return cookies;
            BiliCommitMsgPvder biliCommit = new BiliCommitMsgPvder("610967597602910149");
            return biliCommit.biliReplies[0].text;
        }
        public string ParseUrl(string key) =>
            WebRequest.CreateHttp($"https://pan.huang1111.cn/api/v3/share/download/{ key}").SendPut(null).ReadJsonObject()["data"].ToString();

        public void Download(string key, string filepath) =>
            WebRequest.CreateHttp(ParseUrl(key)).SendGet().WriteToFile(filepath);

        public void Upload(string filename)
        {
            string defaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 Edg/96.0.1054.62";
            string cookies = GetCookies();
            var file = new System.IO.FileInfo(filename);
            var webRequest = WebRequest.CreateHttp($"https://pan.huang1111.cn/api/v3/file/upload/credential?path=%2F&size={ file.Length }&name={ file.Name }&type=onedrive");
            webRequest.Headers[HttpRequestHeader.Cookie] = cookies;
            webRequest.Referer = "https://pan.huang1111.cn/home?path=%2F";
            webRequest.UserAgent = defaultUserAgent;
            var credentialResponce = webRequest.SendGet().ReadJsonObject()["data"];
            string policy = credentialResponce["policy"].ToString();
            if (policy == "")
            {
                var postWebRequest = WebRequest.CreateHttp("https://pan.huang1111.cn/api/v3/file/upload?chunk=0&chunks=1");
                postWebRequest.Headers[HttpRequestHeader.Cookie] = cookies;
                postWebRequest.Headers["x-cr-path"] = "/";
                postWebRequest.Headers["x-cr-filename"] = file.Name;
                postWebRequest.Referer = "https://pan.huang1111.cn/home?path=%2F";
                postWebRequest.UserAgent = defaultUserAgent;
                postWebRequest.ContentType = "application/octet-stream";
                string uploadResponce = postWebRequest.AddFile(filename).SendPost().ReadText();
                Console.WriteLine(uploadResponce);
            }
            else
            {
                var putWebRequest = WebRequest.CreateHttp(policy);
                putWebRequest.Referer = "https://pan.huang1111.cn/";
                putWebRequest.ContentType = "application/octet-stream";
                putWebRequest.UserAgent = defaultUserAgent;
                putWebRequest.Headers[HttpRequestHeader.ContentRange] = $"bytes 0-{file.Length - 1}/{file.Length}";
                string uploadResponce = putWebRequest.AddFile(filename).SendPut().ReadText();
                var finishWebRequest = WebRequest.CreateHttp(credentialResponce["token"].ToString());
                finishWebRequest.UserAgent = defaultUserAgent;
                finishWebRequest.ContentType = "text/plain;charset=UTF-8";
                finishWebRequest.Headers[HttpRequestHeader.Cookie] = cookies;
                finishWebRequest.Referer = "https://pan.huang1111.cn/home?path=%2F";
                var finishResponce = finishWebRequest.AddText(uploadResponce).SendPost().ReadJsonObject();
                Console.WriteLine(finishResponce.ToString());
            }
        }
    }
    public class SMMSImageHost : IImageHost
    {
        string token;
        public SMMSImageHost(string username, string password)
        {
            token = WebRequest.CreateHttp("https://sm.ms/api/v2/token").SendPost(@$"username:{username}
password: {password}").ReadJsonObject()["data"]["token"].ToString();
        }
        public SMMSImageHost(string token)
        {
            this.token = token;
        }
        public void Upload(string imageFilepath)
        {
            var httpRequest = WebRequest.CreateHttp("https://sm.ms/api/v2/upload");
            httpRequest.Headers.Add("Authorization", token);
            httpRequest.Timeout = 120000;
            var responce = httpRequest.AddFormdata(imageFilepath, "smfile").SendPost().ReadJsonObject();
            Console.WriteLine(responce.ToString());
        }
    }
    public class QiniuNetdisk : INetdiskUploader
    {
        Mac mac;
        public QiniuNetdisk(string accessKey, string secretKey)
        {
            mac = new Mac(accessKey, secretKey);
        }
        public void Upload(string filename)
        {
            Config config = new Config()
            {
                Zone = Zone.ZONE_CN_South,
                UseCdnDomains = true
            };
            UploadManager uploadManager = new UploadManager(config);
            PutPolicy putPolicy = new PutPolicy()
            {
                Scope = "awesomebucket"
            };
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            putPolicy.SetExpires(3600);
            PutExtra putExtra = new PutExtra()
            {
                Version = "v2",
                PartSize = 4 * 1024 * 1024
            };
            Console.WriteLine(filename);
            Console.WriteLine(Path.GetFileName(filename));
            var result = uploadManager.UploadFile(filename, Path.GetFileName(filename), token, putExtra);
            Console.WriteLine(result.Code);
        }
    }
}
