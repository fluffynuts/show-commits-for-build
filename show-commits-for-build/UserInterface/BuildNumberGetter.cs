using System.Linq;

namespace ShowCommitsForBuild.UserInterface
{
    public interface IBuildNumberGetter
    {
        int GetBuildNumberFromArgsOrUserInput();
    }

    public class BuildNumberGetter : IBuildNumberGetter
    {
        private readonly IConsole _console;
        private readonly string[] _args;

        public BuildNumberGetter(IConsole console, string[] programArgs)
        {
            _console = console;
            _args = programArgs;
        }
        public int GetBuildNumberFromArgsOrUserInput()
        {
            var selected = GrokBuildNumberFrom(_args);
            while (selected == 0)
            {
                selected = GrokBuildNumberFrom(GetBuildNumberFromUser());
            }
            return selected;
        }

        private string GetBuildNumberFromUser()
        {
            return _console.Prompt("Please enter a build number:");
        }

        private static int GrokBuildNumberFrom(params string[] args)
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