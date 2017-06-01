using System.Diagnostics;
using ShowCommitsForBuild.Config;

namespace ShowCommitsForBuild.Gitlab
{
    public class GitLabUrlLauncher : IGitLabUrlLauncher
    {
        private readonly IGitLabConfig _config;

        public GitLabUrlLauncher(IGitLabConfig config)
        {
            _config = config;
        }

        public void ShowCommits(string hash)
        {
            var url = BuildCommitsUrlFor(hash);
            Process.Start(url);
        }

        private string BuildCommitsUrlFor(string hash)
        {
            return $"{_config.Server}/{_config.Project}/{_config.Repository}/commits/{hash}";
        }
    }
}