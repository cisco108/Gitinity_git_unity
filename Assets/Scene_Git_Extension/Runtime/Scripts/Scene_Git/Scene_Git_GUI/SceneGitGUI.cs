using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneGitGUI : EditorWindow
{
    [MenuItem("Window/SceneGet_GUI")]
    private static void Init() => GetWindow<SceneGitGUI>(true, "SceneGet");

    public static event Action<string> OnStartSceneGet;
    public static event Action<string> OnGetDiffFromSh;
    private string _branchName = string.Empty;

    private void OnGUI()
    {
        _branchName = EditorGUILayout.TextField(new GUIContent("merged branch"), _branchName);


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
        if (_branchName == string.Empty)
        {
            Debug.LogError("Branch name can not be empty!");
            return;
        }

        OnGetDiffFromSh.Invoke(_branchName);
    }

    private void FireStartSceneGet()
    {
        if (_branchName == string.Empty)
        {
            Debug.LogError("Branch name can not be empty!");
            return;
        }

        OnStartSceneGet.Invoke(_branchName);
    }
}