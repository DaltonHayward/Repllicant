using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGenBounds : Editor
{
    private void OnSceneGUI()
    {
        LevelGenerator prefab = target as LevelGenerator;

        if (prefab == null)
            return;

        // Draw the dimensions using Handles
        DrawDimensions(prefab);
    }

    private void DrawDimensions(LevelGenerator prefab)
    {
        // Get the dimensions of your prefab
        int width = prefab.GetComponent<LevelGenerator>().width;
        int height = prefab.GetComponent<LevelGenerator>().height;

        // Get position
        Vector3 generatorPosition = prefab.transform.position;

        // Calculate the corner position
        Vector3 cornerPosition = generatorPosition - new Vector3(-width * 0.5f, 0, -height * 0.5f);

        // Convert dimensions
        Vector3 size = new Vector3(width, 0, height);

        // Draw the dimensions using Handles
        Handles.color = Color.green;
        Handles.DrawWireCube(cornerPosition, size);
    }

    // whenever the inspector is updated, update gui
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Repaint();
    }
}
