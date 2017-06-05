using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;

public class SceneCreator : MonoBehaviour {

    [SerializeField]string dbName;

    private JsonData sceneData;
    // holds presets for scenes
    private List<SceneSettings> sceneSet = new List<SceneSettings>();

	void Start ()
    {
        //CreateSpots(20);
        // acquire json data
        sceneSet.Add(new SceneSettings());
        //AcquireData();
    }

    public int SetEnvironment{set { sceneSet[0].EnvironmentID = value; }}
    public int SetDifficulty{set { sceneSet[0].Difficulty = value; }}

    public void FinishScene()
    {
        sceneSet[0].SceneName = ((EnviroID)sceneSet[0].EnvironmentID).ToString();
        WriteToDatabase();
        SceneManager.LoadScene(((EnviroID)sceneSet[0].EnvironmentID).ToString());
    }

    void WriteToDatabase()
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint = true;
        writer.IndentValue = 1;

        JsonMapper.ToJson(sceneSet, writer);
        Debug.Log(sb.ToString());
        File.WriteAllText(Application.dataPath + "/StreamingAssets/" + dbName + ".json", sb.ToString());
    }
}


