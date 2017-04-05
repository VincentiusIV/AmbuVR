using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;

public class DialogueController : MonoBehaviour {

    private TouchpadInterface ti;

    private void Start()
    {
        DialogueEvent[] de = JSONAssembly.RunJSONFactoryForScene(1);
        Debug.Log(de[0].NPC_ID + de[0].TextLine);
        Debug.Log(de[1].NPC_ID + de[1].TextLine);
    }

}
