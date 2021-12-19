using UnityEngine;

namespace RPG.Statistics
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] [Range(1,99)] int level = 1;
        public int Level { get { return level; } }
        [SerializeField] int experiencePoints;
        [SerializeField] CharacterClass characterClass;
        public CharacterClass CharClass {get { return characterClass; } }
        [SerializeField] Progression progression;

        public float GetStat(Stats stat)
        {
            return progression.GetStat(stat, characterClass, level);
        }
    }
}

