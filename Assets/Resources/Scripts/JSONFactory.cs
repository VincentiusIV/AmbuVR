using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
/// <summary>
/// Namespace JSONFactory;
/// - Return arrays of data from .json files
/// - In a namespace so that it does not need 
/// to be on a gameobject and can be called anywhere
/// </summary>
namespace JSONFactory
{
    class JSONAssembly
    {
        private static Dictionary<int, string> _resourceList = new Dictionary<int, string>
        {
            {1, "/Resources/Dialogue/Scripts/Event1.json" },
            {2, "/Resources/Dialogue/Scripts/Event2.json" }
        };

        public static DialogueEvent[] RunJSONFactoryForScene(int sceneNumber)
        {
            string resourcePath = PathForScene(sceneNumber);

            if (IsValidJSON(resourcePath))
            {
                string jsonString = File.ReadAllText(Application.dataPath + resourcePath);
                DialogueEvent[] de = JsonMapper.ToObject<DialogueEvent[]>(jsonString);

                return de;
            }
            else
            {
                throw new Exception("The JSON is not valid, please check the schema and file extension");
            }
        }
        /// <summary>
        /// Returns string for given scene number
        /// </summary>
        /// <param name="sceneNumber"></param>
        /// <returns></returns>
        private static string PathForScene(int sceneNumber)
        {
            string resourcePathResult;

            if (_resourceList.TryGetValue(sceneNumber, out resourcePathResult))
                return _resourceList[sceneNumber];
            else throw new Exception("The scene number you provided is not in the resource list. Please check the JSONFactory namespace");
        }
        /// <summary>
        /// Checks if the path is directing to a .json file
        /// </summary>
        /// <param name="path"></param>
        /// the path string
        /// <returns></returns>
        private static bool IsValidJSON(string path)
        {
            return (Path.GetExtension(path) == ".json") ? true : false;
        }
    }
}