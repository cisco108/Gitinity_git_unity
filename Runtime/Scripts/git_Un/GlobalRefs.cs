using UnityEditor;
using UnityEngine;

public enum CustomPath
{
    BashExe,
}

[InitializeOnLoad]
public static class GlobalRefs
{
    public static UserConfig filePaths;
    public static string lockingBranch = "file-locking";
    public static string gitignore = ".gitignore";

    static GlobalRefs()
    {
        filePaths = UserConfig.instance;
    }

    // Is this still needed ?
    public static void UpdateCustomPaths(CustomPath pathToUpdate, string newPath)
    {
        switch (pathToUpdate)
        {
            case CustomPath.BashExe:
                filePaths.gitBashExe = newPath;
                break;
            default:
                Debug.Log($"no valid option");
                break;
        }
    }
}
