using System;
using System.Collections.Generic;
using System.Text;
using TaskScheduler;
using Aquc.AquaKits.ComUtil;

namespace Aquc.AquaKits.Examples
{
    internal class TaskSchedulerExample
    {
        /// <summary>
        /// <i>If you want to test this method, please rename this function to "Main"</i><br/><br/>
        /// Seealso: <seealso cref="AescTaskScheduler"/>
        /// </summary>
        public static void TaskSchedulerMain(string[] args)
        {
            Console.WriteLine(args.Length);
            Console.WriteLine(args[0]);
            Console.WriteLine(args[^1]);
            Console.ReadLine();
        }
    }
}
