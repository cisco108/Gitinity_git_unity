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

    //TODO: solve these paths, make it better
    public static string shellScripts =
        @"Library\PackageCache\com.newhere_tools.git_un\Runtime\Scripts\git_Un\ShellScripts\";
    public static string openWhenSceneIsLocked =
        @"Library\PackageCache\com.newhere_tools.git_un\Runtime\Scene\OpensWhenSceneIsLocked.unity";

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