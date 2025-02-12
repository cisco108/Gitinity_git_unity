using System;
using System.IO;
public class LogSystem
{
    public void WriteLog(string[] logMessages)
    {
        // Write the timestamp as the first line
        File.AppendAllLines(FilePaths.logsFile, new[] {DateTime.Now.ToString() });

        // Write each log message on a new line
        foreach (string logMessage in logMessages)
        {
            File.AppendAllLines(FilePaths.logsFile, new[] { logMessage });
        }
    }
 
}