namespace ShowCommitsForBuild.Gitlab
{
    public interface IGitLabUrlLauncher
    {
        void ShowCommits(string hash);
    }
}