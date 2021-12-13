using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2;
        [SerializeField] float weaponDamage = 10;
        [SerializeField] float weaponSpeed = 1;

        float timeSinceLastAttack;
        Health target;
        Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (!target || target.IsDead)  return;
           GetComponent<Mover>().MoveTo(target.transform.position);
           if (Vector3.Distance(target.transform.position, transform.position) <= weaponRange)
           {
                GetComponent<Mover>().Cancel();
                AttackBehavior();                
           }
        }
        public void Attack(EnemyTarget enemyTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = enemyTarget.GetComponent<Health>();
        }

        void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= weaponSpeed)
            {                
                //Triggers Hit() event.
                timeSinceLastAttack = 0;
                TriggerAttack();
            }
        }

        void TriggerAttack()
        {
            animator.ResetTrigger("stopAttacking");
            animator.SetTrigger("IsAttacking");
        }

        //Animation Event
        void Hit()
        {
            if (!target) return;
            target.TakeDamage(weaponDamage);
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }
        void StopAttack()
        {
            animator.ResetTrigger("IsAttacking");
            animator.SetTrigger("stopAttacking");
        }

    }
}

