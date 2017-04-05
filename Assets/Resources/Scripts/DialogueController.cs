using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;

public class DialogueController : MonoBehaviour {

    //private TouchpadInterface ti;

    private TouchpadOptions to;
    private bool isPressed = false;
    private bool isActive;

    private void Start()
    {
        //ti = GameObject.FindWithTag("TouchpadInterface").GetComponent<TouchpadInterface>();
    }

    public void StartDialogue()
    {
        Debug.Log("Attempting to start dialogue");

        if (!isActive)
            StartCoroutine(DialogueSession());
    }

    IEnumerator DialogueSession()
    {
        Debug.Log("Conversation started");
        isActive = true;
        DialogueEvent[] de = JSONAssembly.RunJSONFactoryForScene(1);

        int nextSelection = 0;

        for (int i = 0; i < de.Length; i = nextSelection)
        {
            UpdateNPC(de[i].TextLine);
            UpdateTI(de[i].Responses);
            yield return new WaitUntil(() => isPressed == true);
            isPressed = false;
            nextSelection = de[i].Responses[(int)to].NextTextID;
        }
        Debug.Log("Conversation ended");
        isActive = false;
    }

    public void PressSelectedOption(TouchpadOptions _to)
    {
        to = _to;
        isPressed = true;
    }

    // TODO
    // - Play corresponding animation & sound
    private void UpdateNPC(string line)
    {
        Debug.Log(line);

    }

    // TODO
    // - Update response text when response can be said
    private void UpdateTI(Response[] responses)
    {
        for (int i = 0; i < responses.Length; i++)
        {
            Debug.Log(responses[i].ResponseText);
        }
        
    }
}
