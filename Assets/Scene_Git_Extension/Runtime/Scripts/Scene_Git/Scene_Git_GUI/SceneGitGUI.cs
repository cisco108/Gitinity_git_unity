using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneGitGUI : EditorWindow
{
    [MenuItem("Window/SceneGet_GUI")]
    private static void Init() => GetWindow<SceneGitGUI>(true, "SceneGet");

    public event Action OnStartSceneGet;

    private void OnGUI()
    {
        if (GUILayout.Button("Get Scene content back"))
        {
            FireStartSceneGet();
        }
    }

    private void FireStartSceneGet()
    {
        OnStartSceneGet.Invoke();
    }


    /*void OnEnable()
          => AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;

    void OnDisable()
      => AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;

    void OnAfterAssemblyReload()
    {
        if (!System.IO.File.Exists("Assets/ChatGPTCodeTemporary.cs")) return;
        EditorApplication.ExecuteMenuItem("Edit/Do Task");
        AssetDatabase.DeleteAsset("Assets/ChatGPTCodeTemporary.cs");
    }*/
}