using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    [System.Serializable]
    public class GridData
    {
        [SerializeField] private bool _ObstructSight;
        [SerializeField] private bool _ObstructMovement;

        //Get set
        public bool ObstructSight { get { return _ObstructSight; } set { _ObstructSight = value; } }
        public bool ObstructMovement { get { return _ObstructMovement; } set { _ObstructMovement = value; } }

        public GridData(bool obstructSight, bool obstructMovement)
        {
            _ObstructSight = obstructSight;
            _ObstructMovement = obstructMovement;
        }
    }
}
