using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GitinityUI : EditorWindow
{
    private TextField RemoteLink => rootVisualElement.Q<TextField>("remote-link");
    private Button SetUpBtn => rootVisualElement.Q<Button>("setup-btn");
    private Label WarnLabel => rootVisualElement.Q<Label>("warn-label");
    private TextField LockFile => rootVisualElement.Q<TextField>("lock-file");
    private Button LockBtn=> rootVisualElement.Q<Button>("lock-btn");

    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

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


        /*// Each editor window contains a root VisualElement object
        // VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);*/
    }
}