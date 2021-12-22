using RPG.Control;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class Pickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weaponConfig = null;
        [SerializeField] float timeToRespawn = 5;

        void PickUp(Fighter fighterComp)
        {
            fighterComp.EquipWeapon(weaponConfig);
            StartCoroutine(respawn(timeToRespawn));
        }

        IEnumerator respawn(float time)
        {
            Show(false);
            yield return new WaitForSeconds(time);
            Show(true);
        }

        void Show(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            GetComponentInChildren<MeshRenderer>().enabled = shouldShow;
        }
        public bool HandleRaycast(PlayerControl playerControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickUp(playerControl.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.PICKUP;
        }
    }
}
