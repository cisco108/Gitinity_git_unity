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
    private const string NamePrefix = "+  m_Name:";
    private const string GameObjectPrefix = "+--- !u!1 ";

    private string _pathToDiff = "saved_diff.txt";


    private const int LengthOfFileID = 9;

    private void Start()
    {
        var diffObjs = GetDiffGameObjectsInScene(GetDiffObjIDs(_pathToDiff));
        foreach (var go in diffObjs)
        {
            go.SetActive(false);
        }
    }

    private List<GameObject> GetDiffGameObjectsInScene(List<long> fileIDs)
    {
        List<GameObject> diffGameObjects = new();
        foreach (var fileID in fileIDs)
        {
            diffGameObjects.Add(GetGameObjectFromFileID(fileID));
        }

        return diffGameObjects;
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

        Debug.LogError($"{fileID}: corresponding object in scene found");
        return null;
    }


    private List<long> GetDiffObjIDs(string path)
    {
        if (!File.Exists(path))
            Debug.LogError($"Cant find file {path}");

        string[] linesWhereDiffStarts = File.ReadLines(path).Where(line => line.StartsWith(GameObjectPrefix)).ToArray();
        List<long> diffObjIDs = new();

        foreach (var line in linesWhereDiffStarts)
        {
            int index = line.IndexOf('&');
            if (index != -1)
            {
                string afterAmpersand = line.Substring(index + 1);
                diffObjIDs.Add(long.Parse(afterAmpersand));
                /*Debug.Log($"Original line: {line}");
                Debug.Log($"After '&': {afterAmpersand}");*/
            }
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
            if (line.StartsWith(NamePrefix))
                filteredNames.Add(line.Remove(0, NamePrefix.Length));
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