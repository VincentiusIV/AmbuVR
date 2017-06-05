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

    public void UpdateResponses(Response[] responses)
    {
        for (int i = 0; i < responses.Length; i++)
        {
            responseButtons[i].textMesh.text = responses[i].ResponseText;
        }
        ToggleVisible(true, responses.Length);
    }

    public void ToggleVisible(bool state, int amount = 4)
    {
        for (int i = 0; i < amount; i++)
        {
            responseButtons[i].GetComponent<MeshRenderer>().enabled = state;
            responseButtons[i].transform.GetChild(0).gameObject.SetActive(state);
        }
    }
}
