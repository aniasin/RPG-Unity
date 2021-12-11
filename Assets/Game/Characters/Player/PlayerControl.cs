using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] Transform target;

    Animator animator;
    NavMeshAgent navMeshAgent;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        MoveToCursor();
        UpdateAnimator();
    }

    void MoveToCursor()
    {     
        if (Input.GetMouseButton(0))
        {
          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 3f);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {          
                navMeshAgent.destination = hit.point;
            }      
        }
    }

    void UpdateAnimator()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
        float speed = localVelocity.z;
        animator.SetFloat("speed", speed);
    }
}
