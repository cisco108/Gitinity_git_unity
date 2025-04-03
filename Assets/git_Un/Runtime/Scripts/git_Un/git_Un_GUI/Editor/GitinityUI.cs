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
    private Label WarnLabel => rootVisualElement.Q<Label>("warn-label");
    private ObjectField LockFile => rootVisualElement.Q<ObjectField>("lock-file");
    private Button LockBtn => rootVisualElement.Q<Button>("lock-btn");
    private DropdownField SourceBranchDropDown => rootVisualElement.Q<DropdownField>("source-branch-dd");
    private DropdownField TargetBranchDropDown => rootVisualElement.Q<DropdownField>("target-branch-dd");
    private Button RequestAccessBtn => rootVisualElement.Q<Button>("request-btn");



    public static event Action GetGitInfo;
    
    
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
        SetUpBtn.RegisterCallback<ClickEvent>(Setup);
        
        
        LockBtn.RegisterCallback<ClickEvent>(Lock);

        LockFile.RegisterValueChangedCallback(UpdateLockFile);

        var branchNames = GetBranches();
        TargetBranchDropDown.choices = branchNames; 
        TargetBranchDropDown.RegisterValueChangedCallback(SelectBranch);

        SourceBranchDropDown.choices = branchNames; 
        SourceBranchDropDown.RegisterValueChangedCallback(SelectBranch);
        
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
                return new List<string>() { "example", "example", "example" };
        }
    }
    private void UpdateRemoteLink(ChangeEvent<string> evt)
    {
        Debug.Log(evt.newValue);
        Debug.LogError($"WRONTGFFFF");
    }

    private void UpdateLockFile(ChangeEvent<Object> evt)
    {
        Debug.Log(evt.newValue);
    }

    private void SelectBranch(ChangeEvent<string> evt)
    {
        Debug.Log(evt.newValue);
        Debug.Log(evt.target);
    }

    private void Lock(ClickEvent _)
    {
        Debug.Log("Lock");
    }

    private void Setup(ClickEvent _)
    {
        Debug.Log("Setup");
    }
}