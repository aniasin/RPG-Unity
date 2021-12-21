using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class EnemyTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.COMBAT;
        }

        public bool HandleRaycast(PlayerControl playerControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerControl.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
        
    }
}
