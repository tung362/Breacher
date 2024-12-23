using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    /// <summary>
    /// GPU representation of hide map eye vision data.
    /// </summary>
    [System.Serializable]
    public struct GPUEyeData
    {
        #region Enums
        /// <summary>
        /// Vision shape type.
        /// </summary>
        public enum ShapeType { Circle, Square }
        #endregion

        /// <summary>
        /// Compute buffer stride size.
        /// </summary>
        public const int _Stride = 20;

        /// <summary>
        /// Default value.
        /// </summary>
        public static readonly GPUEyeData _Empty = new GPUEyeData(0, Vector2Int.zero, ShapeType.Circle, 0);

        /// <summary>
        /// If currently used to simulate vision.
        /// </summary>
        [SerializeField] private int _Active;
        /// <summary>
        /// Current position in grid space.
        /// </summary>
        [SerializeField] private Vector2Int _Position;
        /// <summary>
        /// Vision shape.
        /// </summary>
        [SerializeField] private ShapeType _Shape;
        /// <summary>
        /// Vision radius.
        /// </summary>
        [SerializeField] private int _Radius;

        /*Get set*/
        /// <summary>
        /// If currently used to simulate vision.
        /// </summary>
        public int Active { get { return _Active; } set { _Active = value; } }
        /// <summary>
        /// Current position in grid space.
        /// </summary>
        public Vector2Int Position { get { return _Position; } set { _Position = value; } }
        /// <summary>
        /// Vision shape.
        /// </summary>
        public ShapeType Shape { get { return _Shape; } set { _Shape = value; } }
        /// <summary>
        /// Vision radius.
        /// </summary>
        public int Radius { get { return _Radius; } set { _Radius = value; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="active">If currently used to simulate vision.</param>
        /// <param name="position">Current position in grid space.</param>
        /// <param name="shape">Vision shape.</param>
        /// <param name="radius">Vision radius.</param>
        public GPUEyeData(int active, Vector2Int position, ShapeType shape, int radius)
        {
            _Active = active;
            _Position = position;
            _Shape = shape;
            _Radius = radius;
        }
    }
}
