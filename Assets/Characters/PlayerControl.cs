using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using UnityEngine.AI;
using RPG.Attributes;
using UnityEngine.EventSystems;
using System.Linq;
using System;

namespace RPG.Control
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float raycastRadius = 1f;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        Mover mover;
        Fighter fighter;
        Health health;

        public bool UnityEventSystem { get; private set; }

        private void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (InteractWithUi()) return;

            if (health.IsDead)
            {
                SetCursor(CursorType.NONE);
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.NONE);
        }

        bool InteractWithUi()
        {
            bool isOnUi = EventSystem.current.IsPointerOverGameObject();
            if (isOnUi) SetCursor(CursorType.UI);
            return isOnUi;
        }

        bool InteractWithComponent()
        {
            RaycastHit[] hitResults = RaycastAllSorted();
            foreach (RaycastHit hit in hitResults)
            {
                if (!mover.CanMoveTo(hit.transform.position) && !fighter.InRange(hit.transform.position)) continue;
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (!raycastable.HandleRaycast(this)) continue;
                    SetCursor(raycastable.GetCursorType());
                    return true;
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hitResults = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            hitResults = hitResults.OrderBy((d) => (d.transform.position - transform.position).sqrMagnitude).ToArray();
            return hitResults;
        }

        bool InteractWithMovement()
        {
            if (Input.GetKey(KeyCode.LeftShift)) return false;
            Vector3 targetPosition;
            bool hasHit = RaycastNavMesh(out targetPosition);

            if (hasHit && mover.CanMoveTo(targetPosition))
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(targetPosition, true);                    
                }
                SetCursor(CursorType.MOVEMENT);
                return true;
            }
            return false;
        }

        bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hitResult;
            if(Physics.Raycast(GetMouseRay(), out hitResult))
            {
                if (mover.CanMoveTo(hitResult.point))
                {
                    target = hitResult.point;
                    return true;
                }
            }   
             return false;
        }

        void SetCursor(CursorType type)
        {
            CursorMapping cursorMapping = GetCursorMapping(type);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }

        CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping cursorMap in cursorMappings)
            {
                if (cursorMap.type != type) continue;
                return cursorMap;
            }
            return cursorMappings[0];
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

