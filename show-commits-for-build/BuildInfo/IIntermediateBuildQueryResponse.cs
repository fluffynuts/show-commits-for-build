namespace ShowCommitsForBuild.BuildInfo
{
    public interface IIntermediateBuildQueryResponse
    {
        int Count { get; }
        object[] Value { get; }
    }
}