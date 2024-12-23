//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using Breacher.Utils;

//namespace Breacher
//{
//    /// <summary>
//    /// Represents the current scene's vision simulation data.
//    /// </summary>
//    [System.Serializable]
//    public class HideMapTest
//    {
//        /// <summary>
//        /// Current vision mapped texture (hide map) used by shaders to simulate vision.
//        /// </summary>
//        [SerializeField] private Texture2D _Map;
//        /// <summary>
//        /// Collection of all eye instances simulating vision.
//        /// </summary>
//        [SerializeField] private List<EyeData> _Eyes = new List<EyeData>();
//        /// <summary>
//        /// Pool collection of all unused _Eyes indices queued for reuse.
//        /// </summary>
//        [SerializeField] private List<int> _EyesPool = new List<int>();

//        //Get set
//        /// <summary>
//        /// Current vision mapped texture (hide map) used by shaders to simulate vision.
//        /// </summary>
//        public Texture2D Map { get { return _Map; } private set { _Map = value; } }
//        /// <summary>
//        /// Collection of all eye instances simulating vision.
//        /// </summary>
//        public IReadOnlyCollection<EyeData> Eyes { get { return _Eyes.AsReadOnly(); } }
//        /// <summary>
//        /// Pool collection of all unused _Eyes indices queued for reuse.
//        /// </summary>
//        public IReadOnlyCollection<int> EyesPool { get { return _EyesPool.AsReadOnly(); } }

//        #if UNITY_EDITOR
//        /// <summary>
//        /// Creates a new hide map texture to be used by shaders to simulate vision and save texture file to scene data folder.
//        /// </summary>
//        /// <param name="dimension">Texture resolution.</param>
//        public void CreateHideMap(Vector2Int dimension)
//        {
//            //Create Texture2D
//            Texture2D texture2D = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
//            texture2D.wrapMode = TextureWrapMode.Repeat;
//            texture2D.filterMode = FilterMode.Point;

//            for (int x = 0; x < texture2D.width; x++)
//            {
//                for (int y = 0; y < texture2D.height; y++) texture2D.SetPixel(x, y, Color.black);
//            }
//            texture2D.Apply();

//            //Save to png file
//            Scene scene = SceneManager.GetActiveScene();
//            string dataPath = Path.GetDirectoryName(Application.dataPath).Replace('\\', '/');
//            string sceneDirectoryPath = Path.GetDirectoryName(scene.path).Replace('\\', '/');
//            string filePath = $"{dataPath}/{sceneDirectoryPath}/{scene.name}/HideMap.png";
//            byte[] pngData = texture2D.EncodeToPNG();
//            PathUtils.CreateDirectoryFromAssetPath(filePath);
//            File.WriteAllBytes(filePath, pngData);
//            AssetDatabase.Refresh();
//            Object.DestroyImmediate(texture2D);

//            //Scene scene = SceneManager.GetActiveScene();
//            //string sceneDirectoryPath = Path.GetDirectoryName(scene.path).Replace('\\', '/');
//            //string filePath = $"{sceneDirectoryPath}/{scene.name}/HideMap.png";
//            //byte[] pngData = texture2D.EncodeToPNG();
//            //PathUtils.CreateDirectoryFromAssetPath(filePath);
//            //AssetDatabase.CreateAsset(texture2D, filePath);

//            //Apply TextureImporter settings for new png file
//            string fileRelativePath = $"{sceneDirectoryPath}/{scene.name}/HideMap.png";
//            TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(fileRelativePath);
//            textureImporter.isReadable = true;
//            textureImporter.wrapMode = TextureWrapMode.Repeat;
//            textureImporter.filterMode = FilterMode.Point;
//            textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
//            textureImporter.SaveAndReimport();

//            //Load new png file
//            Texture2D map = AssetDatabase.LoadAssetAtPath<Texture2D>(fileRelativePath);
//            _Map = map;
//            RefreshHideMap(_Map);
//        }
//        #endif

//        /// <summary>
//        /// Refresh and show or hide the hide map texture used by shaders.
//        /// </summary>
//        /// <param name="show">Show or hide the vision mapped texture.</param>
//        public void RefreshHideMap(bool show)
//        {
//            Shader.SetGlobalTexture("_HideMap", show ? Map : null);

//            #if UNITY_EDITOR
//            if (!Application.isPlaying) UnityEditor.SceneView.RepaintAll();
//            #endif
//        }

//        /// <summary>
//        /// Get eye instance by index.
//        /// </summary>
//        /// <param name="index">Eye instance index.</param>
//        /// <returns>Eye instance by index.</returns>
//        public EyeData GetEye(int index)
//        {
//            if (index < 0 || index >= _Eyes.Count) return null;

//            return _Eyes[index];
//        }

//        /// <summary>
//        /// Add eye instance to be used in simulating vision.
//        /// </summary>
//        /// <param name="eye">Eye instance.</param>
//        /// <returns>Added eye instance index.</returns>
//        public int AddEye(EyeData eye)
//        {
//            if (_EyesPool.Count > 0)
//            {
//                int index = _EyesPool[0];
//                _Eyes[index] = eye;
//                _EyesPool.Remove(0);
//                return index;
//            }

//            _Eyes.Add(eye);
//            return _Eyes.Count - 1;
//        }

//        /// <summary>
//        /// Remove eye instance by index.
//        /// </summary>
//        /// <param name="index">Eye instance index.</param>
//        public void RemoveEye(int index)
//        {
//            if (index < 0 || index >= _Eyes.Count) return;

//            _Eyes[index] = null;
//            _EyesPool.Add(index);
//        }

//        /// <summary>
//        /// Resimulate all eye instances.
//        /// </summary>
//        /// <param name="gridMap">Grid map for obstruction checking.</param>
//        public void RecalculateEyes(GridMap gridMap = null)
//        {
//            for (int i = 0; i < _Eyes.Count; i++) RecalculateEye(i, false, gridMap);
//            _Map.Apply();
//            RefreshHideMap(true);
//        }

//        /// <summary>
//        /// Resimulate eye instance by index.
//        /// </summary>
//        /// <param name="index">Eye instance index.</param>
//        /// <param name="applyChanges">Apply hide map texture changes and refresh texture on all shaders.</param>
//        /// <param name="gridMap">Grid map for obstruction checking.</param>
//        public void RecalculateEye(int index, bool applyChanges, GridMap gridMap = null)
//        {
//            if (index < 0 || index >= _Eyes.Count || _Eyes[index] == null) return;

//            //Draw sight according to shape
//            EyeData eye = _Eyes[index];
//            HashSet<Vector2Int> edits = new HashSet<Vector2Int>();
//            switch (eye.Shape)
//            {
//                case EyeData.ShapeType.Circle:
//                    DrawSightSquare(eye.Position.x, eye.Position.y, eye.Radius, eye.Radius, ref edits, gridMap, eye);
//                    break;
//                case EyeData.ShapeType.Square:
//                    DrawSightSquare(eye.Position.x, eye.Position.y, eye.Radius, -1, ref edits, gridMap, eye);
//                    break;
//                default:
//                    DrawSightSquare(eye.Position.x, eye.Position.y, eye.Radius, eye.Radius, ref edits, gridMap, eye);
//                    break;
//            }

//            //Mask previous edits with current edits
//            foreach (Vector2Int edit in edits) eye.PreviousEdits.Remove(edit);

//            //Revert back to hidden if no other eyes currently have line of sight
//            HashSet<Vector2Int> previousEdits = eye.PreviousEdits;
//            eye.PreviousEdits = edits;
//            foreach (Vector2Int previousEdit in previousEdits)
//            {
//                bool revert = true;
//                foreach (EyeData eyeData in _Eyes)
//                {
//                    if (eyeData.PreviousEdits.Contains(previousEdit))
//                    {
//                        revert = false;
//                        break;
//                    }
//                }

//                if (!revert) continue;

//                _Map.SetPixel(previousEdit.x, previousEdit.y, Color.black);
//            }

//            if (applyChanges)
//            {
//                _Map.Apply();
//                RefreshHideMap(true);
//            }
//        }

//        /// <summary>
//        /// Draws vision lines from the midpoint to the square edges along a radius.
//        /// </summary>
//        /// <param name="midpointX">Midpoint x coordinate.</param>
//        /// <param name="midpointY">Midpoint y coordinate.</param>
//        /// <param name="halfSquareLength">Half of square's length.</param>
//        /// <param name="radius">Vision lines radius.</param>
//        /// <param name="edits">Collection of all grid space changes.</param>
//        /// <param name="gridMap">Grid map for obstruction checking.</param>
//        /// <param name="eye">Eye instance for duplicate set pixels checking.</param>
//        public void DrawSightSquare(int midpointX, int midpointY, int halfSquareLength, int radius, ref HashSet<Vector2Int> edits, GridMap gridMap = null, EyeData eye = null)
//        {
//            for (int y = midpointY - halfSquareLength; y <= midpointY + halfSquareLength; y++)
//            {
//                DrawSightLine(midpointX, midpointY, midpointX - halfSquareLength, y, radius, ref edits, gridMap, eye); //Left
//                DrawSightLine(midpointX, midpointY, midpointX + halfSquareLength, y, radius, ref edits, gridMap, eye); //Right
//            }

//            for (int x = midpointX - halfSquareLength; x <= midpointX + halfSquareLength; x++)
//            {
//                DrawSightLine(midpointX, midpointY, x, midpointY - halfSquareLength, radius, ref edits, gridMap, eye); //Top
//                DrawSightLine(midpointX, midpointY, x, midpointY + halfSquareLength, radius, ref edits, gridMap, eye); //Bottom
//            }
//        }

//        /// <summary>
//        /// Draws vision line from starting point to ending point.
//        /// </summary>
//        /// <param name="startX">Starting x coordinate.</param>
//        /// <param name="startY">Starting y coordinate.</param>
//        /// <param name="endX">Ending x coordinate.</param>
//        /// <param name="endY">Ending y coordinate.</param>
//        /// <param name="radius">Vision line radius.</param>
//        /// <param name="edits">Collection of all grid space changes.</param>
//        /// <param name="gridMap">Grid map for obstruction checking.</param>
//        /// <param name="eye">Eye instance for duplicate set pixels checking.</param>
//        public void DrawSightLine(int startX, int startY, int endX, int endY, int radius, ref HashSet<Vector2Int> edits, GridMap gridMap = null, EyeData eye = null)
//        {
//            int dx = Mathf.Abs(endX - startX);
//            int dy = Mathf.Abs(endY - startY);
//            int sx = startX < endX ? 1 : -1;
//            int sy = startY < endY ? 1 : -1;
//            int err = dx - dy;
//            int radiusSquared = radius * radius;
//            int x = startX;
//            int y = startY;
//            while (true)
//            {
//                if (radius > 0)
//                {
//                    //float distance = Mathf.Sqrt(Mathf.Pow(x - startX, 2) + Mathf.Pow(y - startY, 2));
//                    //if (distance > radius) break;

//                    int dxCurrent = x - startX;
//                    int dyCurrent = y - startY;
//                    if (dxCurrent * dxCurrent + dyCurrent * dyCurrent > radiusSquared) break;
//                }

//                if (eye != null)
//                {
//                    if (!eye.PreviousEdits.Contains(new Vector2Int(x, y))) _Map.SetPixel(x, y, Color.white);
//                }
//                else _Map.SetPixel(x, y, Color.white);

//                edits.Add(new Vector2Int(x, y));

//                if (gridMap != null)
//                {
//                    GridData gridData = gridMap.GetGrid(new Vector2Int(x, y));
//                    if (gridData == null || gridData.ObstructSight) break;
//                }

//                if (x == endX && y == endY) break;

//                int e2 = 2 * err;
//                if (e2 > -dy)
//                {
//                    err -= dy;
//                    x += sx;
//                }

//                if (e2 < dx)
//                {
//                    err += dx;
//                    y += sy;
//                }
//            }
//        }
//    }
//}
