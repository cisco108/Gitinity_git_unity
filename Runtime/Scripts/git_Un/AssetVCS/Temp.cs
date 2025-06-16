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