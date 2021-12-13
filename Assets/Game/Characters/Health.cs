using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100;

        Animator animator;

        bool isDead;
        public bool IsDead { get { return isDead; } }

        void Awake()
        {
            animator = GetComponent<Animator>();
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

    }
}
