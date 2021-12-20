using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISavable
    {
        [SerializeField] float walkSpeed = 2f;
        [SerializeField] float runSpeed = 5f;

        NavMeshAgent navMeshAgent;
        Animator animator;

        // Start is called before the first frame update
        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, bool running)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, running);
        }

        public void MoveTo(Vector3 destination, bool running)
        {
            if (!navMeshAgent.isActiveAndEnabled) return;
            float speed = running ? runSpeed : walkSpeed;
            navMeshAgent.speed = speed;
            navMeshAgent.destination = destination;            
        }

        public void Cancel()
        {
            navMeshAgent.destination = transform.position;
        }
        
        void UpdateAnimator()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
            float speed = localVelocity.z;
            animator.SetFloat("speed", speed);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().Warp(position.GetVector3());
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }
    }

}


