using System;
using System.Collections.Generic;
using System.Text;
using Aesc.AwesomeKits.Util;
using Aesc.AwesomeKits.Net.WebStorage;
using Aesc.AwesomeKits.Net;

namespace Aesc.AwesomeKits.Examples
{
    class NetdiskServiceExample
    {
        public static void Main(string[] args)
        {
            var disk = new Huang1111Netdisk();
            disk.Upload("C:\\Users\\44319\\Downloads\\genshin-gacha-export.zip");
        }
    }
}
