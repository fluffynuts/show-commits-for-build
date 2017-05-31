namespace ShowCommitsForBuild
{
    public interface IIntermediateBuildQueryResponse
    {
        int Count { get; }
        object[] Value { get; }
    }
}