public interface ICommandBuilder
{
    public string GetMergeBase(string targetBranch, string sourceBranch);
    public string GetRevParse(string branch);

    public string GetDiff(string mergeBase, string revParse);
    
    /// <summary>
    /// Adds and commit the file/directory (.meta not implemented yet)
    /// </summary>
    /// <param name="contentPath">file/directory</param>
    /// <returns></returns>
    public string GetCommit(string contentPath);
    public string GetPush(string branchName);

    public string GetSwitch(string branch);
    public string GetMergeXours(string sourceBranch);

    public string GetMkdir(string path);
    public string GetTouch(string path, string fileNameWithType);
    public string GetSubtreeSplitNewBranch(string prefix, string newBranchName);
}