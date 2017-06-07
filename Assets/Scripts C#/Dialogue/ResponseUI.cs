using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseUI : MonoBehaviour
{
    public static ResponseUI instance;

    public DialogueButton[] responseButtons;

    private void Start()
    {
        if (instance == null)
            instance = this;

        for (int i = 0; i < responseButtons.Length; i++)
        {
            responseButtons[i].option = i;
        }

        ToggleVisible(false);
    }

    public void UpdateResponses(Response[] responses, Transform npcToLookAt)
    {
        for (int i = 0; i < responses.Length; i++)
        {
            responseButtons[i].textMesh.text = responses[i].ResponseText;
        }
        ToggleVisible(true, responses.Length);

        // Aim response ui to the NPC
        Vector3 direction = npcToLookAt.position - transform.parent.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.parent.rotation = rotation;
    }

    public void ToggleVisible(bool state, int amount = 4)
    {
        for (int i = 0; i < amount; i++)
        {
            responseButtons[i].gameObject.SetActive(state);
            responseButtons[i].transform.GetChild(0).gameObject.SetActive(state);
        }
    }
}
