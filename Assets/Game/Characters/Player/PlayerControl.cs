using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Control
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] Transform target;

        Mover mover;


        private void Awake()
        {
            mover = GetComponent<Mover>();
        }

        void Update()
        {
            GetInput();
        }

        void GetInput()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        void MoveToCursor()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                mover.MoveTo(hit.point);
            }
        }
    }
}

