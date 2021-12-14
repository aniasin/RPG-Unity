using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIControl : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 2f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTollerance = 1f;

        float timeSinceLastChase = Mathf.Infinity;
        float waypointDwellTime = 0;
        Vector3 startingPosition;
        int currentPatrolIndex;

        GameObject target;
        Fighter fighter;
        Mover mover;


        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            startingPosition = transform.position;
        }

        void Update()
        {
            UpdateTimers();
            if (InAttackRange())
            {
                ChaseTarget();
            }
            else if (timeSinceLastChase < suspicionTime)
            {
                StopChase();
            }
            else
            {
                BackToRoutine();
            }
        }

        void UpdateTimers()
        {
            timeSinceLastChase += Time.deltaTime;
            waypointDwellTime += Time.deltaTime;
        }

        bool InAttackRange()
        {
            target = GameObject.FindWithTag("Player");
            float currentDistance = Vector3.Distance(target.transform.position, transform.position);
            return currentDistance <= chaseDistance;
        }

        void ChaseTarget()
        {
            timeSinceLastChase = 0;
            fighter.Attack(target);
        }

    void StopChase()
        {
            mover.StartMoveAction(transform.position, false);
        }

    void BackToRoutine()
        {
            if (patrolPath)
            {
                PatrolAlongPath();
            }
            else
            {
                mover.StartMoveAction(startingPosition, false);
            }
        }

        void PatrolAlongPath()
        {
            CycleWaypoint();
            return;
        }
        void CycleWaypoint()
        {
            if (!IsAtCurrentWaypoint(currentPatrolIndex))
            {
                mover.StartMoveAction(patrolPath.GetWaypoint(currentPatrolIndex), false);
                waypointDwellTime = 0;
                return;
            }
            if (waypointDwellTime > suspicionTime)
            {
                currentPatrolIndex = patrolPath.GetNextIndex(currentPatrolIndex);
            }            
        }

        bool IsAtCurrentWaypoint(int index)
        {
            Transform currentWaypoint = patrolPath.transform.GetChild(index);
            float distanceToWaypoint = Vector3.Distance(transform.position, currentWaypoint.position);
            return distanceToWaypoint <= waypointTollerance;
        }

        //Broadcast from Health
        public void Death()
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            enabled = false;
        }

        //Called by Unity
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
