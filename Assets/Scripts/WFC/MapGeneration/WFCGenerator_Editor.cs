using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WFCGenerator))]
public class WFCGenerator_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WFCGenerator generator = (WFCGenerator)target;

        if (GUILayout.Button("Clear"))
        {
            generator.Initiate();
        }        
        
        if (GUILayout.Button("Step"))
        {
            generator.Step(new List<WFCSlot>());
        }
    }
}
