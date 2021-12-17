using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] Transform rightHandTransform;
        [SerializeField] Transform leftHandTransform;
        [SerializeField] Weapon defaultWeapon;
        Weapon currentWeapon;

        float timeSinceLastAttack;
        Health target;
        public Health Target { get { return target; } set { target = value; } }
        Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            EquipWeapon(defaultWeapon);
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (!target || target.IsDead)  return;
           GetComponent<Mover>().MoveTo(target.transform.position, true);
           if (Vector3.Distance(target.transform.position, transform.position) <= currentWeapon.WeaponRange)
           {
                GetComponent<Mover>().Cancel();
                AttackBehavior();                
           }
        }
        public void EquipWeapon(Weapon weapon)
        {
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
            currentWeapon = weapon;
        }

        public void Attack(GameObject enemyTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = enemyTarget.GetComponent<Health>();
        }

        void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= currentWeapon.WeaponSpeed)
            {                
                //Triggers Hit() event.
                timeSinceLastAttack = 0;
                TriggerAttack();
            }
        }

        void TriggerAttack()
        {
            animator.ResetTrigger("stopAttacking");
            animator.SetTrigger("IsAttacking");
        }
        public void Cancel()
        {
            StopAttack();
            GetComponent<Mover>().Cancel();
            target = null;
        }
        void StopAttack()
        {
            animator.ResetTrigger("IsAttacking");
            animator.SetTrigger("stopAttacking");
        }

        //Animation Events
        void Hit()
        {
            if (!target) return;
            target.TakeDamage(currentWeapon.WeaponDamage);
        }

        void Shoot()
        {
            if (!target) return;
            currentWeapon.SpawnProjectile(target, rightHandTransform, leftHandTransform);
        }



    }
}

