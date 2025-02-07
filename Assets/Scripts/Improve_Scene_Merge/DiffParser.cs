using System;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DiffParser : MonoBehaviour
{
    private string _pathToDiff = "saved_diff.txt";

    private void OnValidate()
    {
        Debug.Log(ExtractDiffObjs());
    }

    private string[] ExtractDiffObjs()
    {
        if(!File.Exists(_pathToDiff))
            Debug.LogError($"Cant find file {_pathToDiff}");
        string[] linesWhereDiffStarts = File.ReadLines(_pathToDiff).
            Where(line => line.StartsWith("+---")).ToArray();

        string[] gameObjectNamesInDiff =  File.ReadLines(_pathToDiff).
            Where(line => line.StartsWith("+  m_Name:")).ToArray();
        
        foreach (var line in gameObjectNamesInDiff)
        {
            print(line);
        }
        foreach (var line in linesWhereDiffStarts)
        {
            print(line);
        }

        return linesWhereDiffStarts;
    }

    private GameObject[] GetDiffGameObjects(string[] linesWhereDiffStarts)
    {
        foreach (var line in linesWhereDiffStarts)
        {
           // GameObject go = GameObject.Find() 
        }

        return null;
    }
}
