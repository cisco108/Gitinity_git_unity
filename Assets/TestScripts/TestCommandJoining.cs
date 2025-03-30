using System.IO;
using UnityEngine;

public class TestCommandJoining : MonoBehaviour
{
    private static ITerminalInterface _terminal;
    private static ICommandBuilder _commandBuilder;

    [Button("Execute multiple")]
    public void JoinAndExecute()
    {
        _terminal = new GitBashInterface();
        _commandBuilder = new GitBashCommandBuilder();
        string cmd = File.ReadAllText("Assets/TestScripts/multiple.sh");
        Debug.Log(cmd);

        _terminal.Execute(cmd);

    }
}