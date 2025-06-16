using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using Object = UnityEngine.Object;

//TODO:
/*
 * Add folder structure, with configurable rules:
 *  - Texutres:
 *      - texturRules.scriptableObject (res, polycount, ...)
 *      - hello.png
 *      - sers.jpg
 * - Meshes:
 *      - meshRules.scriptObj
 */
public class AssetVCS
{
    private string _prefix;
    private ICommandBuilder _commandBuilder;
    private ITerminalInterface _terminal;
    public AssetVCS(ITerminalInterface terminal, ICommandBuilder commandBuilder)
    {
        _terminal = terminal;
        _commandBuilder = commandBuilder;
        
        Selection.selectionChanged += OnSelectionChanged;
        _prefix = GlobalRefs.filePaths.versionControlledAssets; 
    }

    private void OnSelectionChanged()
    {
        Object selectedObj = Selection.activeObject;
        
        
        string path = AssetDatabase.GetAssetPath(selectedObj);
        
        Debug.Log("Asset selected: " + path);
        bool controlled = path.StartsWith(_prefix);
        // Debug.Log($"Asset is under AssetVCS? {controlled}");

        if (controlled)
        {
            string getVersionsCmd = _commandBuilder.GetLogOfFile(path);
            string[] versions = _terminal.ExecuteResultToStringArr(getVersionsCmd);

            string name = selectedObj.name;
            (string metadata, bool isValid) = GetAssetMetadata(path);
            
            AssetVCSEditorWindow.ShowWindow(name, versions, path, UpdateVersion, SaveChanges, metadata, isValid);
        }
    }
    
 
private (string info, bool isValid) GetAssetMetadata(string path)
{
    string extension = Path.GetExtension(path).ToLower();
    string metadata = "";
    bool isValid = true;
    
    float expectedScale = 1f;
    int expectedMaxVertexCount = 20;
    int expectedWidth = 1024;
    int hexpectedHeight = 1024;
    // AudioClip audioClip = null;

    if (extension == ".fbx")
    {
        var importer = AssetImporter.GetAtPath(path) as ModelImporter;
        if (importer != null)
        {
            var scale = importer.globalScale;
            metadata += $"Scale Factor: {scale}\n";
            isValid = Mathf.Approximately(scale, expectedScale);
            
            // Load the asset to extract mesh info
            var assetObjs = AssetDatabase.LoadAllAssetsAtPath(path);
            int totalVertices = 0;
            foreach (var obj in assetObjs)
            {
                if (obj is Mesh mesh)
                {
                    totalVertices += mesh.vertexCount;
                }
            }
 
            metadata += $"Total Vertex Count: {totalVertices}\n";
            if (totalVertices > expectedMaxVertexCount)
            {
                isValid = false;
                metadata += "⚠️ Very high vertex count. Consider optimizing.\n";
            }           
        }
    }
    else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
    {
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (texture != null)
        {
            float width = texture.width;
            float height = texture.height;
            metadata += $"Resolution: {width} × {height}\n";
            isValid = Mathf.Approximately(width, expectedWidth) && Mathf.Approximately(height, hexpectedHeight);
        }
    }

    var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
    if (textureImporter != null)
    {
        metadata += $"Texture Format: {textureImporter.textureCompression}\n";
    }
/*
    var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
    if (audioClip != null)
    {
        metadata += $"Sample Rate: {audioClip.frequency} Hz\n";
        metadata += $"Channels: {audioClip.channels}\n";
    }
    */

    return (string.IsNullOrWhiteSpace(metadata) ? "No additional metadata found." : metadata, isValid);
}   
    
    
    
    
    
    


    private void UpdateVersion(string versionCommit, string path)
    {
        string hash = versionCommit.Remove(7);

        string checkoutCmd = _commandBuilder.GetCheckout(hash, path);
        Debug.Log(checkoutCmd);
        _terminal.Execute(checkoutCmd);
        AssetDatabase.Refresh();
    }

    private void SaveChanges(string versionCommit, string path)
    {
         string hash = versionCommit.Remove(7);

         string commitCmd = _commandBuilder.GetCommit(path, $"Changed {path} to version {hash}");
         Debug.Log(commitCmd);
         _terminal.Execute(commitCmd);       
    }
}


public class AssetVCSEditorWindow : EditorWindow
{
    private string _assetName;
    private string[] _versions;
    private int _selectedIndex = 0;
    private string _pathOfContainedAsset;
    private string _metadataInfo;
    private bool _isValid;

    public event Action<string, string> OnUpdateVersion;
    public event Action<string, string> OnSaveChanges;

    public static void ShowWindow(string assetName, string[] versions, string path,
        Action<string, string> onUpdate, Action<string, string> onSave, string metadataInfo, bool isValid)
    {
        AssetVCSEditorWindow window = GetWindow<AssetVCSEditorWindow>("Asset VCS");
        window._assetName = assetName;
        window._versions = versions;
        window._pathOfContainedAsset = path;
        window.OnUpdateVersion = onUpdate;
        window.OnSaveChanges = onSave;
        window._metadataInfo = metadataInfo;
        window._isValid = isValid;
        window.minSize = new Vector2(450, 180);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label($"Version Selection: {_assetName}", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        if (_versions is null)
        {
            Debug.Log($"Asset: {_assetName} has no versions yet.");
            return;
        }
        _selectedIndex = EditorGUILayout.Popup("Select Version", _selectedIndex, _versions);

        if (GUILayout.Button("Switch Version"))
        {
            OnUpdateVersion.Invoke(_versions[_selectedIndex], _pathOfContainedAsset);
        }

        if (GUILayout.Button("Save Changes"))
        {
            OnSaveChanges.Invoke(_versions[_selectedIndex], _pathOfContainedAsset);
        }
        
        EditorGUILayout.Space();
        GUILayout.Label("Asset Metadata", EditorStyles.boldLabel);
        var messageType = _isValid ? MessageType.Info : MessageType.Error;
        EditorGUILayout.HelpBox(_metadataInfo, messageType);
    }



    private string GetAssetMetadata(string path)
    {
        string extension = Path.GetExtension(path).ToLower();
        string metadata = "";

        if (extension == ".fbx")
        {
            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer != null)
            {
                metadata += $"Scale Factor: {importer.globalScale}\n";
            }
        }
        else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
        {
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture != null)
            {
                metadata += $"Resolution: {texture.width} × {texture.height}\n";
            }
        }

        var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if (textureImporter != null)
        {
            metadata += $"Texture Format: {textureImporter.textureCompression}\n";
        }

        var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
        if (audioClip != null)
        {
            metadata += $"Sample Rate: {audioClip.frequency} Hz\n";
            metadata += $"Channels: {audioClip.channels}\n";
        }

        return string.IsNullOrWhiteSpace(metadata) ? "No additional metadata found." : metadata;
    }
}