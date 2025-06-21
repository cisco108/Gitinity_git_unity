using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class TestLog : MonoBehaviour
{
    private bool b = true;

    private ITerminalInterface _terminal;
    
    // private void OnValidate()
    // {
    //     _terminal = new GitBashInterface();
    //     string s = GlobalRefs.ShellScript(1);
    //     Debug.Log(s);
    //     Debug.Log(s);
    //
    // }
    
    /*private void OnValidate()
    {
        string targetFolder = GlobalRefs.filePaths.versionControlledAssets;
        string[] guids = AssetDatabase.FindAssets("", new[] { targetFolder });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            Debug.Log($"Found asset at path: {path} ({asset.GetType()})");
        }
    }*/

    void Start()
    {
        _terminal = new GitBashInterface();
        
        string bS = b.ToString().ToLower();
        _terminal.Execute($"echo '{bS}' > {GlobalRefs.filePaths.allowCommitFile}");
        
        Debug.Log(bS);
    }
    
}