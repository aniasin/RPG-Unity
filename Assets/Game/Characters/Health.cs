using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
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
            GetComponent<CapsuleCollider>().enabled = false;
            animator.SetTrigger("death");
        }

    }
}
