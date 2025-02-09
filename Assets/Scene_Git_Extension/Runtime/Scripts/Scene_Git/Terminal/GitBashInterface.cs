using System;
using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;

public class GitBashInterface : ITerminalInterface
{
    private const string PathToBashExe = @"C:\Program Files\Git\git-bash.exe";
    // private const string PathToBashExe = @"C:\Program Files\Git\bin\git.exe";
    // private const string PathToSh =
        // "Assets/Scene_Git_Extension/Runtime/Scripts/Scene_Git/Terminal/ShellScripts/get_diff.sh";

    public void Execute()
    {
        BashToTxt(GitCommands.logOneLine, "helloBash.txt");
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

            if (!string.IsNullOrEmpty(output))
            {
                Debug.Log($"Git Bash Output: {output}");
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Git Bash Error: {error}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error: {e}");
        }
    }


}