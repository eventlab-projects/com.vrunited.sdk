using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRUnited
{

    public class VRUPunObservablePlayer : VRUPunObservableBase
    {

        public static HashSet<VRUPunObservablePlayer> _instances = new HashSet<VRUPunObservablePlayer>();

        public override void OnEnable()
        {
            base.OnEnable();

            _instances.Add(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();

            _instances.Remove(this);
        }

    }

}


