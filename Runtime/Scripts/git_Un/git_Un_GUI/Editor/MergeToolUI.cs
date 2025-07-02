using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MergeToolUI : EditorWindow
{
    // Merge Options 
    private Button MergeBtn => rootVisualElement.Q<Button>("merge-btn");
    private DropdownField SourceBranchDropDown => rootVisualElement.Q<DropdownField>("source-branch-dd");
    private DropdownField TargetBranchDropDown => rootVisualElement.Q<DropdownField>("target-branch-dd");
    private Button DocsButton => rootVisualElement.Q<Button>("doc-link");
    //


    private string _sourceBranch;
    private string _targetBranch;

    [MenuItem("Tools/Gitinity/Merge Tool")]
    public static void ShowWindow()
    {
        MergeToolUI wnd = GetWindow<MergeToolUI>();
        wnd.titleContent = new GUIContent("Merge Tool");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        VisualTreeAsset asset = Resources.Load<VisualTreeAsset>("MergeToolUI");
        asset.CloneTree(root);

        MergeBtn.RegisterCallback<ClickEvent>(evt => GitinityUI.FireOnMerge(_targetBranch, _sourceBranch));

        var branchNames = GetBranches();
        TargetBranchDropDown.choices = branchNames;
        TargetBranchDropDown.RegisterValueChangedCallback(SelectTargetBranch);

        SourceBranchDropDown.choices = branchNames;
        SourceBranchDropDown.RegisterValueChangedCallback(SelectSourceBranch);

        DocsButton.clicked += () => { Application.OpenURL("https://free-elective-docu-5e29a0.h-da.io/merge_tool/"); };
    }

    private List<string> GetBranches()
    {
        GitinityUI.FireOnGetGitInfo();

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