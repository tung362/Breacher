using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

namespace Breacher
{
    [System.Serializable]
    public class GridMap
    {
        #region Format
        [System.Serializable]
        public class GridDictionary : SerializableDictionaryBase<Vector2Int, GridData> { }
        #endregion

        [SerializeField] private GridDictionary _Map = new GridDictionary(); //TODO: list of tiles
        public Texture2D Test;

        //Get set
        public IReadOnlyDictionary<Vector2Int, GridData> Map { get { return new ReadOnlyDictionary<Vector2Int, GridData>(_Map); } }

        /// <summary>
        /// Initializer.
        /// </summary>
        public void Init()
        {
            Shader.SetGlobalTexture("_SightObstructionMap", Test);
        }

        /// <summary>
        /// Gets the current ideal grid dimension should resizing be needed.
        /// </summary>
        /// <returns>Current ideal grid dimension.</returns>
        public int GetIdealGridDimension()
        {
            byte exponent = 1;
            short powerOf2 = 2;
            foreach (Vector2Int key in _Map.Keys)
            {
                while (key.x >= powerOf2) powerOf2 = (short)Mathf.Pow(2, ++exponent);
                while (key.y >= powerOf2) powerOf2 = (short)Mathf.Pow(2, ++exponent);
            }
            return powerOf2;
        }

        //TODO: grid data should not be manually modified, instead the tiles should be what determines the obstruction flags
        public GridData GetGrid(Vector2Int coord)
        {
            if (!Map.ContainsKey(coord)) return null;

            return Map[coord];
        }
    }
}
