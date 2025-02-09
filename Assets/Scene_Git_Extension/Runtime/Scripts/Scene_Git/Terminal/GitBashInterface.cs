using System;
using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;

public class GitBashInterface : ITerminalInterface
{
    private const string PathToBashExe = @"C:\Program Files\Git\git-bash.exe";
    private const string TempFile = "temp_commit_hash.txt";  

    // private const string PathToBashExe = @"C:\Program Files\Git\bin\git.exe";
    // private const string PathToSh =
    // "Assets/Scene_Git_Extension/Runtime/Scripts/Scene_Git/Terminal/ShellScripts/get_diff.sh";

    public void ExecuteResultToTxt(string branchName)
    {
        BashToTxt(GitCommands.log_oneline, "saved_diff.txt");
        // ExecBashScript();
    }

    public string ExecuteResultToVar(string command)
    {
        BashToTxt(command, TempFile);
        string result = TxtToString(TempFile);
        File.Delete(TempFile);
        return result;
    }


    private void BashToTxt(string command, string outputFileNameWithType)
    {
        try
        {
            using Process gitProcess = new Process();
            gitProcess.StartInfo.FileName = PathToBashExe;
            gitProcess.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();

            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), $"\\{outputFileNameWithType}");
            gitProcess.StartInfo.Arguments = $"-c \"{command} >> {outputPath}\"";

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
        Debug.Log($"Extracted string: {s}");
        return s;
    }
}