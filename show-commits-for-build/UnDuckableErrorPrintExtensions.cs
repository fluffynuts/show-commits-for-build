using System;
using PeanutButter.DuckTyping.Exceptions;
using PeanutButter.Utils;

namespace ShowCommitsForBuild
{
    public static class UnDuckableErrorPrintExtensions
    {
        public static void Print(this UnDuckableException e, string header)
        {
            Console.WriteLine(header);
            Console.WriteLine(e.Errors.JoinWith("\n* "));
        }
    }
}