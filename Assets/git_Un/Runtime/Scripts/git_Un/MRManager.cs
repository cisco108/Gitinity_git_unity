using UnityEngine;

public class MRManager
{
    private ITerminalInterface _terminal;
    private ICommandBuilder _commandBuilder;
    public MRManager(ITerminalInterface terminal, ICommandBuilder commandBuilder)
    {
        
    }

    public void GetFeatureInfo()
    {
        Debug.Log($"GetFeatureInfo() {this}");
    }
}