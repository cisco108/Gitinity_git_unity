public interface ICommandBuilder
{
    public string GetMergeBase(string command, string targetBranch, string sourceBranch);
    public string GetRevParse(string command, string branch);
}