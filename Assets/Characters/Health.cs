using RPG.Core;
using System;
using RPG.Saving;
using RPG.Statistics;
using UnityEngine;
using RPG.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        [SerializeField] TakeDamageEvent takeDamage;
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> {}

        LazyValue<float> healthPoints;

        BaseStats baseStats;
        Animator animator;

        public event Action onHealthChanged;


        bool isDead;
        public bool IsDead { get { return isDead; } }

        void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            animator = GetComponent<Animator>();
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stats.HEALTH);
        }

        void Start()
        {
            healthPoints.ForceInit();
            if (IsDead)
            {
                healthPoints.value = 0;
                return;
            }
            if (gameObject.tag == "Player")
            {
                baseStats.onLevelUp += RestoreFullHealth;
            }
        }


        public void TakeDamage(float damage, GameObject instigator)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            if (!isDead && healthPoints.value <= 0)
            {
                Die(instigator);
                return;
            }
            if (gameObject.tag == "Player")
            {
                onHealthChanged();
            }
            takeDamage.Invoke(damage);
        }

        public string GetHealthPoints()
        {
            string value = String.Format("{0:0}/{1:0}", healthPoints.value, GetComponent<BaseStats>().GetStat(Stats.HEALTH));
            return value;
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
            healthPoints.value = GetComponent<BaseStats>().GetStat(Stats.HEALTH);
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
            healthPoints.value = (float)state;
            if (healthPoints.value <= 0) Die(null);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }
    }
}
