using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonGenerator), true)]
public class RandomGeneratorEditor : Editor
{
    DungeonGenerator generator;

    private void Awake()
    {
        generator = (DungeonGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate"))
        {
            generator.GenerateDungeons();
        }
    }
}
