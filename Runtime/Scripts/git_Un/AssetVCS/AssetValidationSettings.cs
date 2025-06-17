using UnityEngine;

using UnityEngine;

[CreateAssetMenu(fileName = "AssetValidationSettings", menuName = "Asset VCS/Validation Settings", order = 0)]
public class AssetValidationSettings : ScriptableObject
{
    public int maxVertexCount = 100_000;
    public float minScaleFactor = 0.0001f;
    public int minImageWidth = 2;
    public int minImageHeight = 2;
    public int minAudioSampleRate = 1;
}
