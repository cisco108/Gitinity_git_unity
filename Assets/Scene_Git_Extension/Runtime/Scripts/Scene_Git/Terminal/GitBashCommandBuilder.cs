using System;
using UnityEngine;

public class GitBashCommandBuilder : ICommandBuilder
{
    public string GetMergeBase(string targetBranch, string sourceBranch)
    {
        //space after command set in initialization
        return GitCommands.merge_base + targetBranch + " " + sourceBranch;
    }

    public string GetRevParse(string branch)
    {
        //space after command set in initialization
        return GitCommands.rev_parse + branch;
    }

    /// <summary>
    /// Shortens the lenght of hashes to 12 characters.
    /// Full 40 character hashes lead to wrong execution.
    /// </summary>
    /// <param name="mergeBase"></param>
    /// <param name="revParse"></param>
    /// <returns></returns>
    public string GetDiff(string mergeBase, string revParse)
    {
        mergeBase = mergeBase.Remove(mergeBase.Length - 28, 28);
        revParse = revParse.Remove(revParse.Length - 28, 28);
        return GitCommands.diff + mergeBase + " " + revParse + " Assets/Scenes";
    }

    public string GetCommit(string contentPath)
    {
        return GitCommands.add + contentPath + "&& " + GitCommands.commit_m 
               + $" \' added {contentPath} on {DateTime.Now} \'"; 
    }
}