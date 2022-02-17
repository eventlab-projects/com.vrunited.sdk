using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRUnited
{

    public class QuickNetSpawnerPoints : VRUSpawner
    {

        #region PUBLIC ATTRIBUTES

        public List<Transform> _spawnPoints = new List<Transform>();

        #endregion

        #region GET AND SET

        public override bool GetSpotAt(int i, out Vector3 position, out Quaternion rotation)
        {
            position = transform.position;
            rotation = transform.rotation;

            bool isFreeSpot = i < _spawnPoints.Count;

            if (isFreeSpot)
            {
                Debug.Log("freePosition = " + i);
                //The position is currently free. 
                Transform t = _spawnPoints[i];
                position = t.position;
                rotation = t.rotation;
            }

            return isFreeSpot;
        }

        #endregion

        #region DEBUG

        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            foreach (Transform t in _spawnPoints)
            {
                Gizmos.DrawSphere(t.position, 0.25f);
            }
        }

        #endregion

    }

}


