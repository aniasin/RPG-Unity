using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {

        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                  BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, CatureState());
            }
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Loading to " + path);
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                RestoreState(formatter.Deserialize(stream));
            }
        }

        object CatureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueId()] = saveable.CaptureState();
            }
            return state;
        }

        void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>) state;
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                saveable.RestoreState(stateDict[saveable.GetUniqueId()]);
           
            }
        }


        byte[] SerializeVector(Vector3 vector)
        {
            byte[] vectorBytes = new byte[3 * 4];
            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);
            return vectorBytes;
        }

        Vector3 DeserializeVector(byte[] buffer )
        {
            Vector3 vector;
            vector.x = BitConverter.ToSingle(buffer, 0);
            vector.y = BitConverter.ToSingle(buffer, 4);
            vector.z = BitConverter.ToSingle(buffer, 8);
            return vector;
        }
        string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}

