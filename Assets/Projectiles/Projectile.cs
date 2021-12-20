using RPG.Attributes;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 50;
        [SerializeField] bool isHomeSeeking = false;
        [SerializeField] GameObject hitEffect = null;

        float damage;
        public float Damage { get { return damage; } set { damage = value; } }
        Health targetHealth;
        public Health TargetHealth { get { return targetHealth; } set { targetHealth = value; } }
        GameObject instigator;
        public GameObject Instigator { get { return instigator; } set { instigator = value; } }

        bool hasAimPosition;

        void Start()
        {
            Destroy(gameObject, 10);
        }

        void Update()
        {
            if (targetHealth == null) return;

            if (!hasAimPosition || isHomeSeeking)
            {
                transform.LookAt(GetAimPosition());
            }            
            float value = Time.deltaTime * speed;
            transform.Translate(Vector3.forward * value);
        }

         Vector3 GetAimPosition()
        {
            hasAimPosition = true;
            float velocityX = 0;
            float velocityZ = 0;
            if (!isHomeSeeking)
            {
                velocityX = Mathf.Clamp01(targetHealth.GetComponent<NavMeshAgent>().velocity.x);
                velocityZ = Mathf.Clamp01(targetHealth.GetComponent<NavMeshAgent>().velocity.z);
            }
            Vector3 offset = new Vector3(velocityX, 1, velocityZ);
            return targetHealth.transform.position + offset;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Health>() == targetHealth)
            {
                targetHealth.TakeDamage(damage, instigator);
            }
            if (hitEffect)
            {
                GameObject impact =Instantiate(hitEffect, other.transform.position, other.transform.rotation);
                Destroy(impact, 5);
            }
            Destroy(gameObject);
        }

    }
}

