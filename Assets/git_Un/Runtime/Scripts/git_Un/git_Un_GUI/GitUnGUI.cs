using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEngine.UIElements.PopupWindow;

public class GitUnGUI : EditorWindow
{
    [MenuItem("Window/git_Un_GUI")]
    private static void Init() => GetWindow<GitUnGUI>(true, "git_Un_GUI");

    public static event Action<string, string> OnStartSceneGet;
    public static event Action InitGitDataObject;


    private string _sourceBranch = string.Empty;
    private string _targetBranch = string.Empty;
    private int _targetSelection = 0;
    private int _sourceSelection = 0;

    private static GitDataObject _dataObject;

    private string[] _branchNames;

    private int foo = 0;

    void CreateGUI()
    {
        var editor = (UserConfigEditor)Editor.CreateEditor(UserConfig.instance);
        var editorRoot = editor.CreateInspectorGUI();
        editorRoot.Bind(editor.serializedObject);
        rootVisualElement.Add(editorRoot);
    }

    private void OnGUI()
    {
        //TODO: make nice layout, possibly move also to ScriptableObject
        GUILayout.Space(100);
        BranchDropdown();
        if (GUILayout.Button("START"))
        {
            FireStartSceneGet();
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