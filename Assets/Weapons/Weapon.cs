
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : StyledScriptableObject
    {
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Projectile projectile = null;
        [SerializeField] bool isLeftHand = false;
        [SerializeField] AnimatorOverrideController animatorOverride = null;

        [SerializeField] float weaponDamage = 10;
        public float WeaponDamage { get { return weaponDamage; } }
        [SerializeField] float weaponSpeed = 1;
        public float WeaponSpeed { get { return weaponSpeed; } }
        [SerializeField] float weaponRange = 2;
        public float WeaponRange { get { return weaponRange; } }

        GameObject weaponInstance;
        const string weaponName = "weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            Unequip(rightHand, leftHand);
            Transform handTransfor = isLeftHand ? leftHand : rightHand;
            weaponInstance = Instantiate(weaponPrefab, handTransfor);
            weaponInstance.name = weaponName;

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

        public void SpawnProjectile(Health targetHealth, Transform rightHand, Transform leftHand, GameObject instigator)
        {
            Transform handTransform = isLeftHand ? rightHand : leftHand;
            Projectile currentProjectile = Instantiate(projectile, handTransform.position, Quaternion.identity);
            currentProjectile.TargetHealth = targetHealth;
            currentProjectile.Damage = weaponDamage;
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

