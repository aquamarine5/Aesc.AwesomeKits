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
            var disk = new SMMSImageHost("SeGjnwWvbTZyDRSWfzuq8NFLiRd5SLna");
            disk.Upload(@"D:\Program Files\Genshin Impact\Genshin Impact Game\ScreenShot\20220113162934.png");
        }
    }
}
