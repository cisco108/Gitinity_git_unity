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
    public string logsFile = "logs.txt";
    public string lockedProtocolFile = "locked_files.txt";
    public string diffPrefabsParentDirectory = "Assets/";
    public string diffPrefabsDirName = "DiffObjects_as_Prefabs/";
    public string DiffPrefabsDirectory => diffPrefabsParentDirectory + diffPrefabsDirName;
    public string lockingProtocolDirectory = "LockingProtocol/";
    public string gitBashExe = @"C:\Program Files\Git\git-bash.exe";
    public string remoteUrl = "Paste the http link for your empty repository.";
}