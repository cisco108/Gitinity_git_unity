using UnityEngine;

using UnityEngine;

[CreateAssetMenu(fileName = "AssetValidationSettings", menuName = "Asset VCS/Validation Settings", order = 0)]
public class AssetValidationSettings : ScriptableObject
{
    public int maxVertexCount = 10000;
    public float expectedScale = 1f;
    public int expectedImgWidth = 1024;
    public int expectedImgHeight = 1024;
}
