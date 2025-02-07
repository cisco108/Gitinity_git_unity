using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DiffParser : MonoBehaviour
{
    private string _namePrefix = "+  m_Name:";
    private string _pathToDiff = "saved_diff.txt";

    private void Start()
    {
        Debug.Log(ExtractDiffObjs(_pathToDiff));
        var go = GetDiffGameObjects(ExtractDiffObjs(_pathToDiff));
    }

    private string[] ExtractDiffObjs(string path)
    {
        if (!File.Exists(path))
            Debug.LogError($"Cant find file {path}");
        string[] linesWhereDiffStarts = File.ReadLines(path).Where(line => line.StartsWith("+---")).ToArray();

        string[] gameObjectNamesInDiff = File.ReadLines(path).Where(line => line.StartsWith("+  m_Name:")).ToArray();

        foreach (var line in gameObjectNamesInDiff)
        {
            print(line);
        }

        /*
        foreach (var line in linesWhereDiffStarts)
        {
            print(line);
        }
        */

        return gameObjectNamesInDiff;
    }

    private GameObject[] GetDiffGameObjects(string[] linesWhereDiffStarts)
    {
        List<string> filteredNames = new();
        foreach (var line in linesWhereDiffStarts)
        {
            if (line.StartsWith(_namePrefix))
                filteredNames.Add(line.Remove(0, _namePrefix.Length));
            else
                Debug.LogWarning($"no name prefix in {line}");
        }

        foreach (var name in filteredNames)
        {
            Debug.Log($"filtered name: {name}");
        }

        return null;
    }
}