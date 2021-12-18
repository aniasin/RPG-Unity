using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        [SerializeField] float healthPoints = 100;

        Animator animator;

        bool isDead;
        public bool IsDead { get { return isDead; } }

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }


        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (!isDead && healthPoints <= 0)
            {
                Die();
            }
        }
        void Die()
        {
            isDead = true;
            animator.SetTrigger("death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            gameObject.BroadcastMessage("Death");
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints <= 0) Die();
        }

        public object CaptureState()
        {
            return healthPoints;
        }
    }
}
