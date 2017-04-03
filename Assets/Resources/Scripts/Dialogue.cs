using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public enum StressLevel { Calm = 1, Agitated = 2, Furious = 3}

public class DialogueEvent
{
    public List<Dialogue> dialogues { get; set; }
}

public struct Dialogue
{
    public int      NPC_ID { get; set; }
    public int      textID { get; set; }
    public string   textLine { get; set; }
    public string   audioFile { get; set; }
    // animation
    public int      Fx_stress { get; set; }

    //public List<ResponseJSON> Responses { get; set; }
}

public struct ResponseJSON
{
    public Response Response { get; set; }
}

public struct Response
{
    public string ResponseText { get; set; }
    public int NextTextID { get; set; }
}

