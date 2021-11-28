using System;
using System.Collections.Generic;
using System.Text;
using TaskScheduler;
using Aesc.AwesomeKits.TaskScheduler;

namespace Aesc.AwesomeKits.Examples
{
    class TaskSchedulerExample
    {
        public static void lMain(string[] args)
        {
            Console.WriteLine(args.Length);
            Console.WriteLine(args[0]);
            Console.WriteLine(args[^1]);
            Console.ReadLine();
        }
    }
}
