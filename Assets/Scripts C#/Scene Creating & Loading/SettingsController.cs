using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingsController : MonoBehaviour {


    [SerializeField]bool autoConfigure = false;
    [SerializeField]GameObject player;
    [SerializeField]GameObject playerVR;
    [SerializeField]Transform spawnPos;

    public bool enableVR;

    // Sound
    public float volume;

    void Start()
    {
        if(autoConfigure)
        {
            if (SteamVR.active)
                Instantiate(playerVR, spawnPos.position, Quaternion.identity);
            else Instantiate(player, spawnPos.position, Quaternion.identity);
        }
    }
}
