using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    /// <summary>
    /// Hide map eye vision data.
    /// </summary>
    [System.Serializable]
    public class EyeData
    {
        /// <summary>
        /// GPU eye vision data.
        /// </summary>
        public GPUEyeData _Data = GPUEyeData._Empty;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data">GPU eye vision data.</param>
        public EyeData(GPUEyeData data)
        {
            _Data = data;
        }
    }
}
