using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using UnityEngine.AI;
using RPG.Utils;

namespace RPG.Control
{
    public class AIControl : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float aggroCoolDownTime = 5f;
        [SerializeField] float suspicionTime = 2f;
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float waypointTollerance = 1f;

        float timeSinceLastChase = Mathf.Infinity;
        float waypointDwellTime = 0;
        LazyValue<Vector3> startingPosition;
        int currentPatrolIndex;
        float timeSinceAggrevated = Mathf.Infinity;

        GameObject target;
        Fighter fighter;
        Mover mover;


        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            startingPosition = new LazyValue<Vector3>(GetStartingPosition);
        }

        void Start()
        {
            startingPosition.ForceInit();
        }

        void Update()
        {
            UpdateTimers();
            if (IsAggrevated())
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

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
            fighter.Attack(GameObject.FindWithTag("Player"));
        }
        void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, chaseDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIControl ai = hit.collider.GetComponent<AIControl>();
                if (ai == null) continue;

                ai.Aggrevate();
            }
        }

        Vector3 GetStartingPosition()
        {
            return transform.position;
        }

        void UpdateTimers()
        {
            timeSinceLastChase += Time.deltaTime;
            waypointDwellTime += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        bool IsAggrevated()
        {
            GameObject player = GameObject.FindWithTag("Player");
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < aggroCoolDownTime;
        }

        void ChaseTarget()
        {
            timeSinceLastChase = 0;
            GameObject target = GameObject.FindWithTag("Player");
            timeSinceLastChase = 0;
            fighter.Attack(target);
            AggrevateNearbyEnemies();
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
                mover.StartMoveAction(startingPosition.value, false);
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
