using System;
using System.Net;

namespace ShowCommitsForBuild.UserInterface
{
    public interface IFailureFeedback
    {
        void PrintFatalWebFailure(WebException webException);
        void PrintFatalFailure(Exception ex);
    }
}