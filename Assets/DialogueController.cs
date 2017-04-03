using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;

public class DialogueController : MonoBehaviour {

    private TouchpadInterface ti;

    private void Start()
    {
        //DialogueEvent newEvent = JSONAssembly.GetDialogueList(1);
        
        //Debug.Log(string.Format("{0} {1}", newEvent.dialogues[0].NPC_ID, newEvent.dialogues[0].textLine));

        Dialogue dstruct = new Dialogue();
        dstruct.NPC_ID = 1;

    }

}
