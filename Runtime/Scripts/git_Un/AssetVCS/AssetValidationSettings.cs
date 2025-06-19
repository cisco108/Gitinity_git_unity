using UnityEngine;

using UnityEngine;

[CreateAssetMenu(fileName = "AssetValidationSettings", menuName = "Asset VCS/Validation Settings", order = 0)]
public class AssetValidationSettings : ScriptableObject
{
    public int maxVertexCount = 100000;
    public float expectedScale = 0.0001f;
    public int expectedImgWidth = 1024;
    public int expectedImgHeight = 1024;
}
