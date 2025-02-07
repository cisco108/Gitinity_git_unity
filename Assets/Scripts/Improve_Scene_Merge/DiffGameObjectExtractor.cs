using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DiffGameObjectExtractor : MonoBehaviour
{
    public GameObject gameObject;
    private const string NamePrefix = "+  m_Name:";
    private const string GameObjectPrefix = "+--- !u!1 ";
    private string _pathToDiff = "saved_diff.txt";


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

    //https://www.reddit.com/r/unity/comments/q2jw1u/how_to_find_an_object_in_a_scene_using_its/
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

}