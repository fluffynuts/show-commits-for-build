using System;
using System.Linq;

namespace get_build_log
{
    public class BuildNumberGetter
    {
        private readonly string[] _args;

        public BuildNumberGetter(string[] programArgs)
        {
            _args = programArgs;
        }
        public int GetBuildNumberFromArgsOrUserInput()
        {
            var selected = GrokBuildNumberFrom(_args);
            while (selected == 0)
            {
                selected = GetNumberFrom(GetBuildNumberFromUser());
            }
            return selected;
        }

        private static string GetBuildNumberFromUser()
        {
            Console.Write("Please enter a build number: ");
            Console.Out.Flush();
            return Console.ReadLine()?.Trim() ?? "";
        }

        private static int GrokBuildNumberFrom(string[] args)
        {
            if (args.Length == 0)
                return 0;
            var last = args[0].Split('.').Last();
            return GetNumberFrom(last);
        }

        private static int GetNumberFrom(string str)
        {
            int result;
            int.TryParse(str, out result);
            return result;
        }
    }
}