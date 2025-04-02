using UnityEditor;
using UnityEngine;

// Is this still needed ?
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
    public static GitDataObject gitDataObj;

    public static string shellScripts =
        @"Library\PackageCache\com.newhere_tools.git_un\Runtime\Scripts\git_Un\ShellScripts\";

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
    
    
    public static void InitGitDataObj(string[] branchNames)
    {
        if (branchNames != null)
        {
            gitDataObj = new GitDataObject(branchNames);
            return;
        }
    }
}
