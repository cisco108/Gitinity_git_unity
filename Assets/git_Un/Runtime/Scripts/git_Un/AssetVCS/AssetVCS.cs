using UnityEditor;
using UnityEngine;
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
    private AssetValidator _validator;
    public AssetVCS(ITerminalInterface terminal, ICommandBuilder commandBuilder)
    {
        _terminal = terminal;
        _commandBuilder = commandBuilder;
        _validator = new AssetValidator();
        
        Selection.selectionChanged += OnSelectionChanged;
        _prefix = GlobalRefs.filePaths.versionControlledAssets; 
    }

    private void OnSelectionChanged()
    {
        Object selectedObj = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(selectedObj);
        
        Debug.Log("Asset selected: " + path);
        bool controlled = path.StartsWith(_prefix);

        if (controlled)
        {
            string getVersionsCmd = _commandBuilder.GetLogOfFile(path);
            string[] versions = _terminal.ExecuteResultToStringArr(getVersionsCmd);

            string name = selectedObj.name;
            (string metadata, bool isValid) = _validator.ValidateAsset(path);
            
            AssetVCSEditorWindow.ShowWindow(name, versions, path, UpdateVersion, SaveChanges, metadata, isValid);
        }
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

