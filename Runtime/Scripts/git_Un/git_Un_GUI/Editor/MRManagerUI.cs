using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MRManagerUI : EditorWindow
{ 
    private TextField FeatureState => rootVisualElement.Q<TextField>("feature-status");
    private TextField FeatureName => rootVisualElement.Q<TextField>("feature-name");
    private Button StartFeatureBtn => rootVisualElement.Q<Button>("start-feat-btn");
    private Button CheckFeatureBtn => rootVisualElement.Q<Button>("check-feat-btn");
    private string _featureName;
    // private Button RequestAccessBtn => rootVisualElement.Q<Button>("request-btn");

    [MenuItem("Tools/Gitinity/Feature Management")]
    public static void ShowWindow()
    {
        MRManagerUI wnd = GetWindow<MRManagerUI>();
        wnd.titleContent = new GUIContent("Feature Management");
    }

    public void CreateGUI()
    {
        
        VisualElement root = rootVisualElement;
        VisualTreeAsset asset = Resources.Load<VisualTreeAsset>("MRManagerUI");
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
            GitinityUI.FireOnStartFeature(_featureName);
        });
        
       // RequestAccessBtn.RegisterCallback<ClickEvent>((evt) => Debug.Log($"This could go out to the coworkers"));
       
    }

    private string GetFeatureInfo()
    {
        if (String.IsNullOrEmpty(GlobalRefs.currFeatureName))
        {
            return "No current feature present.";
        }
        GitinityUI.FireOnGetFeatureInfo();
        return GlobalRefs.isFeatureMerged ? "Merged." : "Not merged yet.";
    }

}