#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Object), true)]
[CanEditMultipleObjects]
public class ButtonDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        /*var tempSgui = target as TempSGUI;
        if (tempSgui)
        {
            DrawButton(tempSgui);
        }*/
    }

    void DrawButton(Object o)
    {
        var methodInfos = target.GetType().GetMethods(System.Reflection.BindingFlags.Instance |
                                                      System.Reflection.BindingFlags.Public |
                                                      System.Reflection.BindingFlags.NonPublic |
                                                      System.Reflection.BindingFlags.Static);
        foreach (var methodInfo in methodInfos)
        {
            var attributes = methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true);
            if (attributes.Length > 0)
            {
                var buttonAttribute = attributes[0] as ButtonAttribute;
                string label = string.IsNullOrEmpty(buttonAttribute.Label) ?
                    methodInfo.Name : buttonAttribute.Label;

                if (GUILayout.Button(label))
                {
                    methodInfo.Invoke(target, null);
                }
            }
        } 
    }
}
#endif