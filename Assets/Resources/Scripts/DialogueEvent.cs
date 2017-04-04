using System;
using System.Collections.Generic;
using LitJson;

public enum StressLevel { Calm = 1, Agitated = 2, Furious = 3}

public class DialogueEvent
{
    public Dialogue[] dialogues;
}

public struct Dialogue
{
    public int NPC_ID;
    public int TextID;
    public string textLine;
    public string audioFile;
    // animation
    public int Fx_stress;
    public int[] Responses;
}


