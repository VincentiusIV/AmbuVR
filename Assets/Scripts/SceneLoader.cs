using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class SceneLoader : MonoBehaviour {

    [SerializeField] string dbName;
    [SerializeField] TextMesh results;

    private JsonData sceneData;
    private SceneSettings sceneSet;

    // Use this for initialization
    void Start ()
    {
        // Read from SceneData
        sceneData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/" + dbName + ".json"));
        // Apply values in simulation
        AcquireData();
        DisplayResults();
    }

    void AcquireData()
    {
        sceneSet = new SceneSettings(
                                     (string)sceneData[0]["SceneName"],
                                     (int)sceneData[0]["EnvironmentID"],
                                     (int)sceneData[0]["Difficulty"]
                                    );
    }

    void DisplayResults()
    {
        results.text =  "Scene Succesfully loaded! Name: " + sceneSet.SceneName + 
                        " EnviroID: " + (EnviroID)sceneSet.EnvironmentID + 
                        " Difficulty: " + (Difficulty)sceneSet.Difficulty;
    }
}
