using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour {

    public static NPCManager instance;

    public MovingObject[] npcs;

    private void Start()
    {
        if (instance == null)
            instance = this;

    } 
}
