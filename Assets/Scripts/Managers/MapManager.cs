using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Breacher.Utils;

namespace Breacher
{
    /// <summary>
    /// Mapper for current scene's grid, vision, and lighting simulations.
    /// </summary>
    public class MapManager : Singleton<MapManager>
    {
        [SerializeField] private GridMap _GridMap = new GridMap();
        [SerializeField] private LightMap _LightMap = new LightMap();
        [SerializeField] private HideMap _HideMap = new HideMap();

        //Get set
        public GridMap Grid { get { return _GridMap; } private set { _GridMap = value; } }
        public LightMap Light { get { return _LightMap; } private set { _LightMap = value; } }
        public HideMap Hide { get { return _HideMap; } private set { _HideMap = value; } }

        protected override void OnStart()
        {
            /*for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    _GridMap.Test(new Vector2Int(x, y), new GridData(false, false, false));
                }
            }
            _GridMap.Test(new Vector2Int(10, 10), new GridData(true, false, false));
            _GridMap.Test(new Vector2Int(6, 7), new GridData(true, false, false));
            _GridMap.Test(new Vector2Int(8, 7), new GridData(true, false, false));
            _GridMap.Test(new Vector2Int(9, 7), new GridData(true, false, false));*/

            _GridMap.Init();
            _LightMap.Init();
            _HideMap.Init();
            _HideMap.AddEye(new EyeData(new GPUEyeData(1, new Vector2Int(12, 12), GPUEyeData.ShapeType.Circle, 4)));
            _HideMap.AddEye(new EyeData(new GPUEyeData(1, new Vector2Int(8, 8), GPUEyeData.ShapeType.Circle, 6)));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                int index = _HideMap.AddEye(new EyeData(new GPUEyeData(1, new Vector2Int(12, 12), GPUEyeData.ShapeType.Circle, 4)));
                _HideMap.RecalculateEye(index, true);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                EyeData eye1 = _HideMap.GetEye(0);
                EyeData eye2 = _HideMap.GetEye(1);
                eye1._Data.Position = new Vector2Int(eye1._Data.Position.x + 1, eye1._Data.Position.y);
                eye2._Data.Position = new Vector2Int(eye2._Data.Position.x - 1, eye2._Data.Position.y);
                _HideMap.RecalculateEye(0, true);
                _HideMap.RecalculateEye(1, true);
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                EyeData eye1 = _HideMap.GetEye(0);
                EyeData eye2 = _HideMap.GetEye(1);
                eye1._Data.Position = new Vector2Int(eye1._Data.Position.x - 1, eye1._Data.Position.y);
                eye2._Data.Position = new Vector2Int(eye2._Data.Position.x + 1, eye2._Data.Position.y);
                _HideMap.RecalculateEye(0, true);
                _HideMap.RecalculateEye(1, true);
            }
        }

        protected override void OnDestroyed()
        {
            _LightMap.Dispose();
            _HideMap.Dispose();
        }

        #region Domain GC
        /// <summary>
        /// Clears data that presist after runtime ends.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void DomainClear()
        {
            DomainReset();
        }
        #endregion
    }
}
