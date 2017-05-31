using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PeanutButter.DuckTyping.Exceptions;
using PeanutButter.DuckTyping.Extensions;
using PeanutButter.Utils;

namespace get_build_log
{
    public class BuildInfoFinder
    {
        private readonly ITfsConfig _config;

        public BuildInfoFinder(ITfsConfig config)
        {
            _config = config;
        }

        public IBuildInfo GetInfoForBuildRevision(int buildNumber)
        {
            var url = BuildTfsApiUrl();
            Console.WriteLine($"Querying builds at: {url}");
            var request = CreateTfsRequest(url);
            using (var webResponse = request.GetResponse())
            {
                using (var stream = webResponse.GetResponseStream())
                {
                    var response = TryGetResponseFrom(stream, (int) webResponse.ContentLength);
                    return FindInfoForBuildRevision(buildNumber, response);
                }
            }
        }

        private IIntermediateBuildQueryResponse TryGetResponseFrom(Stream stream, int streamLength)
        {
            try
            {
                var asDict = GetResponseDictionaryFrom(stream, streamLength);
                return asDict.FuzzyDuckAs<IIntermediateBuildQueryResponse>(true);
            }
            catch (UnDuckableException e)
            {
                e.Print("Unable to grok tfs server response:");
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
            Stream stream, int contentLength
        )
        {
            var jsonText = GetJsonTextFrom(stream, contentLength);
            var jsonObject = JsonConvert.DeserializeObject(jsonText) as JObject;
            var asDict = jsonObject.ToDictionary();
            return asDict;
        }

        private static string GetJsonTextFrom(Stream stream, int contentLength)
        {
            if (stream == null)
                throw new Exception("Build query gets null response )':");
            var buffer = new byte[contentLength];
            var offset = 0;
            var toRead = contentLength;
            while (toRead > 0)
            {
                var read = stream.Read(buffer, offset, toRead);
                offset += read;
                toRead -= read;
            }
            var jsonText = buffer.ToUTF8String();
            return jsonText;
        }

        private HttpWebRequest CreateTfsRequest(string url)
        {
            var request = WebRequest.CreateHttp(url);
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            return request;
        }

        private string BuildTfsApiUrl()
        {
            return $"{_config.Server}/{_config.Collection}/{_config.Project}/_apis/build/builds";
        }
    }
}