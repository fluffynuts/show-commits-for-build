using System;
using System.Linq;
using System.Net;
using ShowCommitsForBuild.UserInterface;

namespace ShowCommitsForBuild.BuildInfo
{
    public class SecureWebRequestFactory: ISecureWebRequestFactory
    {
        private readonly IConsole _console;

        public SecureWebRequestFactory(IConsole console)
        {
            _console = console;
        }

        public HttpWebRequest CreateFor(string url)
        {
            var request = WebRequest.CreateHttp(url);
            request.Credentials = GetRequestCredentials();
            return request;
        }

        private ICredentials GetRequestCredentials()
        {
            return ShouldPrompt()
                ? GetUserCredentials()
                : CredentialCache.DefaultNetworkCredentials;
        }

        private bool ShouldPrompt()
        {
            if (CredentialCache.DefaultNetworkCredentials == null)
                return true;
            var envVar = Environment.GetEnvironmentVariable("USE_DEFAULT_CREDENTIALS");
            return IsFalse(envVar);
        }

        private readonly string[] _falseValues = {
            "0",
            "no",
            "false"
        };

        private bool IsFalse(string envVar)
        {
            return _falseValues.Contains(envVar);
        }

        private ICredentials GetUserCredentials()
        {
            _console.WriteLine("Network credentials required");
            return new NetworkCredential(
                _console.Prompt("Login:  "),
                _console.PasswordPrompt("Password: ")
            );
        }
    }
}