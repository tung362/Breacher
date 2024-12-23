using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Breacher.Utils;

namespace Breacher
{
    /// <summary>
    /// Handles and manages current scene's vision simulation.
    /// </summary>
    [System.Serializable]
    public class HideMap : IDisposable
    {
        /// <summary>
        /// Collection of all eye instances simulating vision.
        /// </summary>
        [SerializeField] private List<EyeData> _Eyes = new List<EyeData>();
        /// <summary>
        /// Collection pool of all unused eye indices queued for reuse.
        /// </summary>
        [SerializeField] private List<int> _EyesPool = new List<int>();

        /*Cache*/
        /// <summary>
        /// GPU buffer used by shaders for vision simulation.
        /// </summary>
        [SerializeField] private ComputeBuffer _EyesBuffer;
        /// <summary>
        /// Has the hide map been initialized?
        /// </summary>
        [SerializeField] private bool _Ready;

        /*Get set*/
        /// <summary>
        /// Collection of all eye instances simulating vision.
        /// </summary>
        public IReadOnlyCollection<EyeData> Eyes { get { return _Eyes.AsReadOnly(); } }
        /// <summary>
        /// Collection pool of all unused _Eyes indices queued for reuse.
        /// </summary>
        public IReadOnlyCollection<int> EyesPool { get { return _EyesPool.AsReadOnly(); } }

        /// <summary>
        /// Initializer.
        /// </summary>
        public void Init()
        {
            _Ready = true;
            CreateBuffer();
        }

        /// <summary>
        /// Creates and applies a new gpu buffer to be used by shaders.
        /// </summary>
        public void CreateBuffer()
        {
            if (!_Ready) return;

            int bufferCount = GetIdealBufferCount();
            if (_EyesBuffer != null && _EyesBuffer.IsValid()) Dispose();

            _EyesBuffer = new ComputeBuffer(bufferCount, GPUEyeData._Stride);
            RecalculateEyes();
        }

        /// <summary>
        /// Globally reapplies and updates gpu buffer used by shaders.
        /// </summary>
        /// <param name="show">Show or hide the vision simulation.</param>
        public void RefreshBuffer(bool show)
        {
            if (!_Ready) return;

            Shader.SetGlobalBuffer("_Eyes", _EyesBuffer);
            Shader.SetGlobalInt("_EyeCount", show ? _Eyes.Count : -1);

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
            if (_EyesBuffer == null || !_EyesBuffer.IsValid()) return 1;

            if (_Eyes.Count <= _EyesBuffer.count) return _EyesBuffer.count;

            int bufferCount = _EyesBuffer.count;
            while (_Eyes.Count > bufferCount) bufferCount *= 2;

            return bufferCount;
        }

        /// <summary>
        /// Get eye instance by index.
        /// </summary>
        /// <param name="index">Eye instance index.</param>
        /// <returns>Eye instance by index.</returns>
        public EyeData GetEye(int index)
        {
            if (index < 0 || index >= _Eyes.Count) return null;

            return _Eyes[index];
        }

        /// <summary>
        /// Add eye instance to be used in vision simulation.
        /// </summary>
        /// <param name="eye">Eye instance.</param>
        /// <returns>Added eye instance index.</returns>
        public int AddEye(EyeData eye)
        {
            int index;
            if (_EyesPool.Count > 0)
            {
                index = _EyesPool[0];
                _Eyes[index] = eye;
                _EyesPool.RemoveAt(0);
                return index;
            }

            _Eyes.Add(eye);
            index = _Eyes.Count - 1;

            //Check if buffer needs to be resized to accommodate eyes count.
            if (GetIdealBufferCount() != _EyesBuffer.count) CreateBuffer();
            else RecalculateEye(index, true);

            return index;
        }

        /// <summary>
        /// Remove eye instance by index.
        /// </summary>
        /// <param name="index">Eye instance index.</param>
        public void RemoveEye(int index)
        {
            if (index < 0 || index >= _Eyes.Count) return;

            _Eyes[index] = null;
            _EyesPool.Add(index);
            RecalculateEye(index, true);
        }

        /// <summary>
        /// Reapplies and updates all gpu buffer eye data instances.
        /// </summary>
        public void RecalculateEyes()
        {
            if (!_Ready) return;

            for (int i = 0; i < _Eyes.Count; i++) RecalculateEye(i, false);
            RefreshBuffer(true);

            /*#if UNITY_EDITOR
            if (!Application.isPlaying) UnityEditor.SceneView.RepaintAll();
            #endif*/
        }

        /// <summary>
        /// Reapplies and updates gpu buffer eye data instance by index.
        /// </summary>
        /// <param name="index">Eye instance index.</param>
        /// <param name="applyChanges">Should the gpu buffer be updated?</param>
        public void RecalculateEye(int index, bool applyChanges)
        {
            if (!_Ready || index < 0 || index >= _Eyes.Count) return;

            GPUEyeData[] eyes = { _Eyes[index] == null ? GPUEyeData._Empty : _Eyes[index]._Data };
            _EyesBuffer.SetData(eyes, 0, index, eyes.Length);

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
            if (_EyesBuffer != null)
            {
                _EyesBuffer.Dispose();
                RefreshBuffer(false);
            }
        }
    }
}
