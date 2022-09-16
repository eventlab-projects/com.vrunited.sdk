using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRUnited
{

    public class VRUPunObservablePlayer : VRUPunObservableBase
    {

        //public delegate void VRUPunObservablePlayerAction(VRUPunObservablePlayer player);
        //public static event VRUPunObservablePlayerAction OnVRUPlayerEnabled;
        //public static event VRUPunObservablePlayerAction OnVRUPlayerDisabled;

        public static HashSet<VRUPunObservablePlayer> _instances = new HashSet<VRUPunObservablePlayer>();

        public override void OnEnable()
        {
            base.OnEnable();

            //if (OnVRUPlayerEnabled != null)
            //{
            //    OnVRUPlayerEnabled(this);
            //}

            _instances.Add(this);
        }

        protected override void Start()
        {
            base.Start();

            //if (OnVRUPlayerEnabled != null)
            //{
            //    OnVRUPlayerEnabled(this);
            //}
        }

        public override void OnDisable()
        {
            base.OnDisable();

            _instances.Remove(this);

            //if (OnVRUPlayerDisabled != null)
            //{
            //    OnVRUPlayerDisabled(this);
            //}
        }

    }

}


