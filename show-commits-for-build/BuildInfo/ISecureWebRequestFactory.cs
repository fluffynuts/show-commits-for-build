using System.Net;

namespace ShowCommitsForBuild.BuildInfo
{
    public interface ISecureWebRequestFactory
    {
        HttpWebRequest CreateFor(string url);
    }
}