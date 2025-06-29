using System.IO;
using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public static class GlobalRefs
{
    public static UserConfig filePaths;
    public static string lockingBranch = "file-locking";
    public static string gitignore = ".gitignore";
    public static StateObj StateObj;
    public static bool isFeatureMerged;
    public static string currFeatureName;

    // Scripts with bash commands that set up things.
    private static string ShellScript1GUID = "52110bcc73484879893fbbfbb69684b9";
    private static string ShellScript2GUID = "9af826befe274ba68165ff88d2bfa719";
    // End

    private static string SetupGitHookShGUID = "a0172c20623e25e40b23dc43184524b9";

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
            case 3:
                string virtualPath = AssetDatabase.GUIDToAssetPath(SetupGitHookShGUID);
                path = ResolvePackagePath(virtualPath);
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


    /// Needed because the path you get form AssetDataBase.GUIDToAssetPath is a virtual
    /// one, that does work in Unity, but is not an actual path on your file system.
    private static string ResolvePackagePath(string virtualPath)
    {
        try
        {

            string packageRelative = virtualPath.Substring("Packages/".Length);
            string packageCacheDir = Path.Combine("Library", "PackageCache");
            string[] candidates =
                Directory.GetDirectories(packageCacheDir, packageRelative + "@*", SearchOption.TopDirectoryOnly);

            if (candidates.Length == 0)
            {
                Debug.LogError("Could not find matching package in PackageCache for: " + packageRelative);
                return null;
            }

            // Use the first match
            string realPackagePath = candidates[0];
            string relativeScriptPath = virtualPath.Substring(("Packages/" + packageRelative).Length);
            return Path.Combine(realPackagePath, relativeScriptPath.TrimStart('/'));
        }
        catch
        {
            Debug.Log($"Probably in development project, where the virtual path {virtualPath} should work.");
            return virtualPath;
        }
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