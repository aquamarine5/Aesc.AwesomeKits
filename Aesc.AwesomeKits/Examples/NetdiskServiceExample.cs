using System;
using System.Collections.Generic;
using System.Text;
using Aquc.AquaKits.Util;
using Aquc.AquaKits.Net.WebStorage;
using Aquc.AquaKits.Net;

namespace Aquc.AquaKits.Examples
{
    /// <summary>
    /// <i>If you want to test this method, please rename this function to "Main"</i><br/><br/>
    /// Seealso: <seealso cref="AescTaskScheduler"/>
    /// </summary>
    internal class NetdiskServiceExample
    {
        public static void NetdiskServiceMain(string[] args)
        {
            var disk = new SMMSImageHost("SeGjnwWvbTZyDRSWfzuq8NFLiRd5SLna");
            disk.Upload(@"D:\Program Files\Genshin Impact\Genshin Impact Game\ScreenShot\20220113162934.png");
        }
    }
}
