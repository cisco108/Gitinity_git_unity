using System;
using UnityEngine;

public class GitBashCommandBuilder : ICommandBuilder
{
    public string GetMergeBase(string targetBranch, string sourceBranch)
    {
        //space after command set in initialization
        return String.Join(GitCommands.merge_base, targetBranch, " ", sourceBranch);
    }

    public string GetRevParse(string branch)
    {
        //space after command set in initialization
        // Debug.Log("builder " + String.Join(GitCommands.rev_parse, branch));
        // return String.Join(GitCommands.rev_parse, branch);

        return GitCommands.rev_parse + branch;
    }
}