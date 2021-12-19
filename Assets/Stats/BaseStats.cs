using UnityEngine;

namespace RPG.Statistics
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] [Range(1, 99)] int startingLevel = 1;
        public int Level { get { return startingLevel; } }
        [SerializeField] int experiencePoints;
        [SerializeField] CharacterClass characterClass;
        public CharacterClass CharClass { get { return characterClass; } }
        [SerializeField] Progression progression;

        private void Update()
        {
            print("CURRENT LEVEL: " + GetLevel());
        }

        public float GetStat(Stats stat)
        {
            return progression.GetStat(stat, characterClass, startingLevel);
        }

        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float currentXP = experience.Xp;
            int penultimateLevel = progression.GetLevels(Stats.EXPERIENCETOLEVELUP, characterClass);
            for (int level = 1; level < penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stats.EXPERIENCETOLEVELUP, characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }
    }
}

