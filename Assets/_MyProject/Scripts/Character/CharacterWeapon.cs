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
        private WeaponControl _currentArm;

        private void Awake()
        {
            InitWeapons();
        }

        // Start is called before the first frame update
        void Start()
        {
            _currentArm = _weapons[0];
            _currentArm.Active();
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
            _currentArm.IsTriggered = true;
        }

        public void OnBtnFireReleased()
        {
            _currentArm.IsTriggered = false;
        }

        public void SwitchInventory(int inventorySlot)
        {
            if (_currentArm.WeaponType != _weapons[inventorySlot].WeaponType)
            {
                _currentArm.InActive();
                _currentArm = _weapons[inventorySlot];
                _currentArm.Active();
            }
        }

        public void ReloadWeapon()
        {
            if (_currentArm.CurAmmo < _currentArm.AmmoPerMagazines && !_currentArm.IsReloading)
                _currentArm.Reload();
        }

        public void ToggleScope()
        {
            _currentArm.IsScope = !_currentArm.IsScope;
        }
    }
}

