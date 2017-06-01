namespace ShowCommitsForBuild.Config
{
    public interface IGitLabConfig
    {
        string Server { get; }
        string Project { get; }
        string Repository { get; }
    }
}