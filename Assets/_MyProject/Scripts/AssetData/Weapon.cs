using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.AssetData
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "FPS/AssetData/Weapon")]
    public class Weapon : ScriptableObject
    {
        [Header("---Gun info---")]
        [SerializeField]
        private EWeaponType _weaponType;
        [SerializeField]
        private int _fireRate;
        [SerializeField]
        private int _ammoPerMagazines;
        [SerializeField]
        private int _maxAmmo;
        [SerializeField]
        private int _damage;
        [SerializeField]
        private float _reloadTime;
        [SerializeField]
        private float _takeOutTime;

        public EWeaponType WeaponType => _weaponType;
        public int FireRate => _fireRate;
        public int AmmoPerMagazines => _ammoPerMagazines;
        public int MaxAmmo => _maxAmmo;
        public int Damage => _damage;
        public float ReloadTime => _reloadTime;
        public float TakeOutTime => _takeOutTime;
    }
}
