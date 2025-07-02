using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class FileLockUI : EditorWindow
{
    // File Locking
    private Label WarnLabel => rootVisualElement.Q<Label>("warn-label");
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
        FileLocking.OnFileIsLocked += ReactWhenFileIsLocked;
        
        VisualElement root = rootVisualElement;
        VisualTreeAsset asset = Resources.Load<VisualTreeAsset>("FileLockUI");
        asset.CloneTree(root);

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

    
    
    private void ReactWhenFileIsLocked(string message)
    {
        WarnLabel.text = message;
        WarnLabel.RemoveFromClassList("hidden");
    }
    
    private void UpdateLockFile(ChangeEvent<Object> evt)
    {
        _fileToLock = evt.newValue.name;
    }
}