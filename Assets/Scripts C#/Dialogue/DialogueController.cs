using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;

public class DialogueController : MonoBehaviour
{

    //public TouchpadInterface ti;
    public static DialogueController instance;
    public bool isActive;
    public int talkingNPCID;

    private int lastPressedOption;
    private bool goToNext = false;
    private int currentStep;

    private void Start()
    {
        if (instance == null)
            instance = this;

        isActive = false;
        //ti.ToggleTI();
    }
    ///DEPRACATED (ONLY USED FOR OLD TOUCHPAD UI)
    public void Interact_Dialogue(int currentSelection)
    {
        if (!isActive)
        {
            //Debug.Log("Attempting to start dialogue");
            //StartCoroutine(DialogueSession());
            return;
        }
        else
        {
            PressSelectedOption(currentSelection);
        }
    }

    public IEnumerator DialogueSession(int index)
    {
        Debug.Log("Conversation started");
        isActive = true;
        DialogueEvent[] de = JSONAssembly.RunJSONFactoryForScene(index);
        int nextSelection = 0;
        talkingNPCID = de[0].NPC_ID;

        for (int i = 0; i < de.Length; i = nextSelection)
        {
            if(de[i].AudioFile != "")
            {
                AudioClip ac = Resources.Load<AudioClip>("Dialogue/Audio/" + de[i].AudioFile);
                NPCManager.instance.npcs[de[i].NPC_ID].PlayVoice(ac);
                if (ac != null)
                    yield return new WaitForSeconds(ac.length);
                else throw new System.Exception("Could not retrieve audio file: "+de[i].AudioFile);
            }
            ResponseUI.instance.UpdateResponses(de[i].Responses);

            //ti.ConfigureMenu(TouchpadState.DialogueSelect);
            //ti.UpdateText(de[i].Responses);

            yield return new WaitUntil(() => goToNext == true);
            goToNext = false;

            if (i != de.Length - 1)
            {
                int lastSelection = nextSelection;
                nextSelection = de[i].Responses[lastPressedOption].NextTextID;

                if (nextSelection == lastSelection || nextSelection == -1)
                    break;
            }
        }
        Debug.Log("Conversation ended");

       // ti.gameObject.SetActive(false);
        isActive = false;
    }
    // Called whenever an option is pressed
    public void PressSelectedOption(int pressedOption)
    {
        lastPressedOption = pressedOption;
        goToNext = true;
    }
}
