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

    public string GetDiff(string mergeBase, string revParse)
    {
        mergeBase = mergeBase.Remove(mergeBase.Length - 28, 28);
        revParse = revParse.Remove(revParse.Length - 28, 28);
        return GitCommands.diff + mergeBase + " " + revParse + " ";
    }
}