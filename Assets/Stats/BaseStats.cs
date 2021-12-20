using System;
using UnityEngine;

namespace RPG.Statistics
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] [Range(1, 99)] int startingLevel = 1;
        public int Level { get { return startingLevel; } }
        [SerializeField] int experiencePoints;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] GameObject levelUpFx;
        public CharacterClass CharClass { get { return characterClass; } }
        [SerializeField] Progression progression;

        public event Action onLevelUp; 

        int currentLevel = 0;

        void Start()
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.OnXpAwarded += XpAwarded;
            }
        }

        void XpAwarded()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                onLevelUp();
                SpawnLevelUpFx();
            }
        }

        public float GetStat(Stats stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (currentLevel < 1) currentLevel = CalculateLevel();
            return currentLevel;
        }

        public int CalculateLevel()
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

        void SpawnLevelUpFx()
        {
            if (levelUpFx)
            {
                GameObject effect = Instantiate(levelUpFx, transform);
                Destroy(effect, 5);
            }
        }
    }
}

