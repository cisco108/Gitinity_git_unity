/*private void OnSelectionChanged()
{
    Object selectedObj = Selection.activeObject;
    if (selectedObj == null) { return; }

    string path = AssetDatabase.GetAssetPath(selectedObj);
    Debug.Log("Asset selected: " + path);

    bool controlled = path.StartsWith(prefix);
    Debug.Log($"Asset is under AssetVCS? {controlled}");

    if (controlled)
    {
        string getVersionsCmd = _commandBuilder.GetLogOfFile(path);
        string[] versions = _terminal.ExecuteResultToStringArr(getVersionsCmd);
        string name = selectedObj.name;

        string metadataInfo = GetAssetMetadata(path);

        AssetVCSEditorWindow.ShowWindow(name, versions, path, UpdateVersion, SaveChanges, metadataInfo);
    }
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
}*/


/*
private string _metadataInfo;

public static void ShowWindow(string assetName, string[] versions, string path,
    Action<string, string> onUpdate, Action<string, string> onSave, string metadata)
{
    AssetVCSEditorWindow window = GetWindow<AssetVCSEditorWindow>("Asset VCS");
    window._assetName = assetName;
    window._versions = versions;
    window._pathOfContainedAsset = path;
    window.OnUpdateVersion = onUpdate;
    window.OnSaveChanges = onSave;
    window._metadataInfo = metadata;
    window.minSize = new Vector2(450, 220);
    window.Show();
}



private void OnGUI()
{
    GUILayout.Label($"Version Selection: {_assetName}", EditorStyles.boldLabel);

    EditorGUILayout.Space();

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
    EditorGUILayout.HelpBox(_metadataInfo, MessageType.Info);
}
private (string info, bool isValid) GetAssetMetadata(string path)
{
    string extension = Path.GetExtension(path).ToLower();
    string metadata = "";
    bool isValid = true;

    float scaleFactor = -1f;
    int width = -1;
    int height = -1;
    AudioClip audioClip = null;

    if (extension == ".fbx")
    {
        var importer = AssetImporter.GetAtPath(path) as ModelImporter;
        if (importer != null)
        {
            scaleFactor = importer.globalScale;
            metadata += $"Scale Factor: {scaleFactor}\n";

            if (scaleFactor <= 0.0001f)
            {
                isValid = false;
                metadata += "⚠️ Scale factor is too small or invalid.\n";
            }
        }
    }
    else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
    {
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (texture != null)
        {
            width = texture.width;
            height = texture.height;
            metadata += $"Resolution: {width} × {height}\n";

            if (width <= 1 || height <= 1)
            {
                isValid = false;
                metadata += "⚠️ Image resolution seems invalid.\n";
            }
        }
    }

    var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
    if (textureImporter != null)
    {
        metadata += $"Texture Format: {textureImporter.textureCompression}\n";
    }

    audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
    if (audioClip != null)
    {
        metadata += $"Sample Rate: {audioClip.frequency} Hz\n";
        metadata += $"Channels: {audioClip.channels}\n";

        if (audioClip.frequency <= 0)
        {
            isValid = false;
            metadata += "⚠️ Audio sample rate is invalid.\n";
        }
    }

    if (string.IsNullOrWhiteSpace(metadata))
    {
        return ("No additional metadata found.", true);
    }

    return (metadata.TrimEnd(), isValid);
}
*/
