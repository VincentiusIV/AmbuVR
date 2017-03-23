using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingsController : MonoBehaviour {

    // Serialize fields
    [SerializeField]private List<IA_Tags> correctOrder = new List<IA_Tags>();
    private List<IA_Tags> placeOrder = new List<IA_Tags>();

    [SerializeField]bool autoConfigure = false;
    [SerializeField]GameObject player;
    [SerializeField]GameObject playerVR;
    [SerializeField]Transform spawnPos;

    public bool enableVR;

    // Sound
    public float volume;

    // Reference fields
    private Patient pt;

    void Start()
    {
        pt = GameObject.FindWithTag("Patient").GetComponent<Patient>();

        if(autoConfigure)
        {
            if (SteamVR.active)
                Instantiate(playerVR, spawnPos.position, Quaternion.identity);
            else Instantiate(player, spawnPos.position, Quaternion.identity);
        }
    }

    public void PlaceObject(IA_Tags obj) { placeOrder.Add(obj); }

    void FinishGame()
    {
        // 
        if(placeOrder.Count != correctOrder.Count)
            for (int i = 0; i < correctOrder.Count - placeOrder.Count; i++)
                placeOrder.Add(IA_Tags.None);

        for (int i = 0; i < placeOrder.Count; i++)
        {
            if (placeOrder[i] == correctOrder[i])
            { }// correct
            else { }// false
        }
    }
}
