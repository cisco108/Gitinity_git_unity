using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Scene Git", menuName = "SceneGitMain", order = 0)]
public class SceneGitMain : ScriptableObject
{
    public SceneGitUI sceneGitUI;
    private void OnValidate()
    {
        Debug.Log($"OnValidate {this}");
    }
}