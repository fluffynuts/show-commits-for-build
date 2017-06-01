using ShowCommitsForBuild.BuildInfo;
using ShowCommitsForBuild.Config;
using ShowCommitsForBuild.Gitlab;
using ShowCommitsForBuild.UserInterface;

namespace ShowCommitsForBuild.Coordinator
{
    public class ShowCommitsForBuildCoordinator : IShowCommitsForBuildCoordinator
    {
        private readonly IBuildInfoFinder _infoFinder;
        private readonly IGitLabUrlLauncher _gitLabUrlLauncher;

        public ShowCommitsForBuildCoordinator(
            IBuildInfoFinder infoFinder,
            IGitLabUrlLauncher gitLabUrlLauncher)
        {
            _infoFinder = infoFinder;
            _gitLabUrlLauncher = gitLabUrlLauncher;
        }

        public bool ShowCommitsForBuild(int buildNumber)
        {
            var buildInfo = _infoFinder.GetInfoForBuildRevision(buildNumber);
            if (buildInfo == null)
                return false;

            _gitLabUrlLauncher.ShowCommits(buildInfo.SourceVersion);
            return true;
        }

        public static IShowCommitsForBuildCoordinator Create(IConsole console)
        {
            // poor-man's IoC
            var configFinder = new ConfigFinder(console);
            var requestFactory = new SecureWebRequestFactory(console);
            var tfsConfig = configFinder.GetConfig<ITfsConfig>("TFS");
            var gitLabConfig = configFinder.GetConfig<IGitLabConfig>("GitLab");
            var infoFinder = new BuildInfoFinder(console, tfsConfig, requestFactory);
            var gitLabUrlLauncher = new GitLabUrlLauncher(gitLabConfig);
            return new ShowCommitsForBuildCoordinator(
                infoFinder,
                gitLabUrlLauncher
            );
        }
    }
}