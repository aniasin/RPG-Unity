using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] Transform target;

        Mover mover;
        Fighter fighter;


        private void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        void Update()
        {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;         
        }

        bool InteractWithCombat()
        {
            RaycastHit[] hitResults = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hitResults)
            {
                EnemyTarget target = hit.transform.GetComponent<EnemyTarget>();
                if (!target) continue; //Continue the loop, skipping the following.

                if (Input.GetMouseButton(0))
                {                
                    fighter.Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        bool InteractWithMovement()
        {
            RaycastHit hitResult;
            if (Physics.Raycast(GetMouseRay(), out hitResult))
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(hitResult.point, true);                    
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {      
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        //Broadcast from Health
        public void Death()
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            enabled = false;
        }

    }
}

