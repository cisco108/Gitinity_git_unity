public interface ICommandBuilder
{
    public string GetMergeBase(string targetBranch, string sourceBranch);
    public string GetRevParse(string branch);

    public string GetDiff(string mergeBase, string revParse);
    public string GetCommit(string contentPath);
}