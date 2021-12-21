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
        [SerializeField] float maxNavProjectDistance = 1f;
        [SerializeField] float maxWalkableDistance = 30f;
        [SerializeField] CursorMapping[] cursorMappings = null;

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
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hitResults = Physics.RaycastAll(GetMouseRay());
            hitResults = hitResults.OrderBy((d) => (d.transform.position - transform.position).sqrMagnitude).ToArray();
            return hitResults;
        }

        bool InteractWithMovement()
        {
            Vector3 targetPosition;
            bool hasHit = RaycastNavMesh(out targetPosition);

            if (hasHit)
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
                NavMeshHit hit;
                bool hasHit = NavMesh.SamplePosition(hitResult.point, out hit, maxNavProjectDistance, 1);
                if (!hasHit) return false;

                NavMeshPath path = new NavMeshPath();
                bool hasPath = NavMesh.CalculatePath(transform.position, hit.position, 1, path);
                if (!hasPath) return false;

                if (path.status != NavMeshPathStatus.PathComplete)return false;
                if (GetPathLength(path) > maxWalkableDistance) return false;

                target = hit.position;
                return true;
            }   
             return false;
        }

        float GetPathLength(NavMeshPath path)
        {
            float distance = 0;
            Vector3 previousCorner = transform.position;
            foreach (Vector3 corner in path.corners)
            {
                distance += Vector3.Distance(previousCorner, corner);
                previousCorner = corner;
                if (distance > maxWalkableDistance) return distance;
            }
            return distance;
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

