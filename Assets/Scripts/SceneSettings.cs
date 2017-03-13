using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneSettings
{
    public string SceneName { get; set; }
    public int EnvironmentID { get; set; }
    public int Difficulty { get; set; }

    public SceneSettings(string name, int envID, int diff)
    {
        SceneName = name;
        EnvironmentID = envID;
        Difficulty = diff;
    }

    public SceneSettings()
    {
        SceneName = "default";
    }
}

[System.Serializable]
public enum EnviroID
{
    None = 0,
    Office = 1,
    Church = 2,
    Bakery = 3,
}

[System.Serializable]
public enum Difficulty
{
    None = 0,
    Easy = 1,
    Medium = 2,
    Hard = 3,
}
