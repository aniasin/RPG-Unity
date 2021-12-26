using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

namespace RPG.Inventories
{
    /// <summary>
    /// An inventory item that can be equipped to the player. Weapons could be a
    /// subclass of this.
    /// </summary>
    [CreateAssetMenu(menuName = ("Pickups/New Pickup"))]
    public class EquipableItem : InventoryItem
    {
        // CONFIG DATA
        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.Weapon;

        [SerializeField] Weapon weapon = null;
        [SerializeField] Projectile projectile = null;
        [SerializeField] bool isLeftHand = false;
        [SerializeField] bool isRanged = false;
        public bool IsRanged { get { return isRanged; } }
        [SerializeField] AnimatorOverrideController animatorOverride = null;

        [SerializeField] float weaponDamage = 10;
        public float WeaponDamage { get { return weaponDamage; } }

        [SerializeField] float weaponMultiplier = 10;
        public float WeaponMultiplier { get { return weaponMultiplier; } }

        [SerializeField] float weaponSpeed = 1;
        public float WeaponSpeed { get { return weaponSpeed; } }

        [SerializeField] float weaponRange = 2;
        public float WeaponRange { get { return weaponRange; } }

        Weapon weaponInstance;
        public Weapon WeaponInstance { get { return weaponInstance; } }
        const string weaponName = "weapon";

        // PUBLIC
        public EquipLocation GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            Unequip(rightHand, leftHand);
            Transform handTransfor = isLeftHand ? leftHand : rightHand;
            weaponInstance = Instantiate(weapon, handTransfor);
            weaponInstance.gameObject.name = weaponName;

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animator)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        public void SpawnProjectile(Health targetHealth, Transform rightHand, Transform leftHand,
                                    GameObject instigator, float damage)
        {
            Transform handTransform = isLeftHand ? rightHand : leftHand;
            Projectile currentProjectile = Instantiate(projectile, handTransform.position, Quaternion.identity);
            currentProjectile.TargetHealth = targetHealth;
            currentProjectile.Damage = damage;
            currentProjectile.Instigator = instigator;
        }

        void Unequip(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (!oldWeapon)
            {
                oldWeapon = leftHand.Find(weaponName);
                if (!oldWeapon) return;
            }
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
    }
}
