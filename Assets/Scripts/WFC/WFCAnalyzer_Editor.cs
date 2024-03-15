using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WFCAnalyzer))]
public class WFCAnalyzer_Editor : Editor
{
    // public override void OnInspectorGUI()
    // {
    //     base.OnInspectorGUI();
    //
    //     WFCAnalyzer generator = (WFCAnalyzer)target;
    //
    //     if (GUILayout.Button("Analyze"))
    //     {
    //         generator.Analyze();
    //     }        
    // }
}
