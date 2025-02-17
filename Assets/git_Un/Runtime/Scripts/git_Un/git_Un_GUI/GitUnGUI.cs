using System;
using UnityEditor;
using UnityEngine;

public class GitUnGUI : EditorWindow
{
    [MenuItem("Window/git_Un_GUI")]
    private static void Init() => GetWindow<GitUnGUI>(true, "git_Un_GUI");

    public static event Action<string, string> OnStartSceneGet;

    // public static event Action<string> OnLockFile;
    public static event Action<string, string> OnGetDiffFromSh;
    private string _sourceBranch = string.Empty;
    private string _targetBranch = string.Empty;
    private int _targetSelection = 0;
    private int _sourceSelection = 0;

    private string[] branchNames = new[] { "master", "feature", "dev-this" };
    // private string _fileToLock = string.Empty;

    private void OnGUI()
    {
        _targetSelection = EditorGUILayout.Popup(new GUIContent("target branch"), _targetSelection, branchNames);
        _sourceSelection = EditorGUILayout.Popup(new GUIContent("source branch"), _sourceSelection, branchNames);

        _targetBranch = branchNames[_targetSelection];
        _sourceBranch = branchNames[_sourceSelection];
        
        Debug.Log($"target: {_targetBranch}");
        Debug.Log($"source: {_sourceBranch}");
        
        if (GUILayout.Button("START"))
        {
            FireStartSceneGet();
        }




        /*_fileToLock = EditorGUILayout.TextField(new GUIContent("File to Lock"), _fileToLock);
        if (GUILayout.Button("Lock File"))
        {
            FireLockFile();
        }*/
    }

    /*private void FireLockFile()
    {
        if (_fileToLock == string.Empty)
        {
            Debug.LogError("File can not be empty!");
            return;
        }

        OnLockFile.Invoke(_fileToLock);
    }*/

    private void FireStartSceneGet()
    {
        if (_sourceBranch == string.Empty || _targetBranch == string.Empty)
        {
            Debug.LogError("Branch name can not be empty!");
            return;
        }

        OnStartSceneGet.Invoke(_targetBranch, _sourceBranch);
    }
}