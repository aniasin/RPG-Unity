using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 50;

        float damage;
        public float Damage { get { return damage; } set { damage = value; } }
        Health targetHealth;
        public Health TargetHealth { get { return targetHealth; } set { targetHealth = value; } }

        bool hasAimPosition;

        void Update()
        {
            if (targetHealth == null) return;

            if (!hasAimPosition)
            {
                transform.LookAt(GetAimPosition());
            }            
            float value = Time.deltaTime * speed;
            transform.Translate(Vector3.forward * value);
        }

         Vector3 GetAimPosition()
        {
            hasAimPosition = true;
            return targetHealth.transform.position + Vector3.up;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Health>() == targetHealth)
            {
                targetHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

    }
}

