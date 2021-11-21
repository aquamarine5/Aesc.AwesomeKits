using Aesc.AwesomeKits.ArgsParser;
using System;
#pragma warning disable 0649
namespace Aesc.AwesomeKits.Examples
{
    class ArgsParserExample
    {
        static void Main(string[] args)
        {
            Result result = AescArgsParser.Parse<Result>(args);
            Console.WriteLine(result.str);
            Console.WriteLine(result.inputInt);
            Console.WriteLine(result.testFloat);
            Console.WriteLine(result.namedkey.isContains);
            Console.WriteLine(result.bbooooll);
            Console.WriteLine(result.testNull);
        }
        public struct Result : IArgsParseResult
        {

            public string str;
            public int inputInt;
            public float testFloat;
            public ArgsNamedKey namedkey;
            public bool bbooooll;
            public string testNull;
        }
    }
}
