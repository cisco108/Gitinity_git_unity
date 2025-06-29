public static class GitinityUtils
{
    public static bool IsExecutingFromPackageCache()
    {
        var codeBase = System.Reflection.Assembly.GetExecutingAssembly().Location;
        return codeBase.Replace('\\', '/').Contains("/Library/PackageCache/");
    }
 
}