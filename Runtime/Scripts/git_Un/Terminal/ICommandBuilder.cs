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
    public string GetPushAllBranches();
    public string GetCreateBranch(string branchName);
    public string GetCurrentBranch();

    public string GetSwitch(string branch);
    public string GetMergeXours(string sourceBranch);

    public string GetMkdir(string path);
    public string GetTouch(string path, string fileNameWithType);
    public string GetInit();
    public string GetAddRemote();
    public string GetSubtreeSplitNewBranch(string prefix, string newBranchName);

    /// <summary>
    /// Curls (downloads) the gitignore for Unity Projects from github
    /// and writes it to an existing gitignore (has to exist).
    /// </summary>
    /// <returns></returns>
    public string GetNewestGitignoreContent();

    /// <summary>
    /// Current implementation only for file locking
    /// branch: ignores everything except for
    /// locked_files.txt .
    /// </summary>
    /// <returns></returns>
    public string GetOverrideGitignore();

    public string GetEcho(string arg);

}