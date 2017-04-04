using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace JSONFactory
{
    class JSONAssembly
    {
        private static Dictionary<int, string> _resourceList = new Dictionary<int, string>
        {
            {1, "/Resources/Dialogue_Scripts/Event1.json" }
        };

        public static DialogueEvent RunJSONFactoryForScene(int sceneNumber)
        {
            string resourcePath = PathForScene(sceneNumber);

            if (IsValidJSON(resourcePath) == true)
            {
                string jsonString = File.ReadAllText(Application.dataPath + resourcePath);
                DialogueEvent de = JsonMapper.ToObject<DialogueEvent>(jsonString);

                return de;
            }
            else
            {
                throw new Exception("The JSON is not valid, please check the schema and file extension");
            }
        }

        private static string PathForScene(int sceneNumber)
        {
            string resourcePathResult;

            if (_resourceList.TryGetValue(sceneNumber, out resourcePathResult))
                return _resourceList[sceneNumber];
            else throw new Exception("The scene number you provided is not in the resource list. Please check the JSONFactory namespace");
        }

        private static bool IsValidJSON(string path)
        {
            return (Path.GetExtension(path) == ".json") ? true : false;
        }
    }
}