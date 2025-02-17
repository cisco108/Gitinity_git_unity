using System;
using UnityEditor;
using UnityEngine;

public enum Path
{
    BashExe,
}

[InitializeOnLoad]
public static class GlobalRefs
{
    public static FilePaths filePaths;

    static GlobalRefs()
    {
        filePaths = new FilePaths();
    }

    public static void UpdateCustomPaths(Path pathToUpdate, string newPath)
    {
        switch (pathToUpdate)
        {
            case Path.BashExe:
                filePaths.gitBashExe = newPath;
                break;
            default:
                Debug.Log($"hello from Global Refs");
                break;
        }
    }
}