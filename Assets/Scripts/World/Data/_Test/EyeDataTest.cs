//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Breacher
//{
//    /// <summary>
//    /// Represents eye vision data for the hide map.
//    /// </summary>
//    [System.Serializable]
//    public class EyeDataTest
//    {
//        #region Enums
//        /// <summary>
//        /// Vision shape type.
//        /// </summary>
//        public enum ShapeType { Circle, Square }
//        #endregion

//        /// <summary>
//        /// Current position where the eye is located in grid space.
//        /// </summary>
//        [SerializeField] private Vector2Int _Position;
//        /// <summary>
//        /// Vision shape type.
//        /// </summary>
//        [SerializeField] private ShapeType _Shape;
//        /// <summary>
//        /// Vision radius.
//        /// </summary>
//        [SerializeField] private int _Radius;

//        //Cache
//        /// <summary>
//        /// Collection of grid space coordinates where the eye has vision over.
//        /// </summary>
//        [SerializeField] private HashSet<Vector2Int> _PreviousEdits = new HashSet<Vector2Int>();

//        /*Get set*/
//        /// <summary>
//        /// Current position where the eye is located in grid space.
//        /// </summary>
//        public Vector2Int Position { get { return _Position; } set { _Position = value; } }
//        /// <summary>
//        /// Vision shape type.
//        /// </summary>
//        public ShapeType Shape { get { return _Shape; } set { _Shape = value; } }
//        /// <summary>
//        /// Vision radius.
//        /// </summary>
//        public int Radius { get { return _Radius; } set { _Radius = value; } }
//        /// <summary>
//        /// Collection of grid space coordinates where the eye has vision over.
//        /// </summary>
//        public HashSet<Vector2Int> PreviousEdits { get { return _PreviousEdits; } set { _PreviousEdits = value; } }

//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="position">Current position where the eye is located in grid space.</param>
//        /// <param name="shape">Vision shape type.</param>
//        /// <param name="radius">Vision radius.</param>
//        public EyeDataTest(Vector2Int position, ShapeType shape, int radius)
//        {
//            _Position = position;
//            _Shape = shape;
//            _Radius = radius;
//        }
//    }
//}
