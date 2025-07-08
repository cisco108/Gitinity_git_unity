using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Label = UnityEngine.UIElements.Label;

public class FileLockUI : EditorWindow
{
    // File Locking
    private Label WarnLabel => rootVisualElement.Q<Label>("warn");
    private ObjectField LockFile => rootVisualElement.Q<ObjectField>("lock-file");
    private Button LockBtn => rootVisualElement.Q<Button>("lock-btn");
    private Button UnlockBtn => rootVisualElement.Q<Button>("unlock-btn");
    private Toggle UseFileLocking => rootVisualElement.Q<Toggle>("use-locking");
    private Button DocsButton => rootVisualElement.Q<Button>("doc-link");


    private string _sourceBranch;
    private string _targetBranch;
    private string _fileToLock;


    [MenuItem("Tools/Gitinity/File Locking")]
    public static void ShowWindow()
    {
        FileLockUI wnd = GetWindow<FileLockUI>();
        wnd.titleContent = new GUIContent("File Locking");
    }
    
    
    
    public void CreateGUI()
    {
        FileLocking.OnFileIsOpened += ReactWhenFileIsOpened;
        
        VisualElement root = rootVisualElement;
        VisualTreeAsset asset = Resources.Load<VisualTreeAsset>("FileLockUI");
        asset.CloneTree(root);

        // WarnLabel.AddToClassList("hidden");
        WarnLabel.style.display = DisplayStyle.None;
        
        LockBtn.RegisterCallback<ClickEvent>(evt =>
        {
            if (!string.IsNullOrEmpty(_fileToLock))
            {
                GitinityUI.FireOnLockFile(_fileToLock);
            }
            else { Debug.LogError("No file provided.");}
        });
        UnlockBtn.RegisterCallback<ClickEvent>(evt =>
        {
            if (!string.IsNullOrEmpty(_fileToLock))
            {
                GitinityUI.FireOnUnlockFile(_fileToLock);
            }
            else {Debug.LogError("No file provided.");}
        });


        UseFileLocking.SetValueWithoutNotify(GlobalRefs.filePaths.useFileLocking);
        UseFileLocking.RegisterValueChangedCallback(evt => GlobalRefs.filePaths.useFileLocking = evt.newValue);
        LockFile.RegisterValueChangedCallback(UpdateLockFile);
        
        DocsButton.clicked += () => { Application.OpenURL("https://free-elective-docu-5e29a0.h-da.io/file_locking/"); };
   }

    
    
    private void ReactWhenFileIsOpened(bool isLocked, string message)
    {
        Debug.Log($"ReactWhenFileIsOpened GUI {message}, {isLocked}");
        if (isLocked)
        {
            WarnLabel.text = message;
            WarnLabel.style.display = DisplayStyle.Flex;
            // WarnLabel.RemoveFromClassList("hidden");
        }
        else
        {
            WarnLabel.style.display = DisplayStyle.None;
            // WarnLabel.AddToClassList("hidden");
        }
        
    }
    
    private void UpdateLockFile(ChangeEvent<Object> evt)
    {
        _fileToLock = evt.newValue.name;
    }
}