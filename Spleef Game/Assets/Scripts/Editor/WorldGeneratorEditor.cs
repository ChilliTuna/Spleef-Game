using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.UIElements.UxmlAttributeDescription;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10f);
        if(GUILayout.Button(new GUIContent("Generate Static Platform", "Use below button to manually generate a static platform with the above details." +
            "\nStatic platforms can have tiles deleted or be deleted themselves as required, unlike dynamic platforms.")))
        {
            WorldGenerator worldGenerator = (WorldGenerator)target;
            worldGenerator.GenerateStaticPlatform();
        }
        GUILayout.Space(10f);
        EditorGUILayout.HelpBox("DO NOT delete tiles for dynamic platforms, or delete dynamic platforms themselves. " +
            "\nInstead, use the \"Remove Dynamic Platform\" button to delete platform, or the \"Staticize Dynamic Platform\" button to make platform modifiable.", MessageType.Warning);
        if(GUILayout.Button(new GUIContent("Generate Dynamic Platform", "Use below button to manually generate a dynamic platform with the above details. There can only be one dynamic platform at any point in time.")))
        {
            WorldGenerator worldGenerator = (WorldGenerator)target;
            worldGenerator.Initialise();
        }
        if(GUILayout.Button(new GUIContent("Remove Dynamic Platform", "Use below button to remove the dynamic platform.")))
        {
            WorldGenerator worldGenerator = (WorldGenerator)target;
            worldGenerator.DeletePlatform();
        }
        if (GUILayout.Button(new GUIContent("Staticize Dynamic Platform", "Use below button to make dynamic platform static. " +
            "\nIt will no longer be referenced by the script and a new dynamic platform can be generated. " +
            "\nThis is useful if you want to create a static platform from an existing dynamic platform." +
            "\nThis new platform will function as if you had just used the top \"Generate Platform\" button.")))
        {
            WorldGenerator worldGenerator = (WorldGenerator)target;
            worldGenerator.StaticizePlatform();
        }

    }
}
