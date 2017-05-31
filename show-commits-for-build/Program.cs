using System;
using System.Configuration;
using PeanutButter.DuckTyping.Exceptions;
using PeanutButter.DuckTyping.Extensions;
using PeanutButter.Utils;

namespace ShowCommitsForBuild
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var buildNumber = new BuildNumberGetter(args).GetBuildNumberFromArgsOrUserInput();

                var tfsConfig = GetConfig<ITfsConfig>("TFS");
                var gitlabConfig = GetConfig<IGitLabConfig>("GitLab");

                var infoFinder = new BuildInfoFinder(tfsConfig);
                var buildInfo = infoFinder.GetInfoForBuildRevision(buildNumber);
                if (buildInfo == null)
                {
                    Console.WriteLine($"No build info found for #{buildNumber}");
                    Pause();
                    return 1;
                }

                var launcher = new GitLabUrlLauncher(gitlabConfig);
                launcher.ShowCommits(buildInfo.SourceVersion);

                return 0;
            }
            catch (Exception ex)
            {
                PrintFatalFailure(ex);
                Pause();
                return 13;
            }
        }

        private static void PrintFatalFailure(Exception ex)
        {
            new[]
            {
                "Ruh-roh! Something went wrong! Please report this issue to someone you thing can fix it (:",
                "(more info follows)",
                "What went wrong:",
                "----------------",
                ex.Message,
                "Where it went wrong:",
                "--------------------",
                ex.StackTrace,
                "--------------------"
            }.ForEach(s => Console.WriteLine(s));
        }

        private static void Pause()
        {
            Console.WriteLine(" ( Press any key to continue )");
            Console.ReadKey();
        }


        private static T GetConfig<T>(
            string context
        ) where T : class
        {
            try
            {
                return ConfigurationManager.AppSettings.FuzzyDuckAs<T>(
                    s => $"{context}.{s}",
                    s => s.RegexReplace($"^{context}\\.", ""),
                    true
                );
            }
            catch (UnDuckableException e)
            {
                PrintDuckErrors(context, e);
                return null;
            }
        }

        private static void PrintDuckErrors(
            string context,
            UnDuckableException unDuckableException
        )
        {
            unDuckableException.Print($"{context} improperly configured:\n* ");
        }

    }
}