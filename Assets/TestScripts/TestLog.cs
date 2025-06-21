using UnityEngine;

public class TestLog : MonoBehaviour
{
    private bool b = true;

    private ITerminalInterface _terminal;

    void Start()
    {
        _terminal = new GitBashInterface();
        
        string bS = b.ToString().ToLower();
        _terminal.Execute($"echo '{bS}' > {GlobalRefs.filePaths.allowCommitFile}");
        
        Debug.Log(bS);
    }
    
}