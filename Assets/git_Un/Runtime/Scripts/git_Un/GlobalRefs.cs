using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

// Is this still needed ?

[InitializeOnLoad]
public static class GlobalRefs
{
    public static UserConfig filePaths;
    public static string lockingBranch = "file-locking";
    public static string gitignore = ".gitignore";
    public static StateObj StateObj;
    //
    // //TODO: solve these paths, make it better
    // public static string shellScripts =
    //     @"Library\PackageCache\com.newhere_tools.git_un\Runtime\Scripts\git_Un\ShellScripts\";
    // public static string openWhenSceneIsLocked =
    //     @"Library\PackageCache\com.newhere_tools.git_un\Runtime\Scene\OpensWhenSceneIsLocked.unity";
    
    
    private static string ShellScript1GUID = "52110bcc73484879893fbbfbb69684b9";
    private static string ShellScript2GUID = "9af826befe274ba68165ff88d2bfa719";

    public static string ShellScript(int num)
    {
        string path;
        switch (num)
        {
            case 1:
            path = AssetDatabase.GUIDToAssetPath(ShellScript1GUID);
            break;
            case 2:
            path = AssetDatabase.GUIDToAssetPath(ShellScript2GUID);
            break;
            default:
            path = null;
            break;
        }
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Shell script path not found. Check if the file still exists or if the GUID is correct.");
        }
        return path;       
    }

    static GlobalRefs()
    {
        filePaths = UserConfig.instance;
    }

    
    
    public static void SetState(string[] branchNames)
    {
        StateObj = new StateObj(branchNames);
    }
}


public enum State
{
    PreInit,
    PostInit
}
public class StateObj
{
    // take in data in the constructor and initialize accordingly
    private string[] _branchNames;
    public string[] BranchNames => _branchNames;
    public State State;

    public StateObj(string[] branchNames)
    {
        if (branchNames == null)
        {
            State = State.PreInit;
        }
        else
        {
            _branchNames = branchNames;
            State = State.PostInit;
        }
    }
}