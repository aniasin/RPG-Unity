using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab = null;
        static bool hasSpawn = false;

        void Awake()
        {
            if (hasSpawn) return;

            SPawnPersistentObject();
            hasSpawn = true;
        }

        void SPawnPersistentObject()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}

