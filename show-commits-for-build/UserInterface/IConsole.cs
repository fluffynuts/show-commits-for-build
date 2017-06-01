using System;
using System.Security;

namespace ShowCommitsForBuild.UserInterface
{
    public interface IConsole
    {
        void WriteLine(string str);
        string Prompt(string message);
        SecureString PasswordPrompt(string message);
        ConsoleKeyInfo ReadKey();
        void Pause();
    }
}