using System;
using System.Collections.Generic;
using System.Text;
using Qiniu.Storage;
using Qiniu.Util;
using Qiniu.Http;
using System.IO;

namespace Aesc.AwesomeKits.NetdiskService
{
    public class AescNetdiskService
    {
        public static void Download()
        {

        }
        public static void Upload(string filename)
        {
            Mac mac = new Mac("S5QEg5KyY9vZ__hm7UPTqAU9wSmBPnGx_9z9RGNU", "JGukVifLvx--kmdTihJGFyKbvop89EqKzZUIl1wS");
            Config config = new Config() { 
                Zone=Zone.ZONE_CN_South,
                UseCdnDomains=true,
                UseHttps=true
            };
            UploadManager uploadManager = new UploadManager(config);
            PutPolicy putPolicy = new PutPolicy()
            {
                Scope = "awesomebucket",
                
            };
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            putPolicy.SetExpires(3600);
            PutExtra putExtra = new PutExtra()
            {
                Version = "v2",
                PartSize=4*1024*1024
            };
            Console.WriteLine(filename);
            Console.WriteLine(Path.GetFileName(filename));
            var result=uploadManager.UploadFile(filename, Path.GetFileName(filename), token, putExtra);
            Console.WriteLine(result.Code);
        }
    }
}
