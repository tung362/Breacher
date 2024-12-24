using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    /// <summary>
    /// GPU representation of light map lighting data.
    /// </summary>
    [System.Serializable]
    public struct GPULightData
    {
        #region Enums
        /// <summary>
        /// Lighting shape type.
        /// </summary>
        public enum ShapeType { Circle, Square }
        #endregion

        /// <summary>
        /// Compute buffer stride size.
        /// </summary>
        public const int _Stride = 40;

        /// <summary>
        /// Default value.
        /// </summary>
        public static readonly GPULightData _Empty = new GPULightData(0, Vector2Int.zero, ShapeType.Circle, 0, Color.clear, 0);

        /// <summary>
        /// If currently used to simulate lighting.
        /// </summary>
        [SerializeField] private int _Active;
        /// <summary>
        /// Current position in grid space.
        /// </summary>
        [SerializeField] private Vector2Int _Position;
        /// <summary>
        /// Lighting shape.
        /// </summary>
        [SerializeField] private ShapeType _Shape;
        /// <summary>
        /// Lighting radius.
        /// </summary>
        [SerializeField] private int _Radius;
        /// <summary>
        /// Lighting color.
        /// </summary>
        [SerializeField] private Color _GradientColor;
        /// <summary>
        /// Lighting intensity.
        /// </summary>
        [SerializeField] private float _Intensity;

        /*Get set*/
        /// <summary>
        /// If currently used to simulate lighting.
        /// </summary>
        public int Active { get { return _Active; } set { _Active = value; } }
        /// <summary>
        /// Current position in grid space.
        /// </summary>
        public Vector2Int Position { get { return _Position; } set { _Position = value; } }
        /// <summary>
        /// Lighting shape.
        /// </summary>
        public ShapeType Shape { get { return _Shape; } set { _Shape = value; } }
        /// <summary>
        /// Lighting radius.
        /// </summary>
        public int Radius { get { return _Radius; } set { _Radius = value; } }
        /// <summary>
        /// Lighting color.
        /// </summary>
        public Color GradientColor { get { return _GradientColor; } set { _GradientColor = value; } }
        /// <summary>
        /// Lighting intensity.
        /// </summary>
        public float Intensity { get { return _Intensity; } set { _Intensity = value; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="active">If currently used to simulate lighting.</param>
        /// <param name="position">Current position in grid space.</param>
        /// <param name="shape">Lighting shape.</param>
        /// <param name="radius">Lighting radius.</param>
        /// <param name="gradientColor">Lighting color.</param>
        /// <param name="intensity">Lighting intensity.</param>
        public GPULightData(int active, Vector2Int position, ShapeType shape, int radius, Color gradientColor, float intensity)
        {
            _Active = active;
            _Position = position;
            _Shape = shape;
            _Radius = radius;
            _GradientColor = gradientColor;
            _Intensity = intensity;
        }
    }
}
