using UnityEditor;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/Make New Progression", order = 0)]
    public class Progression : StyledScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;
        internal float GetHealth(CharacterClass characterClass, int characterLevel)
        {            
            foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
            {
                if (progressionCharacterClass.characterClass == characterClass)
                {
                    return progressionCharacterClass.health[characterLevel];
                }
            }
            return 0;
        }

        

       [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            [Range(1,99)] public int characterLevel;
            public float[] health;
        }
    }
}
