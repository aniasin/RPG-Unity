using RPG.Utils;
using System;
using System.Collections;
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

        Experience experience;

        public event Action onLevelUp; 

        LazyValue<int> currentLevel;

        void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        void Start()
        {
            currentLevel.ForceInit();
        }

        void OnEnable()
        {
            if (experience != null)
            {
                experience.OnXpAwarded += XpAwarded;
            }
        }

        void OnDisable()
        {
            if (experience != null)
            {
                experience.OnXpAwarded -= XpAwarded;
            }
        }

        void XpAwarded()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                onLevelUp();
                SpawnLevelUpFx();
            }
        }

        public float GetStat(Stats stat)
        {
            float baseStat = progression.GetStat(stat, characterClass, GetLevel());
            float addedStat = GetAdditiveModifiers(stat);
            float multiplierStat = GetMultiplierModifiers(stat); // Will return at least 1
            return (baseStat + addedStat) * multiplierStat;
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        public float GetDamage()
        {
            float damage = GetStat(Stats.DAMAGE);
            return damage;
        }

        int CalculateLevel()
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

        float GetAdditiveModifiers(Stats stat)
        {
            float cumulatedModifiers = 0;
            foreach(IModifierProvider modifier in GetComponents<IModifierProvider>())
            {
                foreach(float mod in modifier.GetAdittiveModifiers(stat))
                {
                    cumulatedModifiers += mod;
                }                
            }
            return cumulatedModifiers;
        }

         float GetMultiplierModifiers(Stats stat)
        {
            float cumulatedMultipliers = 1;
            foreach (IModifierProvider modifier in GetComponents<IModifierProvider>())
            {
                foreach (float mod in modifier.GetMultiplierModifiers(stat))
                {
                    if (mod <= 0) continue;
                    cumulatedMultipliers += mod / 100;
                }
            }
            return cumulatedMultipliers;
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

