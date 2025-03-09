public interface ITerminalInterface
{
    /// <summary>
    /// File location specified in implementation. 
    /// Same location also accessed when reading diff from IGitDiffReader.
    /// This function will likely be replaces by just using >> directly in bash.
    /// </summary>
    /// <param name="command"></param>
    public void ExecuteResultToTxt(string command, string outputPath = default);

    /// <summary>
    /// Writes it to txt first, then extracts string.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public string ExecuteResultToString(string command);
    public string[] ExecuteResultToStringArr(string command);
    /// <summary>
    /// Use it when you just want to do a command where you not want to get anything back.
    /// </summary>
    /// <param name="command"></param>
    public void Execute(string command);
    
}