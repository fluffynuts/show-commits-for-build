namespace get_build_log
{
    public interface IGitLabConfig
    {
        string Server { get; }
        string Project { get; }
        string Repository { get; }
    }
}