using System;
using System.IO;
public static class LogSystem
{
    public static void WriteLog(string[] logMessages)
    {
        // Write the timestamp as the first line
        File.AppendAllLines(GlobalRefs.filePaths.logsFile, new[] {DateTime.Now.ToString() });

        // Write each log message on a new line
        foreach (string logMessage in logMessages)
        {
            File.AppendAllLines(GlobalRefs.filePaths.logsFile, new[] { logMessage });
        }
    }
 
}