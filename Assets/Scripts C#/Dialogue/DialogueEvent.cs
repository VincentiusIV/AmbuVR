using System;
using System.Collections.Generic;
using LitJson;

public enum StressLevel { Calm = 1, Agitated = 2, Furious = 3}

[System.Serializable]
public class DialogueEvent
{
    public int NPC_ID;
    public int TextID;
    public string TextLine;
    public string AudioFile;
    public Response[] Responses;
}
[System.Serializable]
public class Response
{
    public string ResponseText;
    public int NextTextID;
}