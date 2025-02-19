using System;
using UnityEditor;
using UnityEngine;

public class GitUnGUI : EditorWindow
{
    [MenuItem("Window/git_Un_GUI")]
    private static void Init() => GetWindow<GitUnGUI>(true, "git_Un_GUI");

    public static event Action<string, string> OnStartSceneGet;
    public static event Action InitGitDataObject;

    public static event Action<string[]> OnGetBranches;

    // public static event Action<string> OnLockFile;
    // public static event Action<string, string> OnGetDiffFromSh;
    private string _sourceBranch = string.Empty;
    private string _targetBranch = string.Empty;
    private int _targetSelection = 0;
    private int _sourceSelection = 0;

    private static GitDataObject _dataObject;

    private string[] _branchNames;

    // private string _fileToLock = string.Empty;
    private int foo = 0;

    private bool useCustomSettings;

    private void OnGUI()
    {
        BranchDropdown();
        if (GUILayout.Button("START"))
        {
            FireStartSceneGet();
        }

        if (!useCustomSettings)
        {
            return;
        }
    }

    private void BranchDropdown()
    {
        if (_dataObject == null)
        {
            foo++;
            Debug.Log(foo);
            InitGitDataObject.Invoke();
            _branchNames = _dataObject.BranchNames;
        }

        _targetSelection = EditorGUILayout.Popup(new GUIContent("target branch"), _targetSelection, _branchNames);
        _sourceSelection = EditorGUILayout.Popup(new GUIContent("source branch"), _sourceSelection, _branchNames);

        _targetBranch = _branchNames[_targetSelection];
        _sourceBranch = _branchNames[_sourceSelection];
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

    public static void InitGitDataObj(string[] branchNames)
    {
        if (branchNames != null)
        {
            _dataObject = new GitDataObject(branchNames);
            return;
        }
    }

    private void OnDestroy()
    {
        _dataObject = null;
        Debug.Log($"OnDestroy");
    }
}