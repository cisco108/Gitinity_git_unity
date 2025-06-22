using UnityEngine;

public class MRManager
{
    private ITerminalInterface _terminal;
    private ICommandBuilder _commandBuilder;
    public MRManager(ITerminalInterface terminal, ICommandBuilder commandBuilder)
    {
        _terminal = terminal;
        _commandBuilder = commandBuilder;
    }

    public void GetFeatureInfo()
    {
        string featureName = GlobalRefs.currFeatureName;
        
        Debug.Log($"GetFeatureInfo() {this}");
        string command = _commandBuilder.GetIsBranchMerged(featureName, GlobalRefs.defaultBranch, checkOnRemote: true);
        string result = _terminal.ExecuteResultToString(command);
        result = result.StartsWith("* ") ? result.Substring(1) : result;
        result = result.TrimEnd('\n');

        GlobalRefs.isFeatureMerged = featureName == result;
    }

    public void StartFeature(string featureName)
    {
        Debug.Log($"Updating GlobalRefs.currFeatureName to {featureName}");
        GlobalRefs.currFeatureName = featureName;
        
        string command = _commandBuilder.GetCreateSwitchPushBranch(featureName);
        _terminal.Execute(command);
    }
}