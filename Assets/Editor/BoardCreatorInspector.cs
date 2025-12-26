using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MapCreate))]
public class BoardCreatorInspector : Editor
{
    public MapCreate current
    {
        get
        {
            return (MapCreate)target;
        }
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Clear"))
            current.Clear();
        if (GUILayout.Button("Grow"))
            current.Grow();
        if (GUILayout.Button("Shrink"))
            current.Shrink();
        if (GUILayout.Button("Grow Area"))
            current.GrowArea();
        if (GUILayout.Button("Shrink Area"))
            current.ShrinkArea();
        if (GUILayout.Button("Save"))
            current.Save();
        if (GUILayout.Button("Load"))
            current.Load();
        if (GUILayout.Button("AddUnit"))
            current.AddUnit();
        if (GUILayout.Button("RemoveUnit"))
            current.RemoveUnit();
        if (GUILayout.Button("GenerateMaxSizeMap"))
            current.GenerateMap();  
        if (GUI.changed)
            current.UpdateMarker();
    }
}
