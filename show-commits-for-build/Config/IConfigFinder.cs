namespace ShowCommitsForBuild.Config
{
    public interface IConfigFinder
    {
        T GetConfig<T>(
            string context
        ) where T : class;
    }
}