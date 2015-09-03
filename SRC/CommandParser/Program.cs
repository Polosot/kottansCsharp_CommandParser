using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandParser
{
    class Program
    {
        private enum actions { help, kv, ping, print, error };
        private static Dictionary<string, actions> keys = new Dictionary<string, actions>
                {
                    {"/?", actions.help},
                    {"/help", actions.help},
                    {"-help", actions.help},
                    {"-k", actions.kv},
                    {"-ping", actions.ping},
                    {"-print", actions.print},
                };

        static private bool Process(actions AAction, List<string> AParameters) // return true to exit program
        {
            switch (AAction)
            {
                case actions.help:
                    Console.WriteLine("CommandParser.exe [/?] [/help] [-help] [-k key value] [-ping] [-print <print a value>]");
                    return true;
                case actions.kv:
                    for (int i = 0; i < AParameters.Count; i += 2)
                    {
                        Console.WriteLine("{0} - {1}", AParameters[i],
                            i + 1 < AParameters.Count ? AParameters[i + 1] : "null");
                    }
                    return false;
                case actions.ping:
                    Console.WriteLine("Pinging...");
                    Console.Beep();
                    return false;
                case actions.print:
                    Console.WriteLine(String.Join(" ", AParameters));
                    return false;
                case actions.error:
                    Console.WriteLine("Command <{0}> is not supported, use CommandParser.exe /? to see set of allowed commands",
                        String.Join(" ", AParameters));
                    return true;
                default:
                    return true;
            }
        }

        private static void Parse(string[] args)
        {
            int index = 0;
            while (index < args.Length)
            {
                // Choosing action. If there is no correct key - generate 'error' action
                actions action;
                if (keys.TryGetValue(args[index].ToLower(), out action))
                    index++;
                else
                    action = actions.error;
                // Creating parameters' list
                List<string> parameters = new List<string>(20);
                while (index < args.Length)
                    if (keys.ContainsKey(args[index].ToLower()))
                        break;
                    else
                        parameters.Add(args[index++]);
                // Processing action. If method returns true - exit
                if (Process(action, parameters)) return;
            }
        }

        static void Main(string[] args)
        {
            if (args.Count() == 0)
                Process(actions.help, null);
            else
                Parse(args);
        }
    }
}
