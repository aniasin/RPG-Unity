using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        public CursorType GetCursorType()
        {
            if (pickup.CanBePickedUp())
            {
                return CursorType.PICKUP;
            }
            else
            {
                return CursorType.PICKUP;
            }
        }

        public bool HandleRaycast(PlayerControl callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pickup.PickupItem();
            }
            return true;
        }
    }
}
