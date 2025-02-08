using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : PropertyAttribute
{
    public string Label { get; private set; }

    public ButtonAttribute(string label)
    {
        Label = label;
    }
}