using UnityEngine;
using UnityEditor;

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

    public string test = $"Hello Sir {assetName}";
}