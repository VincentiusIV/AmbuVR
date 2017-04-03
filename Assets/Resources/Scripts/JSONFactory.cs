using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace JSONFactory
{
    public class JSONAssembly
    {
        JsonData data;

        private static Dictionary<int, string> _resourceList = new Dictionary<int, string>
        {
            {1, "/Resources/Dialogue_Scripts/Event1.json" }
        };

     /*   public static List<Dialogue> GetDialogueList(int eventID)
        {
            string resourcepath = PathForScene(eventID);
            if (IsValidJSON(resourcepath))
            {
                List<Dialogue> newList = new List<Dialogue>();

                string jsonString = File.ReadAllText(Application.dataPath + resourcepath);
                Debug.Log(jsonString);
                //DialogueEvent de = JsonMapper.ToObject<DialogueEvent>(jsonString);
                JsonData data = JsonMapper.ToObject(jsonString);

                
                return de;
            }
            else throw new Exception("JSON is not a valid .json");
        }*/

        public static string PathForScene(int eventID)
        {
            string resourcePathResult;

            if(_resourceList.TryGetValue(eventID, out resourcePathResult))
                return _resourceList[eventID];
            else throw new Exception("The ID you provided does not exist, check JSONFactory namespace");
        }
        
        private static bool IsValidJSON(string path) {return (Path.GetExtension(path) == ".json") ? true : false;}
    }
}
