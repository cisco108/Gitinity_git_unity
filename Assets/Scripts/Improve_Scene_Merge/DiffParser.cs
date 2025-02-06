using System;
using System.IO;
using UnityEngine;

public class DiffParser : MonoBehaviour
{
    private string _pathToDiff = "saved_diff.txt";

    private void Start()
    {
        Debug.Log(ExtractDiffObjs());
    }

    private string ExtractDiffObjs()
    {
        if(!File.Exists(_pathToDiff))
            Debug.LogError($"Cant find file {_pathToDiff}");

        
        return File.ReadAllText(_pathToDiff);
    }
}
