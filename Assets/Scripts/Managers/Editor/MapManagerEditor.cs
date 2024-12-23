using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Breacher
{
    /// <summary>
    /// Custom inspector for MapManager
    /// </summary>
    [CustomEditor(typeof(MapManager))]
    public class MapManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MapManager targetedScript = (MapManager)target;
            DrawDefaultInspector();
            EditorGUILayout.LabelField("", EditorStyles.boldLabel);

            //Grid map category
            EditorGUILayout.LabelField("Grid", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("", EditorStyles.boldLabel);

            //Hide map category
            EditorGUILayout.LabelField("Hide Map", EditorStyles.boldLabel);
            /*if (GUILayout.Button("Show Hide Map"))
            {
                targetedScript.Hide.RefreshBuffer(true);
                EditorUtility.SetDirty(targetedScript);
            }

            if (GUILayout.Button("Hide Hide Map"))
            {
                targetedScript.Hide.RefreshBuffer(false);
                EditorUtility.SetDirty(targetedScript);
            }

            if (GUILayout.Button("Create Hide Map"))
            {
                targetedScript.Hide.CreateBuffer();
                EditorUtility.SetDirty(targetedScript);
            }

            if (GUILayout.Button("Dispose Hide Map"))
            {
                targetedScript.Hide.Dispose();
                EditorUtility.SetDirty(targetedScript);
            }

            if (GUILayout.Button("Recalculate eyes"))
            {
                targetedScript.Hide.RecalculateEyes();
                EditorUtility.SetDirty(targetedScript);
            }*/
            EditorGUILayout.LabelField("", EditorStyles.boldLabel);

            //Light map category
            EditorGUILayout.LabelField("Light Map", EditorStyles.boldLabel);
            if (GUILayout.Button("Create Light Map"))
            {
                EditorUtility.SetDirty(targetedScript);
            }
        }
    }
}
