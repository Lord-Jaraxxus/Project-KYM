using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public partial class CharacterBase
    {
        [Header("Weapon Socket")]
        [SerializeField] private Transform holsterSocketTransform;
        [SerializeField] private Transform rightHandSocketTransform;

        public bool IsProgressSwapWeapon => isProgressEquip || isProgressHolster;

        private bool isProgressEquip = false;
        private bool isProgressHolster = false;

        public void SetEquipProgressComplete() => isProgressEquip = false;
        public void SetHolsterProgressComplete() => isProgressHolster = false;

        public void TryEquipToPrimaryWeapon() 
        {
            if (IsProgressSwapWeapon) return;
            if (currentWeapon == primaryWeapon) return;

            EquipWeapon(primaryWeapon);
        }
       public void TryEquipToSecondaryWeapon() 
        {
            if (IsProgressSwapWeapon) return;
            if (currentWeapon == secondaryWeapon) return;

            EquipWeapon(secondaryWeapon);
        }

        private void EquipWeapon(WeaponBase target) 
        {
            this.targetWeapon = target;

            if (this.currentWeapon != null)
            {
                HolsterWeapon();
            }
            else 
            {
                isProgressEquip = true;
                this.animator.SetTrigger("Equip Trigger");
            }

            // TDOD : Equip Animation 25% => Current Weapon = New Weapon
            // => Currnent Weapon 의 부모를 손으로 붙인다.
        }
        private void HolsterWeapon() 
        {
            isProgressHolster = true;
            animator.SetTrigger("Holster Trigger");

            // TDOD : Holster Animation 60% => Current Weapon = null
        }
        private void OnEquipCompleted() 
        {
            currentWeapon = targetWeapon;
            currentWeapon.transform.SetParent(rightHandSocketTransform);

            Vector3 offsetPosition = Vector3.zero;
            Quaternion offsetRotation = Quaternion.identity;
            offsetPosition = CurrentWeapon.WeaponDataSO.InitPosition;
            offsetRotation = Quaternion.Euler(CurrentWeapon.WeaponDataSO.InitRotation);

            currentWeapon.transform.SetLocalPositionAndRotation(offsetPosition, offsetRotation);
        }

        private void OnHolsterCompleted() 
        {
            currentWeapon.transform.SetParent(holsterSocketTransform);
            currentWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            currentWeapon = null;

            if (targetWeapon != null) 
            {
                EquipWeapon(targetWeapon);
            }

            OnWeaponSwaped?.Invoke(targetWeapon.CurAmmo, targetWeapon.MaxAmmo, targetWeapon.ReserveAmmo); // 무기 교체 완료 이벤트 호출
        }
    }
}
