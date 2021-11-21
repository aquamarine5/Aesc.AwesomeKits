using System;
using System.IO;
using System.Net;

namespace Aesc.AwesomeKits.RxWebRequest
{
    public static class AescWebRequest
    {
        public static void RxGet(this WebRequest webRequest)
        {

        }
        public static string WebRequestGet(string webUrl)
        {
            HttpWebRequest webRequest = WebRequest.CreateHttp(webUrl);
            webRequest.Method = "GET";
            WebResponse webResponse = webRequest.GetResponse();
            StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            return result;
        }
        public static string WebRequestPut(string webUrl)
        {
            HttpWebRequest webRequest = WebRequest.CreateHttp(webUrl);
            webRequest.Method = "PUT";
            webRequest.Headers[HttpRequestHeader.Cookie] =
                "Hm_lvt_5583c41c8e3159d9302af01337fb1909=1636808565;" +
                " path_tmp=; Hm_lpvt_5583c41c8e3159d9302af01337fb1909=1636808682;" +
                " cloudreve-session=MTYzNjgwODY3N3xOd3dBTkVOTVExWklOVmhOVjBZeU5GZENVRFZQTmpaT1JrSlJTRWt6TTBkSlJqTkJTVVpOTkU5QlNGTlRUbGhEUms1Sk0wcEJUa0U9fKJqkrr6JiA-2auw6dbHvy4amMFvjYqvPfKpNin9WuVF";
            WebResponse webResponse = webRequest.GetResponse();
            StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            return result;
        }
        public static void WebRequestDownload(string webUrl, string filePath)
        {
            HttpWebRequest webRequest = WebRequest.CreateHttp(webUrl);
            webRequest.Method = "GET";
            WebResponse webResponce = webRequest.GetResponse();
            Stream stream = webResponce.GetResponseStream();
            if (File.Exists(filePath))
                File.Delete(filePath);
            Console.WriteLine(filePath);
            FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            byte[] byteSuffer = new byte[1024];
            int totalSize = 0;
            int size = stream.Read(byteSuffer, 0, byteSuffer.Length);
            while (size > 0)
            {
                totalSize += size;
                fileStream.Write(byteSuffer, 0, size);
                size = stream.Read(byteSuffer, 0, byteSuffer.Length);
            }
            fileStream.Flush();
            fileStream.Close();
        }
    }

}
