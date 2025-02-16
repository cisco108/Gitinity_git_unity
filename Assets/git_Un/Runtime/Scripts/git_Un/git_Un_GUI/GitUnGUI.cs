using System;
using UnityEditor;
using UnityEngine;

public class GitUnGUI : EditorWindow
{
    [MenuItem("Window/git_Un_GUI")]
    private static void Init() => GetWindow<GitUnGUI>(true, "git_Un_GUI");

    public static event Action<string, string> OnStartSceneGet;
    public static event Action<string> OnLockFile;
    public static event Action<string, string> OnGetDiffFromSh;
    private string _sourceBranch = string.Empty;
    private string _targetBranch = string.Empty;

    private string _fileToLock = string.Empty;

    private void OnGUI()
    {
        _targetBranch = EditorGUILayout.TextField(new GUIContent("target branch"), _targetBranch);
        _sourceBranch = EditorGUILayout.TextField(new GUIContent("source branch"), _sourceBranch);


        if (GUILayout.Button("START"))
        {
            FireStartSceneGet();
        }

        _fileToLock = EditorGUILayout.TextField(new GUIContent("File to Lock"), _fileToLock);
        if (GUILayout.Button("Lock File"))
        {
            FireLockFile();
        }
    }

    private void FireLockFile()
    {
        if (_fileToLock == string.Empty)
        {
            Debug.LogError("File can not be empty!");
            return;
        }

        OnLockFile.Invoke(_fileToLock);
    }

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