using UnityEngine;
using System.Collections;
using Valve.VR;

public class SettingsController : MonoBehaviour {

    [SerializeField]GameObject player;
    [SerializeField]GameObject playerVR;
    [SerializeField]Transform spawnPos;

    public bool enableVR;

    // Sound
    public float volume;

    void Start()
    {
        if (SteamVR.active)
            Instantiate(playerVR, spawnPos.position, Quaternion.identity);
        else Instantiate(player, spawnPos.position, Quaternion.identity);
    }
}
