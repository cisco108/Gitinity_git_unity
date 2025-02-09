using System;

public class GitBashCommandBuilder : ICommandBuilder
{
    public string GetMergeBase(string targetBranch, string sourceBranch)
    {
        return String.Join(GitCommands.merge_base, " ", targetBranch, " ", sourceBranch);
    }

    public string GetRevParse(string branch)
    {
        return String.Join(GitCommands.rev_parse, " ", branch);
        
    } 
}