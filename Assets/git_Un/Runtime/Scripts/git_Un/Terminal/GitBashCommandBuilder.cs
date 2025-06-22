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
    public string GetDiff(string mergeBase, string revParse)
    {
        mergeBase = mergeBase.Remove(mergeBase.Length - 28, 28);
        revParse = revParse.Remove(revParse.Length - 28, 28);
        //TODO: rn hard coded path, maybe bring to configs
        return GitCommands.diff + mergeBase + " " + revParse + " Assets/Scenes";
    }

    public string GetCommit(string contentPath, string commitMessage = null)
    {
        string message;
        if (commitMessage != null)
        {
            message = $" \' {commitMessage} \' ";
        }
        else
        {
            message = $" \' added {contentPath} on {DateTime.Now} \'";
        }
        
        //TODO: include .meta file
        return GitCommands.add + contentPath
                               + " && " + GitCommands.commit_m
                               + message;
    }

    public string GetPush(string branchName = default)
    {
        if (branchName == default)
        {
            return GitCommands.push_origin;
        }
        return GitCommands.push_origin + branchName;
    }

    public string GetPushAllBranches()
    {
        return GitCommands.push_origin + GitFlags.all;
    }

    public string GetCreateBranch(string branchName, bool push=false)
    {
        string command = GitCommands.branch + branchName;
        command = push ? command + " && git push --set-upstream origin " + branchName : command;
        return command;
    }

    public string GetCreateSwitchPushBranch(string branchName)
    {
        return $"git switch -c {branchName} && git push --set-upstream origin {branchName}";
    }

    public string GetCurrentBranch()
    {
        return GitCommands.branch + GitFlags.show_current;
    }

    public string GetSwitch(string branch)
    {
        return GitCommands.g_switch + branch;
    }

    public string GetSwitchAndPull(string branch)
    {
        return GetSwitch(branch) + " && " + GitCommands.pull;
    }

    /// <summary>
    /// Merges using the -Xours strategy
    /// Uses the --no-edit flag (default commit message)
    /// </summary>
    /// <param name="sourceBranch"></param>
    /// <returns></returns>
    public string GetMergeXours(string sourceBranch)
    {
        return GitCommands.merge_Xours + sourceBranch + GitFlags.no_edit;
    }

    public string GetMkdir(string path)
    {
        return BashCommands.mkdir + path;
    }

    public string GetTouch(string path, string fileNameWithType)
    {
        return BashCommands.touch + path + fileNameWithType;
    }

    public string GetInit()
    {
        return GitCommands.init;
    }

    public string GetAddRemote()
    {
        return GitCommands.add_remote + GlobalRefs.filePaths.remoteUrl;
    }

    public string GetSubtreeSplitNewBranch(string prefix, string newBranchName)
    {
        return GitCommands.subtree_split_prefix + prefix + GitFlags.branch + newBranchName;
    }

    public string GetReadLockedFile()
    {
        // git cat-file -p origin/file-locking:locked_files.json
        return $"{GitCommands.cat_file_p} origin/{GlobalRefs.lockingBranch}:{GlobalRefs.filePaths.lockedProtocolFile}";
    }

    public string GetNewestGitignoreContent()
    {
        return BashCommands.curl_o + ".gitignore https://raw.githubusercontent.com/github/gitignore/main/Unity.gitignore";
    }

    // ($"'*\\n!{GlobalRefs.filePaths.lockedProtocolFile}'",GlobalRefs.gitignore);
    public string GetOverrideFileContent(string content, string file)
    {
        return $"{BashCommands.echo_e} {content} > {file}\n";
    }

    public string GetEcho(string arg)
    {
        return BashCommands.echo + arg;
    }

    public string GetCatFile(string hash, string specificFile = default)
    {
        // Precaution, reduce to 12, because it caused issues in the past.
        hash = hash.Remove(hash.Length - 28, 28);
        
        if (specificFile == default)
        {
            return GitCommands.cat_file_p + hash;

        }
        return GitCommands.cat_file_p + hash + ":" + specificFile;
    }

    public string GetWriteLinesToFile(string[] lines, string filePath)
    {
        string cmd = BashCommands.echo_e;
        foreach (string l in lines)
        {
            string newL = l + $"'\\n'";
            cmd += newL;
        }

        return cmd + " >> " + filePath;
    }

    public string GetLogOfFile(string filePath)
    {
        return GitCommands.log_oneline + filePath;
    }

    public string GetCheckout(string hash, string path = default)
    {
        if (path == default)
        {
            return GitCommands.checkout + hash;
        }

        return GitCommands.checkout + hash + " " + path;
    }

    /// Only works for remote called 'origin'.
    public string GetIsBranchMerged(string featureBranch, string mainBranch, bool checkOnRemote)
    {
        if (!checkOnRemote)
        {
            return GitCommands.check_merged + mainBranch + " | grep " + featureBranch;
        }
        mainBranch = "origin/" + mainBranch;
        return GitCommands.fetch + " && " + GitCommands.check_merged + mainBranch + " | grep " + featureBranch;
    }
}