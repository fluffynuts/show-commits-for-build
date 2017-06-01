namespace ShowCommitsForBuild.Coordinator
{
    public interface IShowCommitsForBuildCoordinator
    {
        bool ShowCommitsForBuild(int buildNumber);
    }
}