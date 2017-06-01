using System.Security;

namespace ShowCommitsForBuild.UserInterface
{
    public interface IConsole
    {
        void WriteLine(string str);
        string Prompt(string message);
        SecureString PasswordPrompt(string message);
        void Pause();
    }
}