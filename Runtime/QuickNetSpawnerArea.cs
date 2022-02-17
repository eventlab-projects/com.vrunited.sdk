using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickVR
{

    public class QuickNetSpawnerArea : QuickNetSpawner
    {

        #region PUBLIC ATTRIBUTES

        public float _areaRadius = 0.0f;
        public float _playerRadius = 0.5f;
        public float _playerHeight = 2f;

        #endregion

        #region GET AND SET

        public override bool GetSpotAt(int id, out Vector3 position, out Quaternion rotation)
        {
            const int maxNumTrials = 100;
            bool safeSpot = false;
            float yOffset = 0.01f;
            Vector3 p1 = Vector3.zero;

            //Check if there is a safe spot in this area
            for (int i = 0; !safeSpot && i < maxNumTrials; i++)
            {
                float x = Random.Range(-_areaRadius, _areaRadius);
                float z = Random.Range(-_areaRadius, _areaRadius);
                p1 = transform.position + transform.right * x + transform.forward * z + Vector3.up * (yOffset + _playerRadius);
                float h = Mathf.Max(2 * _playerRadius, _playerHeight);
                Vector3 p2 = p1 + Vector3.up * (h - 2 * _playerRadius);

                safeSpot = !Physics.CheckCapsule(p1, p2, _playerRadius);
            }

            if (safeSpot)
            {
                position = p1 + Vector3.down * (_playerRadius + yOffset);
                Debug.Log("SafeSpot = " + position.ToString("f3"));
            }
            else
            {
                position = transform.position;
            }

            Vector3 forward = (transform.position - position).normalized;
            if (forward == Vector3.zero)
            {
                forward = transform.forward;
            }

            rotation = Quaternion.FromToRotation(transform.forward, forward);

            return true;
        }

        #endregion

    }

}


