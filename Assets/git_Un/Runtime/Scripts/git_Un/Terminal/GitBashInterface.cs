using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Debug = UnityEngine.Debug;

public class GitBashInterface : ITerminalInterface
{
    private const string PathToBashExe = @"C:\Program Files\Git\git-bash.exe";
    private const string TempFile = "temp_commit_hash.txt";
    private const string SavedDiff = "saved_diff.txt";


    public void ExecuteResultToTxt(string command)
    {
        BashToTxt(command, SavedDiff);
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
        switch (command)
        {
            case "git branch ":
                var s = ExecuteResultToString(command);
                string[] result = s.Split('\n')
                    .Select(b => b.Trim('*', ' '))
                    .ToArray();
                return result;
            
            default:
                Debug.LogError($"Only implemented for 'git branchSPACE' command. Please add implementation for other");
                return null;
        }
    }

    public void ExecuteBasicCommand(string command)
    {
        Bash(command);
    }


    private void Bash(string command)
    {
        try
        {
            using Process gitProcess = new Process();
            gitProcess.StartInfo.FileName = PathToBashExe;
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

    private void BashToTxt(string command, string outputFileNameWithType)
    {
        try
        {
            using Process gitProcess = new Process();
            gitProcess.StartInfo.FileName = PathToBashExe;
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