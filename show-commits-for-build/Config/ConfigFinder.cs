using System.Configuration;
using PeanutButter.DuckTyping.Exceptions;
using PeanutButter.DuckTyping.Extensions;
using PeanutButter.Utils;
using ShowCommitsForBuild.Extensions;
using ShowCommitsForBuild.UserInterface;

namespace ShowCommitsForBuild.Config
{
    public class ConfigFinder : IConfigFinder
    {
        private readonly IConsole _console;

        public ConfigFinder(IConsole console)
        {
            _console = console;
        }

        public T GetConfig<T>(
            string context
        ) where T : class
        {
            try
            {
                return ConfigurationManager.AppSettings.FuzzyDuckAs<T>(
                    s => $"{context}.{s}",
                    s => StringExtensions.RegexReplace(s, $"^{context}\\.", ""),
                    true
                );
            }
            catch (UnDuckableException e)
            {
                PrintDuckErrors(context, e);
                return null;
            }
        }

        private void PrintDuckErrors(
            string context,
            UnDuckableException unDuckableException
        )
        {
            _console.WriteLine(
                unDuckableException.FormatError(
                    $"{context} improperly configured:\n* "
                )
            );
        }
    }
}