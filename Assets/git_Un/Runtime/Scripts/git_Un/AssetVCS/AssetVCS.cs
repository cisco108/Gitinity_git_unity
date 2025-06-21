using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetVCS
{
    private string _prefix;
    private string _currentAssetName;
    private ICommandBuilder _commandBuilder;
    private ITerminalInterface _terminal;
    private AssetValidator _validator;
    public bool isActive;
    public AssetVCS(ITerminalInterface terminal, ICommandBuilder commandBuilder)
    {
        _terminal = terminal;
        _commandBuilder = commandBuilder;
        _validator = new AssetValidator(_terminal);
        
        Selection.selectionChanged += OnSelectionChanged;
        _prefix = GlobalRefs.filePaths.versionControlledAssets; 
    }

    private void OnSelectionChanged()
    {
        if (!isActive)
        {
            return;
        }
        
        Object selectedObj = Selection.activeObject;
        if (selectedObj is AssetValidationSettings)
        {
            return;
        }
        string path = AssetDatabase.GetAssetPath(selectedObj);
        if (path is null || AssetDatabase.IsValidFolder(path))
        {
            return;
        }
        
        if(path.StartsWith(_prefix))
        {
            string getVersionsCmd = _commandBuilder.GetLogOfFile(path);
            string[] versions = _terminal.ExecuteResultToStringArr(getVersionsCmd);

            _currentAssetName = selectedObj.name;
            (string metadata, bool isValid) = _validator.GetInfo(path);
            
            AssetVCSEditorWindow.ShowWindow(_currentAssetName, versions, path, UpdateVersion, SaveChanges, metadata, isValid);
        }
    }

    private void UpdateVersion(string versionCommit, string path)
    {
        string hash = versionCommit.Remove(7);

        string checkoutCmd = _commandBuilder.GetCheckout(hash, path);
        _terminal.Execute(checkoutCmd);
        AssetDatabase.Refresh();
    }

    private void SaveChanges(string versionCommit, string path)
    {
         string hash = versionCommit.Remove(7);

         string commitCmd = _commandBuilder.GetCommit(path, $"{_currentAssetName} -> v: {hash}");
         _terminal.Execute(commitCmd);       
    }
}

