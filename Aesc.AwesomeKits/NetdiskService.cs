using System;
using System.Collections.Generic;
using System.Text;
using Qiniu.Storage;
using Qiniu.Util;
using Qiniu.Http;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Aesc.AwesomeKits
{
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
    public class Huang111Netdisk : INetdiskDownloader
    {
        public string ParseUrl(string key) =>
            WebRequest.CreateHttp($"https://pan.huang1111.cn/api/v3/share/download/{key}").SendPut(null).ReadJsonObject()["data"].ToString();

        public void Download(string key, string filepath) =>
            WebRequest.CreateHttp(ParseUrl(key)).SendGet().WriteToFile(filepath);
    }
    public class SMMSImageHost : IImageHost
    {
        string token;
        public SMMSImageHost(string username, string password)
        {
            token = WebRequest.CreateHttp("https://sm.ms/api/v2/token").SendPost(@$"username:{username}
password: {password}").ReadJsonObject()["data"]["token"].ToString();
        }
        public void Upload(string imageFilepath)
        {
            var httpRequest = WebRequest.CreateHttp("https://sm.ms/api/v2/upload");
            httpRequest.Headers.Add("Authorization", token);
            httpRequest.Timeout = 120000;
            try
            {
                httpRequest.SendPostWithFile(imageFilepath, "smfile").ReadJsonObject();
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Status.ToString());
            }
        }
    }
    public class QiniuNetdisk : INetdiskDownloader, INetdiskUploader
    {
        Mac mac;
        public QiniuNetdisk(string accessKey, string secretKey)
        {
            mac = new Mac(accessKey, secretKey);
        }
        public void Download(string data, string filepath)
        {

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
