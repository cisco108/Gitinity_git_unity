using System.IO;
using UnityEngine;

public class TestLockWriting : MonoBehaviour
{
    private TheLock _theLock;
    private FileLocking _fileLocking;

    [Button("Test Locking")]
    public void Test()
    {
        _theLock = new TheLock();
        _theLock.WriteLocking("SERS");

        // string foo = _theLock.ReadLockInfo();
        // Debug.Log($"locked file is:\n {foo}");
    }

    [Button("Test Unlocking")]
    public void TestUnlock()
    {
        
        _theLock = new TheLock();
        _theLock.WriteUnlocking("SERS");
    }
    
    [Button("Test IsFileLocked")]
    public void TestRead()
    {
        string fileToCheck = "SERS";
        _theLock = new TheLock();
        string json = File.ReadAllText(GlobalRefs.filePaths.lockedProtocolFile);
        bool isLocked =_theLock.IsFileLocked(json, fileToCheck, out var email);
        if (isLocked)
        {
            Debug.Log($"locked file is:\n {fileToCheck}, was locked by {email}");
            return;
        }
        Debug.Log("File is not locked.");
    }

    [Button("Event")]
    public void TestEvent()
    {
        _fileLocking = new FileLocking();
        _fileLocking.TestEvent();
    }
}