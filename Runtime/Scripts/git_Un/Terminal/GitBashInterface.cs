using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Debug = UnityEngine.Debug;

public class GitBashInterface : ITerminalInterface
{
    private const string TempFile = "temp_commit_hash.txt";
    private const string SavedDiff = "saved_diff.txt";


    /// <summary>
    /// This might not do what you want.
    /// Only checks if hardcoded SaveDiff exists, not the outputFile parameter.
    /// </summary>
    public void ExecuteResultToTxt(string command, string outputPath = default)
    {
        // Could be changed, dependent if the save diff is most used or not.
        // Looks like this cause save diff was the first functionality.
        if (outputPath == default)
        {
            outputPath = SavedDiff;
        }
        
        BashToTxt(command, outputPath);
    }

    public string ExecuteResultToString(string command)
    {
        BashToTxt(command, TempFile);
        string result = TxtToString(TempFile);
        File.Delete(TempFile);
        if (result == string.Empty)
        {
            Debug.LogError($"Result is empty. Possible wrong branch names");
        }

        return result;
    }

    public string[] ExecuteResultToStringArr(string command)
    {
        string s = ExecuteResultToString(command);
        
        if(string.IsNullOrEmpty(s))
        {
            Debug.LogError($"Command {command} lead to empty result.");
            return null; 
        }
        
        if (command.StartsWith(GitCommands.log_oneline))
        {
            command = GitCommands.log_oneline;
        }
        
        Debug.Log($"Commands ({command}) result split in array.");
        string[] result;
        switch (command)
        {
            case "git branch ":
                result = s.Split('\n')
                    .Select(b => b.Trim('*', ' '))
                    .ToArray();
                return result;
            
            case "git log --oneline ":
                result = s.Split('\n');
                return result;
            
            default:
                Debug.LogError($"Only implemented for 'git branchSPACE' and 'git log ' command. Please add implementation for other");
                return null;
        }
    }

    public void Execute(string command)
    {
        Bash(command);
    }


    private void Bash(string command)
    {
        try
        {
            using Process gitProcess = new Process();
            gitProcess.StartInfo.FileName = GlobalRefs.filePaths.gitBashExe;
            gitProcess.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();


            gitProcess.StartInfo.Arguments = $"-c \"{command}\"";

            Debug.Log(gitProcess.StartInfo.Arguments);
            gitProcess.StartInfo.UseShellExecute = false;
            gitProcess.StartInfo.RedirectStandardOutput = true;
            gitProcess.StartInfo.RedirectStandardError = true;
            gitProcess.StartInfo.CreateNoWindow = true;

            gitProcess.Start();
            gitProcess.WaitForExit();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error from bash: {e}");
            throw;
        }
    }

    /// <summary>
    /// This might not do what you want.
    /// Uses hardcoded SaveDiff, not the outputFile parameter.
    /// </summary>
    private void BashToTxt(string command, string outputFileNameWithType)
    {
        try
        {
            using Process gitProcess = new Process();
            gitProcess.StartInfo.FileName = GlobalRefs.filePaths.gitBashExe;
            gitProcess.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();

            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), $"\\{outputFileNameWithType}");

            if (File.Exists(SavedDiff))
            {
                File.Delete(SavedDiff);
            }

            gitProcess.StartInfo.Arguments = $"-c \"{command} >> {outputPath}\"";

            // Debug.Log(gitProcess.StartInfo.Arguments);
            gitProcess.StartInfo.UseShellExecute = false;
            gitProcess.StartInfo.RedirectStandardOutput = true;
            gitProcess.StartInfo.RedirectStandardError = true;
            gitProcess.StartInfo.CreateNoWindow = true;

            gitProcess.Start();

            string output = gitProcess.StandardOutput.ReadToEnd();
            string error = gitProcess.StandardError.ReadToEnd();

            gitProcess.WaitForExit();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error from bash: {e}");
            throw;
        }
    }

    private string TxtToString(string path)
    {
        if (!File.Exists(path))
            Debug.LogError($"Cant find file {path}");

        string s = File.ReadAllText(path);
        // Debug.Log($"Extracted string: {s}");
        return s;
    }
}