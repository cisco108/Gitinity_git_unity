using System;

public static class GitCommands
{
    public static string logOneLine = "git log --oneline";
    public static string echoHello = "echo hello";

    public static string CommandBuilde(string command, string commitHash)
    {
        return String.Join(command," ", commitHash);
    }
}