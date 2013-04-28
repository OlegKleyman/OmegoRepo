namespace Oleg.Kleyman.Core
{
    public interface IPathBuilder
    {
        string Build(string target, string target2);
        string Combine(string target1, string target2);
        string GetFullPath(string targetPath);
    }
}