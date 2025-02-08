using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneGitGUI : EditorWindow
{
    public static event Action OnStartIt; 
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/SceneGitGUI")]
    public static void ShowExample()
    {
        SceneGitGUI wnd = GetWindow<SceneGitGUI>();
        wnd.titleContent = new GUIContent("SceneGitGUI");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
        
        Button button = new Button(() => OnStartIt.Invoke())
        {
            text = "Start it",
            style =
            {
                width = 120,
                height = 30,
                marginLeft = 10,
                marginTop = 10,
                alignSelf = Align.FlexStart
            }
        };
        root.Add(button);

    }
}