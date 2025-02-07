using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DiffParser : MonoBehaviour
{
    public GameObject gameObject;
    private string _namePrefix = "+  m_Name:";
    private string _pathToDiff = "saved_diff.txt";


    private int _lengthOfFileID = 9;

    private void Start()
    {
        // Debug.Log(ExtractDiffObjs(_pathToDiff));
        gameObject = GetGameObjectFromFileID(198659496);
        Debug.Log(gameObject.name);
        gameObject.SetActive(false);
    }

    private static GameObject GetGameObjectFromFileID(long fileID) // also called local identifier
    {
        GameObject resultGo = null;
        var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        // Test every gameobjects
        foreach (var go in gameObjects)
        {
#if UNITY_EDITOR
            PropertyInfo inspectorModeInfo =
                typeof(UnityEditor.SerializedObject).GetProperty("inspectorMode",
                    BindingFlags.NonPublic | BindingFlags.Instance);
            UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(go);
            inspectorModeInfo.SetValue(serializedObject, UnityEditor.InspectorMode.Debug, null);
            UnityEditor.SerializedProperty localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile");
#endif
            if (localIdProp.longValue == fileID)
            {
                resultGo = go;
                return resultGo;
            }
        }

        Debug.LogError($"Not found");
        return null;
    }
    
   
    private List<string> ExtractDiffObjIDs(string path)
    {
        if (!File.Exists(path))
            Debug.LogError($"Cant find file {path}");
        
        string[] linesWhereDiffStarts = File.ReadLines(path).Where(line => line.StartsWith("+---")).ToArray();

        List<string> diffObjIDs = new();

        foreach (var line in linesWhereDiffStarts)
        {
           diffObjIDs.Add(line.Remove(0, line.Length - _lengthOfFileID)); 
        }

        return diffObjIDs;
    }
    private string[] ExtractDiffObjsNames(string path)
    {
        if (!File.Exists(path))
            Debug.LogError($"Cant find file {path}");
        string[] linesWhereDiffStarts = File.ReadLines(path).Where(line => line.StartsWith("+---")).ToArray();

        string[] gameObjectNamesInDiff = File.ReadLines(path).Where(line => line.StartsWith("+  m_Name:")).ToArray();

        foreach (var line in gameObjectNamesInDiff)
        {
            print(line);
        }

        return gameObjectNamesInDiff;
    }

    private GameObject[] GetDiffGameObjectsByName(string[] linesWhereDiffStarts)
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