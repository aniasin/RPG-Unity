using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float gizmoRadius = 0.3f;

        void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i ++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), gizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }
        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int index)
        {
            if (index == transform.childCount -1)
            {
                return 0;
            }
            return index + 1;
        }

    }
}
