using System;
using System.Net;
using PeanutButter.Utils;
using ShowCommitsForBuild.BuildInfo;

namespace ShowCommitsForBuild.UserInterface
{
    public class FailureFeedback : IFailureFeedback
    {
        private readonly IConsole _console;

        public FailureFeedback(IConsole console)
        {
            _console = console;
        }

        public void PrintFatalWebFailure(
            WebException webException)
        {
            using (var reader = new WebResponseReader(webException.Response))
            {
                var message = new[]
                {
                    $"Web request failed ({webException.Status})",
                    "Response was:",
                    reader.ReadFullResponse() ?? "(unknown)"
                }.JoinWith("\n");
                PrintFatalFailure(message, webException.StackTrace);
            }
        }

        private void PrintFatalFailure(
            string message,
            string stackTrace
        )
        {
            new[]
            {
                "Ruh-roh! Something went wrong! Please report this issue to someone you thing can fix it (:",
                "(more info follows)",
                "What went wrong:",
                "----------------",
                message,
                "Where it went wrong:",
                "--------------------",
                stackTrace,
                "--------------------"
            }.ForEach(_console.WriteLine);
        }

        public void PrintFatalFailure(Exception ex)
        {
            PrintFatalFailure(ex.Message, ex.StackTrace);
        }
    }
}