using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class TheLock
{
    private JsonSerializer serializer = new JsonSerializer();
    private string path = GlobalRefs.filePaths.lockedProtocolFile; 
    public void WriteLocking()
    {
        var lockInfo = new LockInfo("this file man");
        // serializer.NullValueHandling = NullValueHandling.Ignore;

        using StreamWriter sw = new StreamWriter(path);
        using JsonWriter writer = new JsonTextWriter(sw);
        serializer.Serialize(writer, lockInfo);
    }

    public string ReadLockInfo()
    {
        string json = File.ReadAllText(path);
        var lockInfo = JsonConvert.DeserializeObject<LockInfo>(json);
        if (lockInfo == null)
        {
           Debug.LogError("Deserialization failed.");
           return null;
        }
        
        return lockInfo.lockedFile;
    }
}

public class LockInfo
{
    public LockInfo(string fileToLock)
    {
        lockedFile = fileToLock;
    }
    public string lockedFile;
}