using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;

public class DialogueController : MonoBehaviour {

    public TouchpadInterface ti;
    public Transform hmd;

    private int lastPressedOption;
    private bool isPressed = false;
    public bool isActive { get; private set; }

    // Continue with this
    private DialogueEvent[] currentDialogueEvent;
    private int currentStep;

    private GameObject[] npcs;

    private void Start()
    {
        isActive = false;
        ti.ToggleTI();
        npcs = GameObject.FindGameObjectsWithTag("AI");
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
    int dialogueIndex = 0;

    IEnumerator DialogueSession()
    {
        Debug.Log("Conversation started");
        isActive = true;
        DialogueEvent[] de = JSONAssembly.RunJSONFactoryForScene(dialogueIndex);

        int nextSelection = 0;

        for (int i = 0; i < de.Length; i = nextSelection)
        {
            if(de[i].AudioFile != "")
            {
                AudioClip ac = Resources.Load<AudioClip>("Dialogue/Audio/" + de[i].AudioFile);
                npcs[de[i].NPC_ID].GetComponent<ParentBehaviour>().PlayVoice(ac);
                if (ac != null)
                    yield return new WaitForSeconds(ac.length);
                else throw new System.Exception("Could not retrieve audio file: "+de[i].AudioFile);
            }
            ti.ConfigureMenu(TouchpadState.DialogueSelect);
            ti.UpdateText(de[i].Responses);
            yield return new WaitUntil(() => isPressed == true);
            isPressed = false;

            if (i != de.Length - 1)
            {
                int lastSelection = nextSelection;
                nextSelection = de[i].Responses[lastPressedOption].NextTextID;

                if (nextSelection == lastSelection || nextSelection == -1)
                    break;
            }
        }
        Debug.Log("Conversation ended");
        dialogueIndex++;
       // ti.gameObject.SetActive(false);
        isActive = false;
    }

    public void PressSelectedOption(int _to)
    {
        lastPressedOption = _to;
        isPressed = true;
    }
}
