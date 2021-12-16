using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        public string GetUniqueId()
        {
            return uniqueIdentifier;
        } 

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISavable savable in GetComponents<ISavable>())
            {
                state[savable.GetType().ToString()] = savable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISavable savable in GetComponents<ISavable>())
            {
                string typeString = savable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    savable.RestoreState(stateDict[typeString]);
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; // ensure we're in a Scene and not Prefab

            SerializedObject sereliazedObject = new SerializedObject(this);
            SerializedProperty property = sereliazedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                sereliazedObject.ApplyModifiedProperties();
            }
        }
#endif

    }
}

