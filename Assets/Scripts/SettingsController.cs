using UnityEngine;
using System.Collections;

public class SettingsController : MonoBehaviour {

    [SerializeField]GameObject player;
    [SerializeField]GameObject playerVR;
    [SerializeField]Transform spawnPos;

    public bool enableVR;

    void Start()
    {
        if (enableVR)
            Instantiate(playerVR, spawnPos.position, Quaternion.identity);
        else Instantiate(player, spawnPos.position, Quaternion.identity);
    }
}
