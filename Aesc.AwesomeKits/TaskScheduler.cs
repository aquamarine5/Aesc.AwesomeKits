using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using TaskScheduler;

namespace Aesc.AwesomeKits.ComUtil
{
    public static class TriggerLinq
    {
        public static IWeeklyTrigger Instance(this IWeeklyTrigger weeklyTrigger, int daysOfWeek, int weeklyInterval)
        {
            weeklyTrigger.DaysOfWeek = (short)daysOfWeek;
            weeklyTrigger.WeeksInterval = (short)weeklyInterval;
            return weeklyTrigger;
        }

    }
    public static class ActionLinq
    {
        public static IExecAction Instance(this IExecAction execAction, string execPath, string argument = null, string workingPath = null)
        {
            execAction.Arguments = argument;
            execAction.Path = execPath;
            execAction.WorkingDirectory = workingPath;
            return execAction;
        }
    }
    public struct RegistrationInfo : IRegistrationInfo
    {
        public string Description { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Date { get; set; }
        public string Documentation { get; set; }
        public string XmlText { get; set; }
        public string URI { get; set; }
        public object SecurityDescriptor { get; set; }
        public string Source { get; set; }
    }
    public class AescTaskScheduler
    {
        public AescTaskScheduler()
        {

        }
        private static TaskSchedulerClass taskSchedulerClass;
        private static readonly Dictionary<Type, _TASK_TRIGGER_TYPE2> typeTriggerPairs = new Dictionary<Type, _TASK_TRIGGER_TYPE2>()
        {
            [typeof(IEventTrigger)] = _TASK_TRIGGER_TYPE2.TASK_TRIGGER_EVENT,
            [typeof(IDailyTrigger)] = _TASK_TRIGGER_TYPE2.TASK_TRIGGER_DAILY,
            [typeof(IWeeklyTrigger)] = _TASK_TRIGGER_TYPE2.TASK_TRIGGER_WEEKLY,
            [typeof(IMonthlyTrigger)] = _TASK_TRIGGER_TYPE2.TASK_TRIGGER_MONTHLY,
            [typeof(IMonthlyDOWTrigger)] = _TASK_TRIGGER_TYPE2.TASK_TRIGGER_MONTHLYDOW,
            [typeof(IIdleTrigger)] = _TASK_TRIGGER_TYPE2.TASK_TRIGGER_IDLE
        };
        private static readonly Dictionary<Type, _TASK_ACTION_TYPE> typeActionPairs = new Dictionary<Type, _TASK_ACTION_TYPE>()
        {
            [typeof(IExecAction)] = _TASK_ACTION_TYPE.TASK_ACTION_EXEC
        };
        static TaskSchedulerClass GetClass()
        {
            if (taskSchedulerClass != null) return taskSchedulerClass;
            taskSchedulerClass = new TaskSchedulerClass();
            taskSchedulerClass.Connect();
            return taskSchedulerClass;
        }
        public static IRegisteredTaskCollection GetTasks(string folderPath = "\\")
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
        public static _TASK_TRIGGER_TYPE2 ParseTrigger<TTrigger>() where TTrigger : ITrigger
        {
            if (!typeTriggerPairs.TryGetValue(typeof(TTrigger), out _TASK_TRIGGER_TYPE2 value))
                throw new ArgumentException();
            return value;
        }
        public static _TASK_ACTION_TYPE ParseAction<TAction>() where TAction : IAction
        {
            if (!typeActionPairs.TryGetValue(typeof(TAction), out _TASK_ACTION_TYPE value))
                throw new ArgumentException();
            return value;
        }
        /// <summary>
        /// 创建一个新的计划任务。<br/>
        /// 关于建立计划任务的操作方法：
        /// <para>Step 1/4: 使用此方法建立 <see cref="ITaskDefinition"/></para>
        /// <para>Step 2/4: 为 <paramref name="trigger"/> 赋值设置执行计划的条件</para>
        /// <para>Step 3/4: 为 <paramref name="action"/> 赋值设置执行计划的任务</para>
        /// <para>Step 4/4: 运行 <see cref="SaveTask"/> 以保存计划任务</para>
        /// 关于创建计划任务的其他方法，请见实例化 <see cref="AescTaskScheduler"/>
        /// </summary>
        /// <seealso cref="SaveTask"/>
        /// <returns></returns>
        public static ITaskDefinition CreateTask<TTrigger, TAction>
            (out TTrigger trigger, out TAction action, IRegistrationInfo registrationInfo = null)
            where TTrigger : ITrigger
            where TAction : IAction
        {
            TaskSchedulerClass taskScheduler = GetClass();
            ITaskDefinition task = taskScheduler.NewTask(0);
            if (registrationInfo != null) task.RegistrationInfo = registrationInfo;
            try
            {
                trigger = (TTrigger)task.Triggers.Create(ParseTrigger<TTrigger>());
                action = (TAction)task.Actions.Create(ParseAction<TAction>());
                return task;
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException();
            }
        }
        /// <summary>
        /// Step 4/4: 保存计划任务。
        /// <br/><br/><b>在调用此方法保存计划任务之前，请确保已对<see cref="ITrigger"/>和<see cref="IAction"/>所对的值赋值。<br/>
        /// 详情请见<see cref="CreateTask"/></b>
        /// </summary>
        /// <param name="task"></param>
        public static IRegisteredTask SaveTask(string path, string taskName, ITaskDefinition task, bool isExistsUpdate = true)
        {
            var taskClass = GetClass();
            ITaskFolder folder = taskClass.GetFolder(path);
            IRegisteredTask registeredTask = folder.RegisterTaskDefinition
                (taskName, task,
                (int)(isExistsUpdate ? _TASK_CREATION.TASK_CREATE_OR_UPDATE : _TASK_CREATION.TASK_CREATE),
                null, null, _TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN);
            return registeredTask;
        }
    }
}
