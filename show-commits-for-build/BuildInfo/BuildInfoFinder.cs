using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PeanutButter.DuckTyping.Exceptions;
using PeanutButter.DuckTyping.Extensions;
using ShowCommitsForBuild.Config;
using ShowCommitsForBuild.Extensions;
using ShowCommitsForBuild.UserInterface;

namespace ShowCommitsForBuild.BuildInfo
{
    public interface IBuildInfoFinder
    {
        IBuildInfo GetInfoForBuildRevision(int buildNumber);
    }

    public class BuildInfoFinder : IBuildInfoFinder
    {
        private readonly IConsole _console;
        private readonly ITfsConfig _config;
        private readonly ISecureWebRequestFactory _requestFactory;

        public BuildInfoFinder(
            IConsole console,
            ITfsConfig config,
            ISecureWebRequestFactory requestFactory
        )
        {
            _console = console;
            _config = config;
            _requestFactory = requestFactory;
        }

        public IBuildInfo GetInfoForBuildRevision(int buildNumber)
        {
            var url = BuildTfsApiUrl();
            System.Console.WriteLine($"Querying builds at: {url}");
            var request = _requestFactory.CreateFor(url);
            using (var reader = new WebResponseReader(request))
            {
                var response = TryGetResponseFrom(reader.ReadFullResponse());
                return FindInfoForBuildRevision(buildNumber, response);
            }
        }

        private IIntermediateBuildQueryResponse TryGetResponseFrom(string jsonText)
        {
            try
            {
                var asDict = GetResponseDictionaryFrom(jsonText);
                return asDict.FuzzyDuckAs<IIntermediateBuildQueryResponse>(true);
            }
            catch (UnDuckableException e)
            {
                _console.WriteLine(e.FormatError("Unable to grok tfs server response:"));
                return null;
            }
        }

        private static IBuildInfo FindInfoForBuildRevision(int buildNumber, IIntermediateBuildQueryResponse result)
        {
            var infos = result?.Value
                .Select(o => o.FuzzyDuckAs<IBuildInfo>())
                .Where(o => o != null)
                .ToArray();
            return infos?.FirstOrDefault(i => i.BuildNumberRevision == buildNumber);
        }

        private static Dictionary<string, object> GetResponseDictionaryFrom(
            string jsonText
        )
        {
            var jsonObject = JsonConvert.DeserializeObject(jsonText) as JObject;
            var asDict = jsonObject.ToDictionary();
            return asDict;
        }

        private string BuildTfsApiUrl()
        {
            return $"{_config.Server}/{_config.Collection}/{_config.Project}/_apis/build/builds";
        }
    }
}