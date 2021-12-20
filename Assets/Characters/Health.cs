using RPG.Core;
using System;
using RPG.Saving;
using RPG.Statistics;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        float healthPoints = 100;

        BaseStats baseStats;
        Animator animator;

        public event Action onHealthChanged;


        bool isDead;
        public bool IsDead { get { return isDead; } }

        void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            if (IsDead)
            {
                healthPoints = 0;
                return;
            }
            if (gameObject.tag == "Player")
            {
                baseStats.onLevelUp += RestoreFullHealth;
            }
            healthPoints = GetComponent<BaseStats>().GetStat(Stats.HEALTH);
        }


        public void TakeDamage(float damage, GameObject instigator)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (!isDead && healthPoints <= 0)
            {
                Die(instigator);
            }
            if (gameObject.tag == "Player")
            {
                onHealthChanged();
            }
        }

        public float GetHealthPercentage()
        {
            if (healthPoints <= 0) return 0;
            return (healthPoints / GetComponent<BaseStats>().GetStat(Stats.HEALTH)) * 100;
        }
        void Die(GameObject instigator)
        {
            isDead = true;
            animator.SetTrigger("death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            gameObject.BroadcastMessage("Death");
            if (!instigator) return;
            AwardExperience(instigator);
        }

        void RestoreFullHealth()
        {
            healthPoints = GetComponent<BaseStats>().GetStat(Stats.HEALTH);
            if (gameObject.tag == "Player")
            {
                onHealthChanged();
            }
        }

        void AwardExperience(GameObject instigator)
        {            
            if (!instigator) return;
            Experience experienceComp = instigator.GetComponent<Experience>();
            if (!experienceComp) return;
            CharacterClass characterClass = GetComponent<BaseStats>().CharClass;
            int level = GetComponent<BaseStats>().Level;
            experienceComp.GainExperience(baseStats.GetStat(Stats.EXPERIENCE));
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints <= 0) Die(null);
        }

        public object CaptureState()
        {
            return healthPoints;
        }
    }
}
