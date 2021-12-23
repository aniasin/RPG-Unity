using RPG.Attributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 50;
        [SerializeField] bool isHomeSeeking = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] UnityEvent launchSound = null;
        [SerializeField] UnityEvent hitSound = null;

        float damage;
        public float Damage { get { return damage; } set { damage = value; } }
        Health targetHealth;
        public Health TargetHealth { get { return targetHealth; } set { targetHealth = value; } }
        GameObject instigator;
        public GameObject Instigator { get { return instigator; } set { instigator = value; } }

        bool hasAimPosition;

        void Start()
        {
            launchSound.Invoke();
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
            Vector3 offset = new Vector3(0, 1, 0);
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
            hitSound.Invoke();
            Destroy(gameObject);
        }

    }
}

