using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> stateDict = LoadFile(saveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (stateDict.ContainsKey("SceneBuildIndex"))
            {
                buildIndex = (int)stateDict["SceneBuildIndex"];
            }
            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreState(stateDict);      
        }

        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);               
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        public void Erase(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path)) return new Dictionary<string, object>();

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueId()] = saveable.CaptureState();
            }
            state["SceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        void RestoreState(Dictionary<string, object> stateDict)
        {            
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueId();
                if (stateDict.ContainsKey(id))
                {
                    saveable.RestoreState(stateDict[id]);
                }              
            }
        }

        string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}

