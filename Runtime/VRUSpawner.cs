using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRUnited
{

    public abstract class VRUSpawner : MonoBehaviour
    {

        #region PUBLIC ATTRIBUTES

        public enum InitialPose
        {
            Standing,
            Sitting,
        }
        public InitialPose _initialPose = InitialPose.Standing;

        public RuntimeAnimatorController _initialAnimatorController = null;

        #endregion

        #region GET AND SET

        public abstract bool GetSpotAt(int i, out Vector3 position, out Quaternion rotation);
        
        #endregion

        #region DEBUG

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            foreach (Transform t in transform)
            {
                Gizmos.DrawSphere(t.position, 0.25f);
            }
        }

        #endregion

    }

}


