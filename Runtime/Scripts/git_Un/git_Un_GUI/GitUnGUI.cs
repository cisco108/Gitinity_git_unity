using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEngine.UIElements.PopupWindow;

public class GitUnGUI : EditorWindow
{
    [MenuItem("Window/Gitinity Version Control")]
    private static void Init() => GetWindow<GitUnGUI>(true, "git_Un_GUI");

    public static event Action<string, string> OnStartSceneGet;
    public static event Action OnLockFile;
    public static event Action OnSetupGitUn;
    public static event Action InitGitDataObject;

    private string _fileToLock = string.Empty;
    private string _sourceBranch = string.Empty;
    private string _targetBranch = string.Empty;
    private int _targetSelection = 0;
    private int _sourceSelection = 0;
    private bool _isSetup = false;
    private bool _guiUseFileLocking = false;

    private static StateObj _dataObject;

    private string[] _branchNames = {"Not setup yet!"};

    private int foo = 0;

    private void CreateGUI()
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
        if (GUILayout.Button("Lock File"))
        {
            FireLockFile();
        }
        
        GUILayout.Space(50);
        if (GUILayout.Button("Setup git Un"))
        {
            FireSetup();
        }

        GUILayout.Space(50);
        BranchDropdown();
        if (GUILayout.Button("START"))
        {
            FireStartSceneGet();
        }

        GUILayout.Space(50);
        _guiUseFileLocking = GUILayout.Toggle(_guiUseFileLocking, "Use FileLocking");


        // GUILayout.Space(50);
        // _fileToLock = EditorGUILayout.TextField(new GUIContent("file to lock"), _fileToLock);

    }

    private void FireLockFile()
    {
        OnLockFile.Invoke();
    }

    private void FireSetup()
    {
        Debug.Log("Fire setup");
        _isSetup = true;
        OnSetupGitUn.Invoke();
    }

    private void BranchDropdown()
    {
        if (_dataObject == null)
        {
            foo++;
            Debug.Log(foo);

            if (_isSetup)
            {
                InitGitDataObject.Invoke();
                _branchNames = _dataObject.BranchNames;
            }
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

    

    private void OnDestroy()
    {
        _dataObject = null;
        Debug.Log($"OnDestroy");
    }
}
