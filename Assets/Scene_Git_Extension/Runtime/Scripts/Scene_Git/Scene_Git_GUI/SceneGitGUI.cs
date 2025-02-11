using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneGitGUI : EditorWindow
{
    [MenuItem("Window/SceneGet_GUI")]
    private static void Init() => GetWindow<SceneGitGUI>(true, "SceneGet");

    public static event Action<string, string> OnStartSceneGet;
    public static event Action<string,string> OnGetDiffFromSh;
    private string _sourceBranch = string.Empty;
    private string _targetBranch = string.Empty;

    private void OnGUI()
    {
        _targetBranch = EditorGUILayout.TextField(new GUIContent("target branch"), _targetBranch);
        _sourceBranch = EditorGUILayout.TextField(new GUIContent("source branch"), _sourceBranch);


        if (GUILayout.Button("Get Scene content back"))
        {
            FireStartSceneGet();
        }

        if (GUILayout.Button("Get Diff from shell"))
        {
            FireGetDiff();
        }
    }

    private void FireGetDiff()
    {
        if (_sourceBranch == string.Empty)
        {
            Debug.LogError("Branch name can not be empty!");
            return;
        }

        OnGetDiffFromSh.Invoke(_targetBranch, _sourceBranch);
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