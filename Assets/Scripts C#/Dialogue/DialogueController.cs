using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;

public class DialogueController : MonoBehaviour
{

    //public TouchpadInterface ti;
    public static DialogueController instance;
    public bool isActive;
    public int activeID;
    public int talkingNPCID;

    private int lastPressedOption;
    private bool goToNext = false;
    private int currentStep;

    public List<Transform> customLocations = new List<Transform>();

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
        Debug.Log("Starting dialogue index " + index);
        isActive = true;
        activeID = index;
        int nextSelection = 0;
        // Get DialogueEvent array from the JSONAssembly class
        DialogueEvent[] de = JSONAssembly.RunJSONFactoryForScene(index);
        // Store which npc will start talking (for pathfinding)
        talkingNPCID = de[0].NPC_ID;
        
        // Loop through all the dialogue events
        for (int i = 0; i < de.Length; i = nextSelection)
        {
            if(de[i].AudioFile != "")   // Only load audio when a link is specified
            {
                AudioClip ac = Resources.Load<AudioClip>("Sounds/Dialogue/Audio/" + de[i].AudioFile);

                if(ac != null)          // If an audio clip was found
                {
                    if(!NPCManager.instance.npcs[talkingNPCID].reachedPlayer)
                    {
                        // Before anything begins, the ai that is supposed to speak has to walk to the player
                        NPCManager.instance.npcs[instance.talkingNPCID].ChangeBehaviour(AIBehaviourState.Follow);
                        // Wait untill npc has reached player
                        yield return new WaitUntil(() => NPCManager.instance.npcs[talkingNPCID].reachedPlayer);
                    }
                    
                    // Let the NPC play the corresponding voice
                    NPCManager.instance.npcs[de[i].NPC_ID].PlayVoice(ac, (AIEmotionalState)de[i].AngerLevel);
                    // Wait untill NPC is done talking
                    yield return new WaitForSeconds(ac.length);
                }
                else throw new System.Exception("Could not retrieve audio file: "+de[i].AudioFile);
            }
            // Print responses on the response buttons
            UIController.instance.UpdateResponses(de[i].Responses, NPCManager.instance.npcs[de[i].NPC_ID].transform);
            // Wait untill a dialogue option is pressed
            yield return new WaitUntil(() => goToNext == true);
            goToNext = false;
            // If this is not the last event, continue to the next event
            if (i != de.Length - 1)
            {
                int lastSelection = nextSelection;
                nextSelection = de[i].Responses[lastPressedOption].NextTextID;

                if (nextSelection == lastSelection || nextSelection == -1)
                    break;
            }
            else if (de[i].Responses[lastPressedOption].MoveNPCToLocation > -1)
            {
                int locIndex = de[i].Responses[lastPressedOption].MoveNPCToLocation;
                NPCManager.instance.npcs[de[i].NPC_ID].ChangeBehaviour(AIBehaviourState.Command, customLocations[locIndex].position);
                break;
            }
            else break;
        }
        isActive = false;
    }
    // Called whenever an option is pressed
    public void PressSelectedOption(int pressedOption)
    {
        lastPressedOption = pressedOption;
        goToNext = true;
    }

    public void ForceQuitDialogue()
    {
        if(isActive)
        {
            StopAllCoroutines();
        }
    }
}
