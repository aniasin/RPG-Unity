using UnityEngine;

namespace RPG.Combat
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon;

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag != "Player") return;
            collider.GetComponent<Fighter>().EquipWeapon(weapon);
            Destroy(gameObject);
        }

    }
}
