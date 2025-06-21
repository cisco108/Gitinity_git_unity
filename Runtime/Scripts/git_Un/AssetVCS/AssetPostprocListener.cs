using System;
using UnityEditor;
using UnityEngine;

public class AssetPostprocListener : AssetPostprocessor
{
    public static Action OnAssetPosPro;
    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        OnAssetPosPro?.Invoke();
        Debug.Log($"OnPostPro form AssetPostproListener");
    }
}