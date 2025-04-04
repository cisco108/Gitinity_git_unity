using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class TheLock
{
    private JsonSerializer serializer = new JsonSerializer();
    private string path = GlobalRefs.filePaths.lockedProtocolFile;

    public void WriteLocking()
    {
        var lockInfo = new LockInfo(
            GlobalRefs.filePaths.fileToLockName,
            GlobalRefs.filePaths.userEmail);

        using StreamWriter sw = new StreamWriter(path);
        using JsonWriter writer = new JsonTextWriter(sw);
        serializer.Serialize(writer, lockInfo);
    }

    public string ReadLockInfo() // Not really used, just kept it for now.
    {
        string json = File.ReadAllText(path);
        var r =DeserializeFileLockInfo(json);
        return $"{r.lockedFile} {r.personWhoLocked}";
    }

    /// <summary>
    /// Converts the json to a LockInfo object and returns
    /// the lockedFile property.
    /// </summary>
    /// <returns>lockedFile</returns>
    public (string lockedFile, string personWhoLocked) DeserializeFileLockInfo(string json)
    {
        var lockInfo = JsonConvert.DeserializeObject<LockInfo>(json);
        if (lockInfo == null)
        {
            Debug.LogError("Deserialization failed.");
            return (null, null);
        }
        return (lockInfo.lockedFile, lockInfo.lockerEmail);
    }
}

public class LockInfo
{
    public LockInfo(string fileToLock, string lockerMail)
    {
        lockedFile = fileToLock;
        lockerEmail = lockerMail;
    }

    public string lockedFile;
    public string lockerEmail;
}