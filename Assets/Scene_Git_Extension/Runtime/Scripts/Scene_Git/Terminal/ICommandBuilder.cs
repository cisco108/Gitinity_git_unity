public interface ICommandBuilder
{
    public string GetMergeBase(string targetBranch, string sourceBranch);
    public string GetRevParse(string branch);
}