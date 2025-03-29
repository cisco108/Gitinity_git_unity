using UnityEngine;

public class TestLockWriting : MonoBehaviour
{
    private TheLock _theLock = new TheLock();

    void Start()
    {
        _theLock.WriteLocking();
    }
}