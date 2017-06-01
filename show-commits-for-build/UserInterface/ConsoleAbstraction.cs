using System;
using System.Linq;
using System.Security;

namespace ShowCommitsForBuild.UserInterface
{
    public class ConsoleAbstraction : IConsole
    {
        public void WriteLine(string str)
        {
            Console.WriteLine(str);
        }

        public string Prompt(string message)
        {
            WritePromptMessage(message);
            return Console.ReadLine()?.Trim() ?? "";
        }

        public SecureString PasswordPrompt(string message)
        {
            WritePromptMessage(message);
            return ReadPassword('*');
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public void Pause()
        {
            WriteLine(" ( Press any key to continue )");
            ReadKey();
        }

        private readonly int _enter = 13;
        private readonly int _backspace = 8;
        private readonly int _ctrlBackspace = 127;
        private readonly int[] _filter = {0, 27, 9, 10};

        private SecureString ReadPassword(char mask)
        {
            var securePass = new SecureString();
            char chr;

            while ((chr = Console.ReadKey(true).KeyChar) != _enter)
            {
                if (IsBackspace(chr))
                {
                    if (securePass.Length < 1)
                        continue;
                    Console.Write("\b \b");
                    securePass.RemoveAt(securePass.Length - 1);
                }
                else if (!IsFiltered(chr))
                {
                    securePass.AppendChar(chr);
                    Console.Write(mask);
                }
            }

            Console.WriteLine();
            return securePass;
        }

        private bool IsBackspace(char chr)
        {
            return chr == _backspace || chr == _ctrlBackspace;
        }

        private bool IsFiltered(char chr)
        {
            return _filter.Contains(chr);
        }

        private static void WritePromptMessage(string message)
        {
            Console.Write($"{message} ");
            Console.Out.Flush();
        }
    }
}