using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Statistics;
using System.Collections.Generic;
using RPG.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISavable, IModifierProvider
    {
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";

        LazyValue<Weapon> currentWeapon;

        float timeSinceLastAttack;
        Health target;
        public Health Target { get { return target; } set { target = value; } }

        Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
            currentWeapon = new LazyValue<Weapon>(InitializeDefaultWeapon);
        }

        void Start()
        {
            currentWeapon.ForceInit();
            EquipWeapon(currentWeapon.value);        
        }

        Weapon InitializeDefaultWeapon()
        {
            return defaultWeapon;
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (!target || target.IsDead)   return;

            GetComponent<Mover>().MoveTo(target.transform.position, true);
            if (Vector3.Distance(target.transform.position, transform.position) <= currentWeapon.value.WeaponRange)
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }
        public void EquipWeapon(Weapon weapon)
        {
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
            currentWeapon.value = weapon;
        }

        public void Attack(GameObject enemyTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = enemyTarget.GetComponent<Health>();
        }

        void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= currentWeapon.value.WeaponSpeed)
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
            float damage = GetComponent<BaseStats>().GetDamage();
            target.TakeDamage(damage, gameObject);
        }

        void Shoot()
        {
            float damage = GetComponent<BaseStats>().GetDamage();
            if (!target) return;
            currentWeapon.value.SpawnProjectile(target, rightHandTransform, leftHandTransform, gameObject, damage);
        }

        public void RestoreState(object state)
        {
            defaultWeaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(defaultWeaponName);
            EquipWeapon(weapon);
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public IEnumerable<float> GetAdittiveModifiers(Stats stat)
        {
            if (stat == Stats.DAMAGE)
            {
                yield return currentWeapon.value.WeaponDamage;
            }
        }

        public IEnumerable<float> GetMultiplierModifiers(Stats stat)
        {
            if (stat == Stats.DAMAGE)
            {
                yield return currentWeapon.value.WeaponMultiplier;
            }
        }
    }
}

