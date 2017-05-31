namespace get_build_log
{
    public interface IBuildInfo
    {
        int BuildNumberRevision { get; }
        string SourceVersion { get; }
    }
}