using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MRManagerUI : EditorWindow
{ 
    private Label FeatureState => rootVisualElement.Q<Label>("status-value");
    private TextField FeatureName => rootVisualElement.Q<TextField>("feature-name");
    private Button StartFeatureBtn => rootVisualElement.Q<Button>("start-feat-btn");
    private Button CheckFeatureBtn => rootVisualElement.Q<Button>("check-feat-btn");
    private Button DocsButton => rootVisualElement.Q<Button>("doc-link");
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
        
        (string info, Color color) = GetFeatureInfo();
        FeatureState.text = info;
        FeatureState.style.color = color;
        
        FeatureName.SetValueWithoutNotify(GlobalRefs.currFeatureName);
        FeatureName.RegisterValueChangedCallback(evt => _featureName = evt.newValue);
        CheckFeatureBtn.RegisterCallback<ClickEvent>(evt =>
        {
            (string i, Color c) = GetFeatureInfo();
            FeatureState.text = i;
            FeatureState.style.color = c;
        }); 
        
        StartFeatureBtn.RegisterCallback<ClickEvent>(evt =>
        {
            if (String.IsNullOrEmpty(_featureName))
            {
                Debug.Log($"No name for feature provided - return.");
                return;
            }
            GitinityUI.FireOnStartFeature(_featureName);
        });
        
        DocsButton.clicked += () => { Application.OpenURL("https://free-elective-docu-5e29a0.h-da.io/mr_man/"); };
       // RequestAccessBtn.RegisterCallback<ClickEvent>((evt) => Debug.Log($"This could go out to the coworkers"));
       
    }

    private (string state, Color color) GetFeatureInfo()
    {
        if (String.IsNullOrEmpty(GlobalRefs.currFeatureName))
        {
            return ("No current feature present.", Color.white);
        }
        GitinityUI.FireOnGetFeatureInfo();
        return GlobalRefs.isFeatureMerged ? ("Merged.", Color.green) : ("Not merged yet.", Color.orange);
    }

}