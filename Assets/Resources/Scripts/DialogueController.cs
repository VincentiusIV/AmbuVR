using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;

public class DialogueController : MonoBehaviour {

    private TouchpadInterface ti;
    private int lastPressedOption;
    private bool isPressed = false;
    private bool isActive = false;

    // Continue with this
    private DialogueEvent[] currentDialogueEvent;
    private int currentStep;

    private void Start()
    {
        ti = GameObject.FindWithTag("TouchpadInterface").GetComponent<TouchpadInterface>();
    }

    public void Interact_Dialogue(int currentSelection)
    {
        if (!isActive)
        {
            Debug.Log("Attempting to start dialogue");
            StartCoroutine(DialogueSession());
            return;
        }
        else
        {
            PressSelectedOption(currentSelection);
        }
            
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
            ti.UpdateText(de[i].Responses);
            yield return new WaitUntil(() => isPressed == true);
            isPressed = false;
            nextSelection = de[i].Responses[(int)lastPressedOption].NextTextID;
        }
        Debug.Log("Conversation ended");
        isActive = false;
    }

    public void PressSelectedOption(int _to)
    {
        lastPressedOption = _to;
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
