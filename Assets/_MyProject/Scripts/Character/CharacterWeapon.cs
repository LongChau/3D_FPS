using LC.Ultility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class CharacterWeapon : MonoBehaviour
    {
        // Slot 0 is handgun, slot 1 is rifle.
        [SerializeField]
        private WeaponControl[] _weapons;
        private WeaponControl _currentWeapon;

        private void Awake()
        {
            InitWeapons();
        }

        // Start is called before the first frame update
        void Start()
        {
            _currentWeapon = _weapons[0];
            _currentWeapon.Active();
            _weapons[1].InActive();
        }

        private void InitWeapons()
        {
            for (int i = 0; i < _weapons.Length; i++)
            {
                _weapons[i].Init();
            }
        }

        public void OnBtnFirePressed()
        {
            _currentWeapon.IsTriggered = true;
        }

        public void OnBtnFireReleased()
        {
            _currentWeapon.IsTriggered = false;
        }

        public void SwitchInventory(int inventorySlot)
        {
            if (_currentWeapon.WeaponType != _weapons[inventorySlot].WeaponType)
            {
                _currentWeapon.InActive();
                _currentWeapon = _weapons[inventorySlot];
                _currentWeapon.Active();
            }
        }

        public void ReloadWeapon()
        {
            if (_currentWeapon.CurAmmo < _currentWeapon.AmmoPerMagazines && !_currentWeapon.IsReloading)
                _currentWeapon.Reload();
        }

        public void ToggleScope()
        {
            _currentWeapon.IsScope = !_currentWeapon.IsScope;
        }
    }
}

