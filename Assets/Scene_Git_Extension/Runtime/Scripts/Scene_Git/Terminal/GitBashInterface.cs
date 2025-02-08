
using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GitBashInterface : ITerminalInterface
{
    private const string PathToBashExe = @"C:\Program Files\Git";

    private const string PathToSh =
        "Assets/Scene_Git_Extension/Runtime/Scripts/Scene_Git/Terminal/ShellScripts/get_diff.sh";
    public void Execute()
    {
        try
        {
            using (Process gitProcess = new Process())
            {
                // gitProcess.StartInfo.FileName = PathToBashExe;
                Process.Start(PathToBashExe, PathToSh);

            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error: {e}");
        }
    }
}