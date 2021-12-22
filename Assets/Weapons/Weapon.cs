using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent hitSound;
        public void OnHit()
        {
            hitSound.Invoke();
        }
    }
}
