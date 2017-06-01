using System;
using System.Net;
using ShowCommitsForBuild.Coordinator;
using ShowCommitsForBuild.UserInterface;

namespace ShowCommitsForBuild
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        private enum ProgramReturnValues
        {
            Success = 0,
            NoBuild = 1,
            WebException = 13,
            GeneralException = 14
        }


        static int Main(string[] args)
        {
            var console = new ConsoleAbstraction();
            var logic = ShowCommitsForBuildCoordinator.Create(console);
            var buildNumberGetter = new BuildNumberGetter(console, args);
            var failureFeedback = new FailureFeedback(console);

            try
            {
                var buildNumber = buildNumberGetter.GetBuildNumberFromArgsOrUserInput();
                return logic.ShowCommitsForBuild(buildNumber)
                    ? (int) ProgramReturnValues.Success
                    : (int) ProgramReturnValues.NoBuild;
            }
            catch (WebException ex)
            {
                failureFeedback.PrintFatalWebFailure(ex);
                console.Pause();
                return (int) ProgramReturnValues.WebException;
            }
            catch (Exception ex)
            {
                failureFeedback.PrintFatalFailure(ex);
                console.Pause();
                return (int) ProgramReturnValues.GeneralException;
            }
        }
    }
}