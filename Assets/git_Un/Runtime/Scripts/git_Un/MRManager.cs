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

        GlobalRefs.isFeatureMerged = CheckIfMerged(featureName);
    }

    bool CheckIfMerged(string featureName)
    {
        string command = _commandBuilder.GetIsBranchMerged(featureName, GlobalRefs.defaultBranch, checkOnRemote: true);
        string result = _terminal.ExecuteResultToString(command);
        return !string.IsNullOrEmpty(result);


        // All not needed, because when the string contains something,
        // the featureName was in the list of merged branches and got grep.
        /*
        if (String.IsNullOrEmpty(result))
        {
            return false;
        }
        result = result.StartsWith("*") ? result.Substring(2) : result.Substring(1); // is ether '* branch' or ' branch'
        result = result.TrimEnd('\n');

        return featureName == result;*/
    }

    public void StartFeature(string featureName)
    {
        Debug.Log($"Updating GlobalRefs.currFeatureName to {featureName}");
        GlobalRefs.currFeatureName = featureName;

        string pullDefaultBranchCmd = _commandBuilder.GetSwitchAndPull(GlobalRefs.defaultBranch); 
        string createSwitchPushCmd = _commandBuilder.GetCreateSwitchPushBranch(featureName);

        string startFeatureCmd = pullDefaultBranchCmd + " && " + createSwitchPushCmd;
        
        _terminal.Execute(startFeatureCmd);
    }
}