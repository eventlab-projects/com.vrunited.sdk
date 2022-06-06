using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRUnited
{

    public class VRUAvatarDescriptor : MonoBehaviour
    {

        #region PUBLIC ATTRIBUTES

        public enum UserType
        {
            Human,
            Guest,
        }

        public UserType _userType = UserType.Human;

        #endregion

    }

}


