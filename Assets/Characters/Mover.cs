using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
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
    }

}


