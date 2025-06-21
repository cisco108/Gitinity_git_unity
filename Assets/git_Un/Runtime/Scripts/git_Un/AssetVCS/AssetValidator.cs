using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class AssetValidator
{
    private ITerminalInterface _terminal;
    private Dictionary<string, (string, bool)> _assetValidStates = new();

    public AssetValidator(ITerminalInterface terminal)
    {
        _terminal = terminal;
        AssetPostprocListener.OnAssetPosPro += () => ValidateAllControlledAssets(); 
    }
    private AssetValidationSettings GetRules(string path)
    {
        int index = path.LastIndexOf('/');
        path = path.Remove(index +1);
        
        string[] guids = AssetDatabase.FindAssets("t:AssetValidationSettings", new[] { path });
        if (guids.Length == 0)
        {
            Debug.LogWarning($"No AssetValidationSettings found in folder: {path}");
            return null;
        }
        
        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        var rules = AssetDatabase.LoadAssetAtPath<AssetValidationSettings>(assetPath);
        return rules;
    }

    private bool ValidateAllControlledAssets()
    {
        Debug.Log($"Validate all Assets");
        bool areAllValid = true;
        
        string targetFolder = GlobalRefs.filePaths.versionControlledAssets;
        string[] guids = AssetDatabase.FindAssets("", new[] { targetFolder });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            (string info, bool isValid) = ValidateAsset(path);
            if (!isValid)
            {
                areAllValid = false;
            }

            _assetValidStates[path] = (info, isValid);
        }
        _terminal.Execute($"echo '{areAllValid.ToString().ToLower()}' > {GlobalRefs.filePaths.allowCommitFile}");
        return areAllValid;
    }

    public (string info, bool isValid) GetInfo(string path)
    {
        //TODO: come up with a way of less validation calls
        ValidateAllControlledAssets();
        return _assetValidStates[path];
    }
    
    /// <summary> Filetypes that are not handles will always return isValid true; </summary>
    private (string info, bool isValid) ValidateAsset(string path)
    {
        string extension = Path.GetExtension(path).ToLower();
        string metadata = "";
        bool isValid = true;

        var rules = GetRules(path);
        if (!rules)
        {
            Debug.LogWarning($"There is no Rules object on in the current directory.");
            return (null, false);
        }

        if (extension == ".fbx")
        {
            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer != null)
            {
                var scale = importer.globalScale;
                metadata += $"Scale Factor: {scale}\n";
                isValid = Mathf.Approximately(scale, rules.expectedScale);

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
                if (totalVertices > rules.maxVertexCount)
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
                isValid = Mathf.Approximately(width, rules.expectedImgWidth) && Mathf.Approximately(height, rules.expectedImgHeight);
            }
        }

        var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if (textureImporter != null)
        {
            metadata += $"Texture Format: {textureImporter.textureCompression}\n";
        }
        
        
        return (string.IsNullOrWhiteSpace(metadata) ? "No additional metadata found." : metadata, isValid);
    }
} 