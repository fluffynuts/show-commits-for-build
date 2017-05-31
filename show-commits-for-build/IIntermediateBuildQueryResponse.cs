namespace get_build_log
{
    public interface IIntermediateBuildQueryResponse
    {
        int Count { get; }
        object[] Value { get; }
    }
}