using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

public class UserConfig : ScriptableObject
{
    private static string assetName => nameof(UserConfig);

    private static UserConfig _instance;

    public static UserConfig instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = CreateInstance<UserConfig>();
            AssetDatabase.CreateAsset(_instance, $"Assets/{assetName}.asset");
            return _instance;
        }
    }

    public string userEmail = "example@mail.com";
    public string logsFile = "logs.txt";
    public string lockedProtocolFile = "locked_files.json";
    public string diffPrefabsParentDirectory = "Assets/";
    public string diffPrefabsDirName = "DiffObjects_as_Prefabs/";
    public string versionControlledAssets = "Assets/VersionControlledAssets/";
    public string DiffPrefabsDirectory => diffPrefabsParentDirectory + diffPrefabsDirName;
    public string gitBashExe = @"C:\Program Files\Git\git-bash.exe";
    public string defaultBranchName = "master";
    public string remoteUrl = "Paste the http link for your empty repository.";
    public string allowCommitFile = ".allow_commit";
    public string setupGitHookFile = "setup_git_hook_asset_validation.sh";

    public bool useFileLocking = false;
    public bool useAssetVCS = false;
    // public string fileToLockName = "This file will the locked!";
}