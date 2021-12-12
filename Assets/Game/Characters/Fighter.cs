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
        Transform target;

        void Update()
        {
            if (target)
            {
                GetComponent<Mover>().MoveTo(target.position);
                if (Vector3.Distance(target.position, transform.position) <= weaponRange)
                {
                    GetComponent<Mover>().Cancel();
                }
            }
        }
        public void Attack(EnemyTarget enemyTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = enemyTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }

    
}

