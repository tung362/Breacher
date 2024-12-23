using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    /// <summary>
    /// Enforces a class having only a single instance in the scene.
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : class
    {
        /// <summary>
        /// Easy reference accessor in the instance of a singleton class.
        /// </summary>
        public static T _Instance { get; private set; }

        /// <summary>
        /// Checks for multiple instances of a class and enforces single instance.
        /// Do not use. Use OnStart() instead.
        /// </summary>
        void Start()
        {
            if (_Instance != null)
            {
                if (_Instance != this as T)
                {
                    Debug.LogError($"Multiple instance of \"{this.GetType()}\", destroying extra instance.");
                    DestroyImmediate(this);
                    return;
                }
            }
            else _Instance = this as T;
            OnStart();
        }

        /// <summary>
        /// Reset upon being destroyed.
        /// Do not use. Use OnDestroyed() instead.
        /// </summary>
        void OnDestroy()
        {
            OnDestroyed();
            if (_Instance == this as T) DomainReset();
        }

        /// <summary>
        /// Redirect for Awake().
        /// </summary>
        protected virtual void OnStart() { }

        /// <summary>
        /// Redirect for OnDestroy().
        /// </summary>
        protected virtual void OnDestroyed() { }

        #region Domain GC
        /// <summary>
        /// Clears data that presist after runtime ends.
        /// </summary>
        protected static void DomainReset()
        {
            _Instance = null;
        }
        #endregion
    }
}
