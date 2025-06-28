using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using ClickEvent = UnityEngine.UIElements.ClickEvent;
using Object = UnityEngine.Object;

public class GitinityUI : EditorWindow
{
    // Setup and Settings
    private TextField UserEmail => rootVisualElement.Q<TextField>("user-email");
    private TextField GitExe => rootVisualElement.Q<TextField>("git-exe");
    private TextField DefaultBranch => rootVisualElement.Q<TextField>("default-branch-name");
    private TextField RemoteLink => rootVisualElement.Q<TextField>("remote-link");
    private TextField DiffObjPath => rootVisualElement.Q<TextField>("diff-obj-path");
    private Button SetUpBtn => rootVisualElement.Q<Button>("setup-btn");
    //
    
    // Merge Request Features
    // private Label FeatureState => rootVisualElement.Q<Label>("feature-state");
    private TextField FeatureState => rootVisualElement.Q<TextField>("feature-status");
    private TextField FeatureName => rootVisualElement.Q<TextField>("feature-name");
    private Button StartFeatureBtn => rootVisualElement.Q<Button>("start-feat-btn");
    private Button CheckFeatureBtn => rootVisualElement.Q<Button>("check-feat-btn");
    //
   
    // Merge Options 
    private Button MergeBtn => rootVisualElement.Q<Button>("merge-btn");
    private DropdownField SourceBranchDropDown => rootVisualElement.Q<DropdownField>("source-branch-dd");
    private DropdownField TargetBranchDropDown => rootVisualElement.Q<DropdownField>("target-branch-dd");
    //
    
    // File Locking
    private Label WarnLabel => rootVisualElement.Q<Label>("warn-label");
    private ObjectField LockFile => rootVisualElement.Q<ObjectField>("lock-file");
    private Button LockBtn => rootVisualElement.Q<Button>("lock-btn");
    private Button UnlockBtn => rootVisualElement.Q<Button>("unlock-btn"); 
    private Toggle UseFileLocking => rootVisualElement.Q<Toggle>("use-locking");
    //
    
    
    // Asset VCS
    private Toggle UseAssetVCS => rootVisualElement.Q<Toggle>("use-asset-vcs");
    //

    
    // private Button RequestAccessBtn => rootVisualElement.Q<Button>("request-btn");


    public static event Action OnSetup;
    public static event Action GetGitInfo;
    // public static event Action<string> OnGetFeatureInfo;
    public static event Action OnGetFeatureInfo;
    public static event Action<bool> OnActivateAssetVCS;
    public static event Action<string, string> OnMerge;
    public static event Action<string> OnLockFile;
    public static event Action<string> OnUnlockFile;
    public static event Action<string> OnStartFeature;
    // public static event Action OnStartFeature;

    private string _sourceBranch;
    private string _targetBranch;

    private string _fileToLock;
    private string _featureName;


    [MenuItem("Tools/GitinityUI")]
    public static void ShowWindow()
    {
        GitinityUI wnd = GetWindow<GitinityUI>();
        wnd.titleContent = new GUIContent("🚀Gitinity Control🚀");
    }

    public void CreateGUI()
    {
        FileLocking.OnFileIsLocked += ReactWhenFileIsLocked;
        
        VisualElement root = rootVisualElement;
        VisualTreeAsset asset = Resources.Load<VisualTreeAsset>("GitinityUI");
        asset.CloneTree(root);

        // Setup and Settings
        UserEmail.SetValueWithoutNotify(GlobalRefs.filePaths.userEmail);
        UserEmail.RegisterValueChangedCallback(UpdateUser);

        GitExe.SetValueWithoutNotify(GlobalRefs.filePaths.gitBashExe);
        DefaultBranch.SetValueWithoutNotify(GlobalRefs.filePaths.defaultBranchName);
        DefaultBranch.RegisterValueChangedCallback(evt => GlobalRefs.filePaths.defaultBranchName = evt.newValue);
        RemoteLink.RegisterValueChangedCallback(UpdateRemoteLink);

        RemoteLink.SetValueWithoutNotify(GlobalRefs.filePaths.remoteUrl);
        RemoteLink.RegisterValueChangedCallback(UpdateRemoteLink);

        DiffObjPath.SetValueWithoutNotify(GlobalRefs.filePaths.diffPrefabsDirName);
        DiffObjPath.RegisterValueChangedCallback(UpdateDiffPath);

        SetUpBtn.RegisterCallback<ClickEvent>(FireSetup);

        
        // Merge Request Features
        FeatureState.value = GetFeatureInfo();
        FeatureName.SetValueWithoutNotify(GlobalRefs.currFeatureName);
        FeatureName.RegisterValueChangedCallback(evt => _featureName = evt.newValue);
        CheckFeatureBtn.RegisterCallback<ClickEvent>(evt =>  FeatureState.value = GetFeatureInfo()); 
        StartFeatureBtn.RegisterCallback<ClickEvent>(evt =>
        {
            if (String.IsNullOrEmpty(_featureName))
            {
                Debug.Log($"No name for feature provided - return.");
                return;
            }
            OnStartFeature.Invoke(_featureName);
        });
        
        
        MergeBtn.RegisterCallback<ClickEvent>(evt => OnMerge.Invoke(_targetBranch, _sourceBranch));

        LockBtn.RegisterCallback<ClickEvent>(evt =>
        {
            if (!string.IsNullOrEmpty(_fileToLock))
            {
                OnLockFile.Invoke(_fileToLock);
            }
            else { Debug.LogError("No file provided.");}
        });
        UnlockBtn.RegisterCallback<ClickEvent>(evt =>
        {
            if (!string.IsNullOrEmpty(_fileToLock))
            {
                OnUnlockFile.Invoke(_fileToLock);
            }
            else {Debug.LogError("No file provided.");}
        });


        UseFileLocking.SetValueWithoutNotify(GlobalRefs.filePaths.useFileLocking);
        UseFileLocking.RegisterValueChangedCallback(evt => GlobalRefs.filePaths.useFileLocking = evt.newValue);
        LockFile.RegisterValueChangedCallback(UpdateLockFile);

        var branchNames = GetBranches();
        TargetBranchDropDown.choices = branchNames;
        TargetBranchDropDown.RegisterValueChangedCallback(SelectTargetBranch);

        SourceBranchDropDown.choices = branchNames;
        SourceBranchDropDown.RegisterValueChangedCallback(SelectSourceBranch);

        // RequestAccessBtn.RegisterCallback<ClickEvent>((evt) => Debug.Log($"This could go out to the coworkers"));

        WarnLabel.AddToClassList("hidden");
        
        UseAssetVCS.SetValueWithoutNotify(GlobalRefs.filePaths.useAssetVCS);
        UseAssetVCS.RegisterValueChangedCallback(evt =>
        {
            GlobalRefs.filePaths.useAssetVCS = evt.newValue;
            OnActivateAssetVCS.Invoke(evt.newValue);
        });
    }

    private string GetFeatureInfo()
    {
        if (String.IsNullOrEmpty(GlobalRefs.currFeatureName))
        {
            return "No current feature present.";
        }
        OnGetFeatureInfo.Invoke();
        return GlobalRefs.isFeatureMerged ? "Merged." : "Not merged yet.";
    }

    private void ReactWhenFileIsLocked(string message)
    {
        WarnLabel.text = message;
        WarnLabel.RemoveFromClassList("hidden");
    }


    private void FireSetup(ClickEvent _)
    {
        var state = GlobalRefs.StateObj.State;
        if (state == State.PostInit)
        {
            return;
        }
        OnSetup.Invoke();
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
                list.Remove(GlobalRefs.lockingBranch);
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

    private void UpdateDiffPath(ChangeEvent<string> evt)
    {
        GlobalRefs.filePaths.diffPrefabsDirName = evt.newValue;
        Debug.Log($"Updated Diff Prefabs path to: {GlobalRefs.filePaths.DiffPrefabsDirectory}");
    }

    private void UpdateUser(ChangeEvent<string> evt)
    {
        GlobalRefs.filePaths.userEmail = evt.newValue;
        Debug.Log($"Updated user email to: {GlobalRefs.filePaths.userEmail}");
    }

    private void UpdateLockFile(ChangeEvent<Object> evt)
    {
        _fileToLock = evt.newValue.name;
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