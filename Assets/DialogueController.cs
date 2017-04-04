using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;

public class DialogueController : MonoBehaviour {

    private TouchpadInterface ti;

    private void Start()
    {
        DialogueEvent de = JSONAssembly.RunJSONFactoryForScene(1);
        Debug.Log(de.dialogues[0].NPC_ID + de.dialogues[0].textLine);
    }

}
