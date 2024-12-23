using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Breacher
{
    #if UNITY_EDITOR
    /// <summary>
    /// Creates 2DTextureArray using a list of texture2D and export to a file
    /// </summary>
    public class Texture2DArrayExporter : EditorWindow
    {
        /*Settings*/
        public bool _CreateMipMaps = true;
        public List<Texture2D> _Textures = new List<Texture2D>();

        /*Cache*/
        private SerializedObject _Target;
        private SerializedProperty _CreateMipMapsProperty;
        private SerializedProperty _TexturesProperty;

        /// <summary>
        /// Show window in editor
        /// </summary>
        [MenuItem("Breacher Tools/Texture2DArrayExporter")]
        public static void ShowWindow()
        {
            //Display window
            EditorWindow.GetWindow(typeof(Texture2DArrayExporter));
        }

        /// <summary>
        /// Initialize window
        /// </summary>
        void CreateGUI()
        {
            //Bind window
            _Target = new SerializedObject(this);

            //Get window properties
            _CreateMipMapsProperty = _Target.FindProperty("CreateMipMaps");
            _TexturesProperty = _Target.FindProperty("Textures");
        }

        /// <summary>
        /// Window update
        /// </summary>
        void OnGUI()
        {
            EditorGUILayout.PropertyField(_CreateMipMapsProperty, true);
            EditorGUILayout.PropertyField(_TexturesProperty, true);
            _Target.ApplyModifiedProperties();

            if (_Textures.Count == 0 || _Textures[0] == null) return;

            //Export Texture2DArray to file
            if (GUILayout.Button("Export"))
            {
                //Check for invalid path
                //string fullPath = StandaloneFileBrowser.SaveFilePanel("Export Texture2DArray", "", "", "asset").Replace('\\', '/');
                string fullPath = EditorUtility.SaveFilePanel("Export Texture2DArray", "", "", ".asset");
                if (!fullPath.StartsWith(Application.dataPath))
                {
                    Debug.LogError($"Error: Could not create 2DTextureArray from path: {fullPath}, must be relative path to {Application.dataPath}. @Texture2DArrayExporter");
                    return;
                }

                //Create Texture2DArray
                Texture2DArray texture2DArray = new Texture2DArray(_Textures[0].width, _Textures[0].height, _Textures.Count, _Textures[0].format, _CreateMipMaps);
                texture2DArray.wrapMode = _Textures[0].wrapMode;
                texture2DArray.filterMode = _Textures[0].filterMode;
                for (int i = 0; i < _Textures.Count; i++)
                {
                    if (_Textures[i] == null) continue;
                    Graphics.CopyTexture(_Textures[i], 0, texture2DArray, i);
                }
                texture2DArray.Apply();

                //Export to file
                Uri fullPathUri = new Uri(fullPath);
                Uri basePathUri = new Uri(Application.dataPath);
                Uri relativePathUri = basePathUri.MakeRelativeUri(fullPathUri);
                string relativePath = relativePathUri.ToString();
                AssetDatabase.CreateAsset(texture2DArray, relativePath);
            }
        }
    }
    #endif
}
