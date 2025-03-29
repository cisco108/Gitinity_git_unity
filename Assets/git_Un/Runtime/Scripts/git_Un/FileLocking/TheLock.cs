using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class TheLock
{
    public void WriteLocking()
    {
        var lockInfo = new LockInfo("this file man");
        JsonSerializer serializer = new JsonSerializer();
        serializer.NullValueHandling = NullValueHandling.Ignore;

        string path = Directory.GetCurrentDirectory();
        using StreamWriter sw = new StreamWriter( path + @"\json.json");
        using JsonWriter writer = new JsonTextWriter(sw);
        serializer.Serialize(writer, lockInfo);
    }
}

public class LockInfo
{
    public LockInfo(string fileToLock)
    {
        lockedFile = fileToLock;
    }

    public string lockedFile;
    public int someInt = 23;
    public int[] arr = new[] { 2, 34, 53, 55, 32 };
}