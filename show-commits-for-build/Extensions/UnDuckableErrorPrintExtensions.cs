using PeanutButter.DuckTyping.Exceptions;
using PeanutButter.Utils;

namespace ShowCommitsForBuild.Extensions
{
    public static class UnDuckableErrorPrintExtensions
    {
        public static string FormatError(this UnDuckableException e, string header)
        {
            return new[]
            {
                header,
                $"* {e.Errors.JoinWith("\n* ")}"
            }.JoinWith("\n");
        }
    }
}