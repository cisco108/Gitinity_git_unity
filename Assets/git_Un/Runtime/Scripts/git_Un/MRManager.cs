using System;
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
        
        if (CheckIfMerged(featureName))
        {
            ReactIfMerged();
        }
    }

    bool CheckIfMerged(string featureName)
    {
        string command = _commandBuilder.GetIsBranchMerged(featureName, GlobalRefs.defaultBranch, checkOnRemote: true);
        string result = _terminal.ExecuteResultToString(command);
        if (String.IsNullOrEmpty(result))
        {
            return false;
        }
        result = result.StartsWith("*") ? result.Substring(2) : result.Substring(1); // is ether '* branch' or ' branch'
        result = result.TrimEnd('\n');

        bool isMerged = featureName == result;
        GlobalRefs.isFeatureMerged = isMerged;
        return isMerged;
    }
    
    private void ReactIfMerged()
    {
        Debug.Log($"Is merged so reset.");
    }

    public void StartFeature(string featureName)
    {
        Debug.Log($"Updating GlobalRefs.currFeatureName to {featureName}");
        GlobalRefs.currFeatureName = featureName;
        
        string command = _commandBuilder.GetCreateSwitchPushBranch(featureName);
        _terminal.Execute(command);
    }
}