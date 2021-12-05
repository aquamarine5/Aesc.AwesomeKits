using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Aesc.AwesomeKits
{
    public static class WebRequestLinq
    {
        public static WebResponse SendGet(this HttpWebRequest webRequest)
        {
            webRequest.Method = "GET";
            return webRequest.GetResponse();
        }
        public static WebResponse SendPost(this HttpWebRequest webRequest, string body)
        {
            webRequest.Method = "POST";
            var stream = webRequest.GetRequestStream();
            var bytes = Encoding.UTF8.GetBytes(body);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            webRequest.ContentLength = bytes.Length;
            return webRequest.GetResponse();
        }
        public static WebResponse SendPostWithFile(this HttpWebRequest webRequest, string filePath, string key)
        {
            webRequest.Method = "POST";
            var boundary = DateTime.Now.Ticks.ToString("x");
            var startBoundary = Encoding.ASCII.GetBytes($"--{boundary}\r\n");
            var endBoundary = Encoding.ASCII.GetBytes($"--{boundary}--\r\n");
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            webRequest.ContentType = $"multipart/form-data; boundary={boundary}";
            var memoryStream = new MemoryStream();
            var splitFileContent = Encoding.ASCII.GetBytes($"Content-Disposition: form-data; name=\"{key}\"; filename=\"{Path.GetFileName(filePath)}\"\r\n" +
                    "Content-Type: application/octet-stream\r\n\r\n");

            memoryStream.Write(startBoundary, 0, startBoundary.Length);
            memoryStream.Write(splitFileContent, 0, splitFileContent.Length);
            var fileBuffer = new byte[1024];
            int totalSize = 0;
            int size = fileStream.Read(fileBuffer, 0, fileBuffer.Length);
            while (size > 0)
            {
                totalSize += size;
                memoryStream.Write(fileBuffer, 0, size);
                size = fileStream.Read(fileBuffer, 0, fileBuffer.Length);
            }
            memoryStream.Write(endBoundary, 0, endBoundary.Length);
            webRequest.ContentLength = memoryStream.Length;
            return webRequest.GetResponse();
        }
        public static WebResponse SendPut(this HttpWebRequest webRequest,string body)
        {
            webRequest.Method = "PUT";
            if (body == "" || body == null)
            {
                
            }
            return webRequest.GetResponse();
        }
    }
    public static class WebResponseLinq
    {
        public static string ReadText(this WebResponse webResponse)
        {
            StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            return result;
        }
        public static JObject ReadJsonObject(this WebResponse webResponse) => JObject.Parse(webResponse.ReadText());

        public static void WriteToFile(this WebResponse webResponse, string filePath)
        {
            Stream stream = webResponse.GetResponseStream();
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
