using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class HierarchyIcon
{
    static HierarchyIcon()
    {
        // EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj != null && obj.GetComponent<Collider>())
        {
            Rect buttonRect = new Rect(selectionRect.x + selectionRect.width - 50, selectionRect.y, 50, 16);

            IconAttribute icon = new IconAttribute("Assets/TutorialInfo/Icons/URP.png");
            if (GUI.Button(buttonRect, "Btn"))
            {
                Debug.Log($"Button clicked on {obj.name}");
            }
        }
    }
}