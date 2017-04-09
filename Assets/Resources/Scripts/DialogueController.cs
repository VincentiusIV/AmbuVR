using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;

public class DialogueController : MonoBehaviour {

    private TouchpadInterface ti;
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
        ti = GameObject.FindWithTag("TouchpadInterface").GetComponent<TouchpadInterface>();
        ti.ToggleTI();
        npcs = GameObject.FindGameObjectsWithTag("AI");
        Debug.Log(npcs.Length + npcs[0].name);
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
            if(de[i].AudioFile != "")
            {
                AudioClip ac = Resources.Load<AudioClip>("Dialogue/Audio/" + de[i].AudioFile);
                npcs[de[i].NPC_ID].GetComponent<AI_Movement>().PlayVoice(ac);
                yield return new WaitForSeconds(ac.length);
            }
            ti.ConfigureMenu(TouchpadState.DialogueSelect, de[i].Responses.Length);
            ti.UpdateText(de[i].Responses);
            yield return new WaitUntil(() => isPressed == true);
            isPressed = false;

            npcs[de[i].NPC_ID].GetComponent<AI_Movement>().StressLevel = de[i].Responses[nextSelection].Fx_stress;

            if(i != de.Length - 1)
            {
                int lastSelection = nextSelection;
                nextSelection = de[i].Responses[lastPressedOption].NextTextID;

                if (nextSelection == lastSelection)
                    break;
            }
        }
        Debug.Log("Conversation ended");
        ti.gameObject.SetActive(false);
        isActive = false;
    }

    public void PressSelectedOption(int _to)
    {
        lastPressedOption = _to;
        isPressed = true;
    }
}
