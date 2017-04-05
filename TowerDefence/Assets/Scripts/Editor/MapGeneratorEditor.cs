using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGenerator mapGen = (MapGenerator)target;

        if(GUILayout.Button("Generate Map"))
        {
            mapGen.InitData();
        }

        if (GUILayout.Button("Load Texture Maps"))
        {
            mapGen.LoadAllTextures();
        }

        if (GUILayout.Button("Save To Texture"))
        {
            mapGen.SaveToTexture(mapGen.mapData, mapGen.savedPNGName + ".png", mapGen.GridSizeX, mapGen.GridSizeY);
        }
    }
}
