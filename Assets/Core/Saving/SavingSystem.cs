using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
                byte[] buffer = SerializeVector(GetPlayerTransform().position);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Loading to " + path);
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                Vector3 position = DeserializeVector(buffer);
                SetPlayerTransform(position);
            }
        }
        Transform GetPlayerTransform()
        {
            return GameObject.FindGameObjectWithTag("Player").transform;
        }

        void SetPlayerTransform(Vector3 position)
        {
            Transform playerTransform = GetPlayerTransform();
            playerTransform.position = position;
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

