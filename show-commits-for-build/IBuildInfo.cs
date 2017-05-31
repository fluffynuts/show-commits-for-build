namespace ShowCommitsForBuild
{
    public interface IBuildInfo
    {
        int BuildNumberRevision { get; }
        string SourceVersion { get; }
    }
}