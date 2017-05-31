namespace get_build_log
{
    public interface ITfsConfig
    {
        string Server { get; }
        string Collection { get; }
        string Project { get; }
        int BuildDefintionId { get; }
    }
}