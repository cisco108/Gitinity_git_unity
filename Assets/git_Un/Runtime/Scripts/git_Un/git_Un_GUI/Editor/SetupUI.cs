using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SetupUI : EditorWindow
{ 
    // Setup and Settings
    private TextField UserEmail => rootVisualElement.Q<TextField>("user-email");
    private TextField GitExe => rootVisualElement.Q<TextField>("git-exe");
    private TextField DefaultBranch => rootVisualElement.Q<TextField>("default-branch-name");
    private TextField RemoteLink => rootVisualElement.Q<TextField>("remote-link");
    private TextField DiffObjPath => rootVisualElement.Q<TextField>("diff-obj-path");
    private Button SetUpBtn => rootVisualElement.Q<Button>("setup-btn");
    private Toggle UseAssetVCS => rootVisualElement.Q<Toggle>("use-asset-vcs");
    //


    [MenuItem("Tools/Gitinity/Setup and Settings")]
    public static void ShowWindow()
    {
        SetupUI wnd = GetWindow<SetupUI>();
        wnd.titleContent = new GUIContent("Gitinity Setup and Settings");
    }
    
    public void CreateGUI()
    {

        VisualElement root = rootVisualElement;
        VisualTreeAsset asset = Resources.Load<VisualTreeAsset>("SetupUI");
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
        
        UseAssetVCS.SetValueWithoutNotify(GlobalRefs.filePaths.useAssetVCS);
        UseAssetVCS.RegisterValueChangedCallback(evt =>
        {
            GlobalRefs.filePaths.useAssetVCS = evt.newValue;
            GitinityUI.FireOnActivateAssetVCS(evt.newValue);
        });
    }
    
    private void FireSetup(ClickEvent _)
    {
        var state = GlobalRefs.StateObj.State;
        if (state == State.PostInit)
        {
            Debug.Log("Gitinity: Already set up.");
            return;
        }
        GitinityUI.FireOnSetup();
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

}