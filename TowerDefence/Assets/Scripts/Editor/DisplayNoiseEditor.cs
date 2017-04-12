using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DisplayNoise))]
public class DisplayNoiseEditor: Editor
{

    public override void OnInspectorGUI()
    {
        DisplayNoise NoiseDisplay = (DisplayNoise)target;

        if(DrawDefaultInspector())
        {
            NoiseDisplay.GenerateMap();
        }

        if(GUILayout.Button("Generate"))
        {
            NoiseDisplay.GenerateMap();
        }
    }
}
