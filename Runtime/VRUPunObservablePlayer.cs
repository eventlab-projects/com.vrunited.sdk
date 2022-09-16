using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRUnited
{

    public class VRUPunObservablePlayer : VRUPunObservableBase
    {

        public delegate void VRUPunObservablePlayerAction(VRUPunObservablePlayer player);
        public static event VRUPunObservablePlayerAction OnVRUPlayerEnabled;
        public static event VRUPunObservablePlayerAction OnVRUPlayerDisabled;

        public override void OnEnable()
        {
            base.OnEnable();

            if (OnVRUPlayerEnabled != null)
            {
                OnVRUPlayerEnabled(this);
            }
        }

        protected override void Start()
        {
            base.Start();

            if (OnVRUPlayerEnabled != null)
            {
                OnVRUPlayerEnabled(this);
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();

            if (OnVRUPlayerDisabled != null)
            {
                OnVRUPlayerDisabled(this);
            }
        }

    }

}


