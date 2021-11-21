using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace Aesc.AwesomeKits.ArgsParser
{
    public class AescArgsParser
    {
        public AescArgsParser(string[] args)
        {

        }
        public static T Parse<T>(string[] args) where T : struct, IArgsParseResult
        {
            T result = Activator.CreateInstance<T>();
            object resultObject = result;
            List<string> argsList = new List<string>(args);
            FieldInfo[] fieldInfos = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public);
            int argsLength = argsList.Count;
            foreach (var field in fieldInfos)
            {
                string fieldName = field.Name;
                Type fieldType = field.FieldType;
                bool isContains = true;
                string content = null;
                int keyIndex = -1;
                int contentIndex = -1;
                int index1 = argsList.IndexOf("-" + fieldName);
                int index2 = argsList.IndexOf("/" + fieldName);
                if (index1 == -1 && index2 == -1) isContains = false;
                else if (index1 != -1 && index2 != -1) keyIndex = index2;
                else keyIndex = index1 != -1 ? index1 : index2;
                if (isContains && argsLength > keyIndex + 1)
                {
                    contentIndex = keyIndex + 1;
                    content = argsList[contentIndex];
                }
                UnionCondition unionCondition = (UnionCondition)Attribute.GetCustomAttribute(field, typeof(UnionCondition)); ;
                UniqueCondition uniqueCondition = (UniqueCondition)Attribute.GetCustomAttribute(field, typeof(UniqueCondition)); ;
                NecessaryCondition necessaryCondition = (NecessaryCondition)Attribute.GetCustomAttribute(field, typeof(NecessaryCondition));
                if (fieldType == typeof(ArgsNamedKey))
                {
                    field.SetValue(result, isContains ? ArgsNamedKey.Contains : ArgsNamedKey.NotContains);
                    continue;
                }
                if (isContains)
                {
                    if (fieldType == typeof(int))
                        field.SetValue(resultObject, int.Parse(content));
                    else if (fieldType == typeof(float))
                        field.SetValue(resultObject, float.Parse(content));
                    else if (fieldType == typeof(bool))
                        field.SetValue(resultObject, bool.Parse(content.ToLower()));
                    else if (fieldType == typeof(string))
                    {
                        field.SetValue(resultObject, content);
                    }
                    else throw new ArgumentException();
                }
                else field.SetValue(resultObject, null);
            }
            return (T)resultObject;
        }
    }
    public interface IArgsParseResult
    {

    }
    public struct ArgsSwitchKey<T> where T : notnull
    {

    }
    public struct ArgsNamedKey
    {
        public static ArgsNamedKey Contains = new ArgsNamedKey(true);
        public static ArgsNamedKey NotContains = new ArgsNamedKey(false);
        public readonly bool isContains;
        public ArgsNamedKey(bool isContains)
        {
            this.isContains = isContains;
        }
    }
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class UnionCondition : Attribute
    {
        public readonly string positionalString;
        public UnionCondition(string positionalString)
        {
            this.positionalString = positionalString;
        }
    }
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class UniqueCondition : Attribute
    {
        public readonly string positionalString;
        public UniqueCondition(string positionalString)
        {
            this.positionalString = positionalString;
        }
    }
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class NecessaryCondition : Attribute
    {
        public readonly string positionalString;
        public NecessaryCondition(string positionalString)
        {
            this.positionalString = positionalString;
        }
    }
}
