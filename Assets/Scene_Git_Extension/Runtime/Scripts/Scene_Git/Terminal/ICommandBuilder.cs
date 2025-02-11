﻿public interface ICommandBuilder
{
    public string GetMergeBase(string targetBranch, string sourceBranch);
    public string GetRevParse(string branch);

    public string GetDiff(string mergeBase, string revParse);
    
    /// <summary>
    /// Adds and commit the file/directory + the respective .meta file.
    /// </summary>
    /// <param name="contentPath">file/directory</param>
    /// <returns></returns>
    public string GetCommit(string contentPath);

    public string GetSwitch(string branch);
    public string GetMergeXours(string sourceBranch);
}