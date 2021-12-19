using System.Collections.Generic;
using UnityEngine;

namespace RPG.Statistics
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/Make New Progression", order = 0)]
    public class Progression : StyledScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        Dictionary<CharacterClass, Dictionary<Stats, float[]>> lookUpTable = null;

        void BuildLookUp()
        {
            if (lookUpTable != null) return;
            lookUpTable = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();
            
            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                Dictionary<Stats, float[]> statsDict = new Dictionary<Stats, float[]>();
                foreach (ProgressionStat progressionStat in progressionClass.stats)                {
                    
                    statsDict[progressionStat.stat] = progressionStat.values;
                    lookUpTable[progressionClass.characterClass] = statsDict;
                }
            }
        }

        internal float GetStat(Stats stat, CharacterClass characterClass, int characterLevel)
        {
            BuildLookUp();
            float[] values = lookUpTable[characterClass][stat];
            if (values.Length < characterLevel) return 0;
            return values[characterLevel];
        }        

       [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            [Range(1,99)] public int characterLevel;
            public ProgressionStat[] stats;
        }
        [System.Serializable]
        public class ProgressionStat
        {
            public Stats stat;
            public float[] values;
        }
    }
}
