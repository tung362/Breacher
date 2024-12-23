using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    /// <summary>
    /// Light map lighting data.
    /// </summary>
    [System.Serializable]
    public class LightData
    {
        /// <summary>
        /// GPU lighting data.
        /// </summary>
        public GPULightData _Data = GPULightData._Empty;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data">GPU lighting data.</param>
        public LightData(GPULightData data)
        {
            _Data = data;
        }
    }
}