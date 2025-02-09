using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class HierarchyColor
{
    static HierarchyColor()
    {
        // EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj != null && obj.GetComponent<Collider>())
        {
            // Change background color
            EditorGUI.DrawRect(selectionRect, Color.grey);

            // Change text color
            var style = new GUIStyle();
            style.normal.textColor = Color.red;
            EditorGUI.LabelField(selectionRect, obj.name, style);
        }
    }
}
