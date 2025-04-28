using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonGenerator), true)]
public class RandomGeneratorEditor : Editor
{
    DungeonGenerator generator;

    private void OnEnable()
    {
        generator = (DungeonGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate"))
        {
            generator.GenerateDungeons();
        }
    }
}
