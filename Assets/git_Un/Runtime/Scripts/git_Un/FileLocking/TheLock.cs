using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class TheLock
{
    private JsonSerializer serializer = new JsonSerializer();
    private string path = GlobalRefs.filePaths.lockedProtocolFile;

    public void WriteLocking(string fileToLock)
    {
        LockInfo lockInfo;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            lockInfo = JsonConvert.DeserializeObject<LockInfo>(json);
        }
        else
        {
            lockInfo = new LockInfo();
        }

        int hash = fileToLock.GetHashCode();
        try
        {
            lockInfo.lockedFiles.Add(hash, (fileToLock, GlobalRefs.filePaths.userEmail));
        }
        catch
        {
            Debug.LogWarning("This file is already locked!");
            return;
        }

        using StreamWriter sw = new StreamWriter(path);
        using (JsonWriter writer = new JsonTextWriter(sw))
        {
            writer.Formatting = Formatting.Indented;
            serializer.Serialize(writer, lockInfo);
        }
        Debug.Log($"{fileToLock} was just locked by {GlobalRefs.filePaths.userEmail}");
    }

    /// This does not check if it is the same user, who tries to unlock it,
    /// as the one who locked it. It is assumed, that this is not needed
    /// due to responsible behaviour of group members XD. 
    public void WriteUnlocking(string fileToUnlock)
    {
        if (!File.Exists(path))
        {
            Debug.Log("No file has been locked, so unlock not possible.");
            return;
        }

        string json = File.ReadAllText(path);
        LockInfo lockInfo = JsonConvert.DeserializeObject<LockInfo>(json);
        int hash = fileToUnlock.GetHashCode();
        if (lockInfo.lockedFiles.ContainsKey(hash))
        {
            lockInfo.lockedFiles.Remove(hash);

            using StreamWriter sw = new StreamWriter(path);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, lockInfo);
            }
            Debug.Log($"The file {fileToUnlock} has been unlocked (locally), will be pushed from some where else.");
            return;
        }
        Debug.Log($"{fileToUnlock} was not locked.");
    }


    /// <summary>
    /// Converts the json to a LockInfo object and returns
    /// the lockedFile property.
    /// </summary>
    /// <returns>lockedFile</returns>
    public bool IsFileLocked(string json, string fileToCheck, out string lockerEmail)
    {
        lockerEmail = null;
        var lockInfo = JsonConvert.DeserializeObject<LockInfo>(json);
        if (lockInfo == null)
        {
            Debug.LogError("Deserialization failed.");
            return false;
        }

        int hash = fileToCheck.GetHashCode();
        if (lockInfo.lockedFiles.ContainsKey(hash))
        {
            var obj = lockInfo.lockedFiles[hash];
            lockerEmail = obj.Item2;
            return true;
        }

        return false;
    }
}

public class LockInfo
{
    public Dictionary<int, (string, string)> lockedFiles = new();
}