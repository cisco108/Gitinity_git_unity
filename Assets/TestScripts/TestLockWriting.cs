using UnityEngine;

public class TestLockWriting : MonoBehaviour
{
    private TheLock _theLock;

    [Button("Test lock write and read")]
    public void Test()
    {
        _theLock = new TheLock();
        _theLock.WriteLocking();

        string foo = _theLock.ReadLockInfo();
        Debug.Log($"locked file is:\n {foo}");
    }
    [Button("Test lock read")]
    public void TestRead()
    {
        _theLock = new TheLock();

        string foo = _theLock.ReadLockInfo();
        Debug.Log($"locked file is:\n {foo}");
    }
}