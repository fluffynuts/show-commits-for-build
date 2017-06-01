namespace ShowCommitsForBuild.BuildInfo
{
    public interface IBuildInfo
    {
        int BuildNumberRevision { get; }
        string SourceVersion { get; }
    }
}