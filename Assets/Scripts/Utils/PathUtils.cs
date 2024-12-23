using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Breacher.Utils
{
    public static class PathUtils
    {
        #if UNITY_EDITOR
        public static void CreateDirectoryFromAssetPath(string assetPath)
        {
            string directoryPath = Path.GetDirectoryName(assetPath);
            Directory.CreateDirectory(directoryPath);
            AssetDatabase.Refresh();
        }
        #endif
    }
}
