public class GitDataObject
{
    private string[] _branchNames;
    public string[] BranchNames => _branchNames;

    public GitDataObject(string[] branchNames)
    {
        _branchNames = branchNames;
    }
}
