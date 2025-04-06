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
    public string GetPush(string branchName = default);
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
    public string GetReadLockedFile();

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
    public string GetOverrideFileContent(string content, string file);

    public string GetEcho(string arg);
    /// <summary>
    /// Pass in path to file, if you want only want the content of
    /// a specific file. Otherwise, you get commit message, author, ... 
    /// </summary>
    /// <param name="specificFile"></param>
    /// <returns></returns>
    public string GetCatFile(string hash, string specificFile = default);

    public string GetWriteLinesToFile(string[] lines, string filePath);

}