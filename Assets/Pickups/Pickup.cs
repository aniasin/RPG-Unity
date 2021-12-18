using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float timeToRespawn = 5;

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag != "Player") return;
            Fighter fighterComp = collider.GetComponent<Fighter>();
            fighterComp.EquipWeapon(weapon);
            
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
    }
}
