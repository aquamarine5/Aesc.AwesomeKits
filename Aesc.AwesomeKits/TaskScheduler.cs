using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TaskScheduler;

namespace Aesc.AwesomeKits.TaskScheduler
{
    public interface ITaskTrigger
    {
        public _TASK_TRIGGER_TYPE2 TaskTriggerType { get; set; }
        public string Interval { get; set; }
    }
    public class TaskScheduler
    {
        static TaskSchedulerClass GetClass()
        {
            TaskSchedulerClass ts = new TaskSchedulerClass();
            ts.Connect(null, null, null, null);
            return ts;
        }
        public static IRegisteredTaskCollection GetTasks(string folderPath="\\")
        {
            TaskSchedulerClass taskScheduler = GetClass();
            ITaskFolder folder = taskScheduler.GetFolder(folderPath);
            return folder.GetTasks(1);
        }
        public static bool IsExists(string taskName)
        {
            IRegisteredTaskCollection taskCollection = GetTasks(Path.GetDirectoryName(taskName));
            string taskLastName = Path.GetFileName(taskName);
            foreach (IRegisteredTask task in taskCollection)
            {
                if (task.Name == taskLastName) return true;
            }
            return false;
        }
        // TODO
        public static _TASK_STATE CreateTask(string taskPath,_TASK_TRIGGER_TYPE2 taskTriggerType,DateTime dateTime,string author="",string description="")
        {
            TaskSchedulerClass taskScheduler = GetClass();
            ITaskFolder folder = taskScheduler.GetFolder(Path.GetDirectoryName(taskPath));
            ITaskDefinition task = taskScheduler.NewTask(0);
            task.RegistrationInfo.Author = author;
            task.RegistrationInfo.Description = description;
            ITimeTrigger timeTrigger = (ITimeTrigger)task.Triggers.Create(taskTriggerType);
            timeTrigger.StartBoundary="";
            return _TASK_STATE.TASK_STATE_DISABLED;
        }
    }
}
