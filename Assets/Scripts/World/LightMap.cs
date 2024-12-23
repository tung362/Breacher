using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    /// <summary>
    /// Handles and manages current scene's lighting simulation.
    /// </summary>
    [System.Serializable]
    public class LightMap
    {
        /// <summary>
        /// Scene ambient light color.
        /// </summary>
        [SerializeField] private Color _AmbientColor = Color.white;
        /// <summary>
        /// Collection of all light instances simulating lighting.
        /// </summary>
        [SerializeField] private List<LightData> _Lights = new List<LightData>();
        /// <summary>
        /// Collection pool of all unused light indices queued for reuse.
        /// </summary>
        [SerializeField] private List<int> _LightsPool = new List<int>();

        /*Cache*/
        /// <summary>
        /// GPU buffer used by shaders for light simulation.
        /// </summary>
        [SerializeField] private ComputeBuffer _LightsBuffer;
        /// <summary>
        /// Has the light map been initialized?
        /// </summary>
        [SerializeField] private bool _Ready;

        /*Get set*/
        /// <summary>
        /// Scene ambient light color.
        /// </summary>
        public Color AmbientColor { get { return _AmbientColor; } set { SetAmbientColor(value); } }
        /// <summary>
        /// Collection of all light instances simulating lighting.
        /// </summary>
        public IReadOnlyCollection<LightData> Lights { get { return _Lights.AsReadOnly(); } }
        /// <summary>
        /// Collection pool of all unused light indices queued for reuse.
        /// </summary>
        public IReadOnlyCollection<int> LightsPool { get { return _LightsPool.AsReadOnly(); } }

        /// <summary>
        /// Initializer.
        /// </summary>
        public void Init()
        {
            _Ready = true;
            CreateBuffer();
            RefreshAmbientColor(true);
        }

        /// <summary>
        /// Creates and applies a new gpu buffer to be used by shaders.
        /// </summary>
        public void CreateBuffer()
        {
            if (!_Ready) return;

            int bufferCount = GetIdealBufferCount();
            if (_LightsBuffer != null && _LightsBuffer.IsValid()) Dispose();

            _LightsBuffer = new ComputeBuffer(bufferCount, GPULightData._Stride);
            RecalculateLights();
        }

        /// <summary>
        /// Globally reapplies and updates ambient color used by shaders.
        /// </summary>
        /// <param name="show">Show or hide the ambient color.</param>
        public void RefreshAmbientColor(bool show)
        {
            if (!_Ready) return;

            Shader.SetGlobalColor("_AmbientColor", show ? _AmbientColor : Color.white);

            /*#if UNITY_EDITOR
            if (!Application.isPlaying) UnityEditor.SceneView.RepaintAll();
            #endif*/
        }

        /// <summary>
        /// Globally reapplies and updates gpu buffer used by shaders.
        /// </summary>
        /// <param name="show">Show or hide the light simulation.</param>
        public void RefreshBuffer(bool show)
        {
            if (!_Ready) return;

            Shader.SetGlobalBuffer("_Lights", _LightsBuffer);
            Shader.SetGlobalInt("_LightCount", show ? _Lights.Count : -1);

            /*#if UNITY_EDITOR
            if (!Application.isPlaying) UnityEditor.SceneView.RepaintAll();
            #endif*/
        }

        /// <summary>
        /// Gets the current ideal buffer count should resizing be needed.
        /// </summary>
        /// <returns>Current ideal buffer count.</returns>
        public int GetIdealBufferCount()
        {
            if (_LightsBuffer == null || !_LightsBuffer.IsValid()) return 1;

            if (_Lights.Count <= _LightsBuffer.count) return _LightsBuffer.count;

            int bufferCount = _LightsBuffer.count;
            while (_Lights.Count > bufferCount) bufferCount *= 2;

            return bufferCount;
        }

        /// <summary>
        /// Set ambient color.
        /// </summary>
        /// <param name="ambientColor">Ambient color.</param>
        public void SetAmbientColor(Color ambientColor)
        {
            _AmbientColor = ambientColor;
            RefreshAmbientColor(true);
        }

        /// <summary>
        /// Get light instance by index.
        /// </summary>
        /// <param name="index">Light instance index.</param>
        /// <returns>Light instance by index.</returns>
        public LightData GetLight(int index)
        {
            if (index < 0 || index >= _Lights.Count) return null;

            return _Lights[index];
        }

        /// <summary>
        /// Add light instance to be used in light simulation.
        /// </summary>
        /// <param name="light">Light instance.</param>
        /// <returns>Added light instance index.</returns>
        public int AddLight(LightData light)
        {
            int index;
            if (_LightsPool.Count > 0)
            {
                index = _LightsPool[0];
                _Lights[index] = light;
                _LightsPool.RemoveAt(0);
                return index;
            }

            _Lights.Add(light);
            index = _Lights.Count - 1;

            //Check if buffer needs to be resized to accommodate lights count.
            if (GetIdealBufferCount() != _LightsBuffer.count) CreateBuffer();
            else RecalculateLight(index, true);

            return index;
        }

        /// <summary>
        /// Remove light instance by index.
        /// </summary>
        /// <param name="index">Light instance index.</param>
        public void RemoveLight(int index)
        {
            if (index < 0 || index >= _Lights.Count) return;

            _Lights[index] = null;
            _LightsPool.Add(index);
            RecalculateLight(index, true);
        }

        /// <summary>
        /// Reapplies and updates all gpu buffer light data instances.
        /// </summary>
        public void RecalculateLights()
        {
            if (!_Ready) return;

            for (int i = 0; i < _Lights.Count; i++) RecalculateLight(i, false);
            RefreshBuffer(true);

            /*#if UNITY_EDITOR
            if (!Application.isPlaying) UnityEditor.SceneView.RepaintAll();
            #endif*/
        }

        /// <summary>
        /// Reapplies and updates gpu buffer light data instance by index.
        /// </summary>
        /// <param name="index">Light instance index.</param>
        /// <param name="applyChanges">Should the gpu buffer be updated?</param>
        public void RecalculateLight(int index, bool applyChanges)
        {
            if (!_Ready || index < 0 || index >= _Lights.Count) return;

            GPULightData[] lights = { _Lights[index] == null ? GPULightData._Empty : _Lights[index]._Data };
            _LightsBuffer.SetData(lights, 0, index, lights.Length);

            if (applyChanges) RefreshBuffer(true);

            /*#if UNITY_EDITOR
            if (!Application.isPlaying) UnityEditor.SceneView.RepaintAll();
            #endif*/
        }

        /// <summary>
        /// Dispose all persistent data, call during destruction to prevent memory leaks.
        /// </summary>
        public void Dispose()
        {
            RefreshAmbientColor(false);
            if (_LightsBuffer != null)
            {
                _LightsBuffer.Dispose();
                RefreshBuffer(false);
            }
        }
    }
}
