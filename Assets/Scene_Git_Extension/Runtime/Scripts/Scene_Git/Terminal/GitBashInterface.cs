
using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GitBashInterface : ITerminalInterface
{
    private const string PathToBashExe = @"C:\Program Files\Git";
    public void Execute()
    {
        try
        {
            using (Process gitProcess = new Process())
            {
                gitProcess.StartInfo.FileName = PathToBashExe;
                gitProcess.Start();

            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error: {e}");
        }
    }
}