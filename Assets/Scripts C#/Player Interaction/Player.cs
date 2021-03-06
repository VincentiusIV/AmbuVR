﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AmbuVR
{
    public class Player : MonoBehaviour
    {
        public static Player instance;

        public TeleportVive teleporter;
        public Transform hmd;
        public Transform leftController;
        public Transform rightController;
        public Transform feet;

        private void Awake()
        {
            // Singleton
            if (instance == null)
                instance = this;
            else Destroy(gameObject);
        }
        
        // Forces teleporter if it can teleport or not      
        public void SetCanTeleport(bool value)
        {
            teleporter.CanTeleport = value;
        }
    }
}

