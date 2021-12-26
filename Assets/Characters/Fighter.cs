using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Statistics;
using System.Collections.Generic;
using RPG.Utils;
using RPG.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISavable, IModifierProvider
    {
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] EquipableItem defaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";

        LazyValue<EquipableItem> currentWeaponConfig;

        float timeSinceLastAttack;
        Health target;
        public Health Target { get { return target; } set { target = value; } }

        Animator animator;
        Equipment equipment;

        void Awake()
        {
            equipment = GetComponent<Equipment>();
            animator = GetComponent<Animator>();
            currentWeaponConfig = new LazyValue<EquipableItem>(InitializeDefaultWeapon);
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }            
        }
        void Start()
        {
            currentWeaponConfig.ForceInit();
            EquipWeapon(currentWeaponConfig.value);        
        }

        EquipableItem InitializeDefaultWeapon()
        {
            return defaultWeapon;
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (!target || target.IsDead)   return;

            GetComponent<Mover>().MoveTo(target.transform.position, true);
            if (InRange(target.transform.position))
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        public bool InRange(Vector3 target)
        {
            return Vector3.Distance(target, transform.position) 
                <= currentWeaponConfig.value.WeaponRange;
        }
        public void EquipWeapon(EquipableItem weapon)
        {
            if (weapon.WeaponDamage < 0)
            {
                GetComponent<Health>().RestoreFullHealth();
                return;
            }
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
            currentWeaponConfig.value = weapon;
        }
         void UpdateWeapon()
        {
            EquipableItem item = equipment.GetItemInSlot(EquipLocation.Weapon);
            if (item == null)
            {
                EquipWeapon(defaultWeapon);
                return;
            }
            EquipWeapon(item);
        }

        public void Attack(GameObject enemyTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = enemyTarget.GetComponent<Health>();
        }

        void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= currentWeaponConfig.value.WeaponSpeed)
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
            currentWeaponConfig.value.WeaponInstance.OnHit();
        }

        void Shoot()
        {
            float damage = GetComponent<BaseStats>().GetDamage();
            currentWeaponConfig.value.SpawnProjectile(target, rightHandTransform, leftHandTransform, gameObject, damage);
        }

        public void RestoreState(object state)
        {
            defaultWeaponName = (string)state;
            EquipableItem weapon = Resources.Load<EquipableItem>(defaultWeaponName);
            EquipWeapon(weapon);
        }

        public object CaptureState()
        {
            return currentWeaponConfig.value.name;
        }

        public IEnumerable<float> GetAdittiveModifiers(Stats stat)
        {
            if (stat == Stats.DAMAGE)
            {
                yield return currentWeaponConfig.value.WeaponDamage;
            }
        }

        public IEnumerable<float> GetMultiplierModifiers(Stats stat)
        {
            if (stat == Stats.DAMAGE)
            {
                yield return currentWeaponConfig.value.WeaponMultiplier;
            }
        }
    }
}

