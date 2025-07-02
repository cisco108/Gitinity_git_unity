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
   
    // Asset VCS
    private Toggle UseAssetVCS => rootVisualElement.Q<Toggle>("use-asset-vcs");
    //

    
    // private Button RequestAccessBtn => rootVisualElement.Q<Button>("request-btn");


    public static event Action OnSetup;
    public static void FireOnSetup() => OnSetup.Invoke();
    public static event Action GetGitInfo;
    // public static event Action<string> OnGetFeatureInfo;
    public static event Action OnGetFeatureInfo;
    public static event Action<bool> OnActivateAssetVCS;
    public static event Action<string, string> OnMerge;
    public static event Action<string> OnLockFile;

    public static void FireOnLockFile(string fileToLock) => OnLockFile.Invoke(fileToLock);
    
    public static event Action<string> OnUnlockFile;
    public static void FireOnUnlockFile(string fileToLock) => OnUnlockFile.Invoke(fileToLock);
    public static event Action<string> OnStartFeature;
    // public static event Action OnStartFeature;
    
    private string _sourceBranch;
    private string _targetBranch;

    private string _featureName;


    [MenuItem("Tools/GitinityUI")]
    public static void ShowWindow()
    {
        GitinityUI wnd = GetWindow<GitinityUI>();
        wnd.titleContent = new GUIContent("🚀Gitinity Control🚀");
    }

    public void CreateGUI()
    {
        
        VisualElement root = rootVisualElement;
        VisualTreeAsset asset = Resources.Load<VisualTreeAsset>("GitinityUI");
        asset.CloneTree(root);
        
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

        var branchNames = GetBranches();
        TargetBranchDropDown.choices = branchNames;
        TargetBranchDropDown.RegisterValueChangedCallback(SelectTargetBranch);

        SourceBranchDropDown.choices = branchNames;
        SourceBranchDropDown.RegisterValueChangedCallback(SelectSourceBranch);

        // RequestAccessBtn.RegisterCallback<ClickEvent>((evt) => Debug.Log($"This could go out to the coworkers"));

        
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