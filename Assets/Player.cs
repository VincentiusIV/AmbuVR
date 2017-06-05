using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AmbuVR
{
    public class Player : MonoBehaviour
    {
        public static Player instance;

        public TeleportVive teleporter;

        private void Start()
        {
            if (instance == null)
                instance = this;  
        }

        public void SetCanTeleport(bool value)
        {
            teleporter.CanTeleport = value;
        }
    }
}

