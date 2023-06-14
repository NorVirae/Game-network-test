using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    [CreateAssetMenu(fileName = "Profile", menuName = "Network/User Profile")]
    public class UserProfileCatch : ScriptableObject
    {
        public string userId;
    }

}

