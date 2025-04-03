using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class GitinityUI : EditorWindow
{
    private TextField GitExe => rootVisualElement.Q<TextField>("git-exe");
    private TextField RemoteLink => rootVisualElement.Q<TextField>("remote-link");
    private TextField DiffObjPath => rootVisualElement.Q<TextField>("diff-obj-path");
    private Button SetUpBtn => rootVisualElement.Q<Button>("setup-btn");
    private Button MergeBtn => rootVisualElement.Q<Button>("merge-btn");
    private Label WarnLabel => rootVisualElement.Q<Label>("warn-label");
    private ObjectField LockFile => rootVisualElement.Q<ObjectField>("lock-file");
    private Button LockBtn => rootVisualElement.Q<Button>("lock-btn");
    private DropdownField SourceBranchDropDown => rootVisualElement.Q<DropdownField>("source-branch-dd");
    private DropdownField TargetBranchDropDown => rootVisualElement.Q<DropdownField>("target-branch-dd");
    private Button RequestAccessBtn => rootVisualElement.Q<Button>("request-btn");


    public static event Action OnSetup;
    public static event Action GetGitInfo;
    public static event Action<string, string> OnMerge;
    public static event Action OnLockFile;

    private string _sourceBranch;
    private string _targetBranch;


    [MenuItem("Tools/GitinityUI")]
    public static void ShowWindow()
    {
        GitinityUI wnd = GetWindow<GitinityUI>();
        wnd.titleContent = new GUIContent("🚀Gitinity Control🚀");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            "Assets/git_Un/Runtime/Scripts/git_Un/git_Un_GUI/Editor/GitinityUI.uxml");

        asset.CloneTree(root);

        GitExe.SetValueWithoutNotify(GlobalRefs.filePaths.gitBashExe);
        GitExe.RegisterValueChangedCallback(UpdateRemoteLink);
        RemoteLink.SetValueWithoutNotify(GlobalRefs.filePaths.remoteUrl);
        RemoteLink.RegisterValueChangedCallback(UpdateRemoteLink);
        DiffObjPath.SetValueWithoutNotify(GlobalRefs.filePaths.diffPrefabsDirName);
        DiffObjPath.RegisterValueChangedCallback(UpdateRemoteLink);
        SetUpBtn.RegisterCallback<ClickEvent>(_ => OnSetup.Invoke());

        MergeBtn.RegisterCallback<ClickEvent>(evt => OnMerge.Invoke(_targetBranch, _sourceBranch));

        LockBtn.RegisterCallback<ClickEvent>(evt => OnLockFile.Invoke());

        LockFile.RegisterValueChangedCallback(UpdateLockFile);

        var branchNames = GetBranches();
        TargetBranchDropDown.choices = branchNames;
        TargetBranchDropDown.RegisterValueChangedCallback(SelectTargetBranch);

        SourceBranchDropDown.choices = branchNames;
        SourceBranchDropDown.RegisterValueChangedCallback(SelectSourceBranch);

        RequestAccessBtn.RegisterCallback<ClickEvent>((evt) => Debug.Log($"This could go out to the coworkers"));
    }

    private List<string> GetBranches()
    {
        GetGitInfo.Invoke();

        var state = GlobalRefs.StateObj.State;
        switch (state)
        {
            case State.PostInit:
                var names = GlobalRefs.StateObj.BranchNames;
                List<string> list = new List<string>(names);
                return list;

            default:
                return new List<string>() { "example1", "example2", "example3" };
        }
    }

    private void UpdateRemoteLink(ChangeEvent<string> evt)
    {
        GlobalRefs.filePaths.remoteUrl = evt.newValue;
        Debug.Log($"Updated remote url to: {GlobalRefs.filePaths.remoteUrl}");
    }

    private void UpdateLockFile(ChangeEvent<Object> evt)
    {
        GlobalRefs.filePaths.fileToLockName = evt.newValue.name;
        Debug.Log($"Updated file to lock to: {GlobalRefs.filePaths.fileToLockName}");
    }

    private void SelectSourceBranch(ChangeEvent<string> evt)
    {
        _sourceBranch = evt.newValue;
        Debug.Log($"source branch: {_sourceBranch}");
    }

    private void SelectTargetBranch(ChangeEvent<string> evt)
    {
        _targetBranch = evt.newValue;
        Debug.Log($"target branch: {_targetBranch}");
    }

}