using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class GitinityUI : EditorWindow
{
    private TextField RemoteLink => rootVisualElement.Q<TextField>("remote-link");
    private Button SetUpBtn => rootVisualElement.Q<Button>("setup-btn");
    private Label WarnLabel => rootVisualElement.Q<Label>("warn-label");
    private ObjectField LockFile => rootVisualElement.Q<ObjectField>("lock-file");
    private Button LockBtn => rootVisualElement.Q<Button>("lock-btn");
    private DropdownField SourceBranchDropDown => rootVisualElement.Q<DropdownField>("source-branch-dd");
    private DropdownField TargetBranchDropDown => rootVisualElement.Q<DropdownField>("target-branch-dd");
    private Button RequestAccessBtn => rootVisualElement.Q<Button>("request-btn");


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

        RemoteLink.RegisterValueChangedCallback(UpdateRemoreLink);
        SetUpBtn.RegisterCallback<ClickEvent>(Setup);
        LockBtn.RegisterCallback<ClickEvent>(Lock);

        LockFile.RegisterValueChangedCallback(UpdateLockFile);

        TargetBranchDropDown.choices = new List<string>() { "master", "feature", "value" };
        TargetBranchDropDown.RegisterValueChangedCallback(SelectBranch);

        SourceBranchDropDown.choices = new List<string>() { "master", "feature", "value" };
        SourceBranchDropDown.RegisterValueChangedCallback(SelectBranch);
        
        RequestAccessBtn.RegisterCallback<ClickEvent>((evt) => Debug.Log($"This could go out to the coworkers"));
    }

    private void UpdateRemoreLink(ChangeEvent<string> evt)
    {
        Debug.Log(evt.newValue);
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