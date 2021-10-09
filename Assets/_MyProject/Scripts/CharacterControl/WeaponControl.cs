using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class WeaponControl : MonoBehaviour
    {
        [SerializeField]
        private CharacterWeapon _charWeapon;
        [SerializeField]
        private Transform _head;
        [SerializeField]
        private Camera _fpsCam;
        [SerializeField]
        private Transform _gunPlacement;
        [SerializeField]
        private Camera _weaponCam;
        [SerializeField]
        private CrossHairUI _crossHair;
        [SerializeField]
        private GameObject _hitEffect;

        [Header("---Gun info---")]
        [SerializeField]
        private EWeaponType _weaponType;
        [SerializeField]
        private int _fireRate;
        [SerializeField]
        private int _curAmmo;
        [SerializeField]
        private int _ammoPerMagazines;
        [SerializeField]
        private int _maxAmmo;
        [SerializeField]
        private int _damage;
        [SerializeField]
        private float _camScopePos;
        [SerializeField]
        private float _camUnScopePos;

        private float _nextShootTime;
        private bool _isScope;

        public bool IsTriggered { get; set; }

        public bool IsScope 
        { 
            get => _isScope; 
            set
            {
                _isScope = value;
                _crossHair.gameObject.SetActive(!_isScope);
                if (_isScope)
                    _weaponCam.transform.DOLocalMoveX(_camScopePos, 0.5f);
                else
                    _weaponCam.transform.DOLocalMoveX(_camUnScopePos, 0.5f);
            }
        }

        public EWeaponType WeaponType => _weaponType;

        // Start is called before the first frame update
        void Start()
        {
            _nextShootTime = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            //Shoot
            if (_weaponType == EWeaponType.AutoRifle)
            {
                if (IsTriggered && Time.time >= _nextShootTime && _curAmmo > 0)
                {
                    _nextShootTime = Time.time + 1 / _fireRate;
                    var ray = new Ray(_fpsCam.transform.position, _fpsCam.transform.forward);
                    RaycastHit hit;
                    bool isHitSomething = Physics.Raycast(ray, out hit, float.PositiveInfinity);
                    Debug.DrawRay(_fpsCam.transform.position, _fpsCam.transform.forward * 10f, Color.blue);
                    if (isHitSomething)
                    {
                        //Debug.Log($"Hit {hit.collider.name}");
                        var shootable = hit.collider.GetComponent<IShootable>();
                        if (shootable != null)
                        {
                            shootable.CurrentHp -= _damage;
                            //shootable.InstantiateEffect(_hitEffect, hit.point, Quaternion.identity, 1.0f);
                        }
                    }

                    // Apply gun recoil
                    transform.DOShakePosition(0.2f, 0.01f);
                    _curAmmo--;
                }
                else
                {
                    // Reset gun position.
                    transform.localPosition = Vector3.zero;
                    transform.DOLocalMove(Vector3.zero, 0.2f);
                    //transform.DOMove(_gunStartPosition, 0.1f);
                }
            }
            else
            {
                if (IsTriggered && _curAmmo > 0)
                {
                    var ray = new Ray(_fpsCam.transform.position, _fpsCam.transform.forward);
                    RaycastHit hit;
                    bool isHitSomething = Physics.Raycast(ray, out hit, float.PositiveInfinity);
                    Debug.DrawRay(_fpsCam.transform.position, _fpsCam.transform.forward * 10f, Color.blue);
                    if (isHitSomething)
                    {
                        Debug.Log($"Hit {hit.collider.name}");
                        _curAmmo--;
                    }

                    // Apply gun recoil
                    transform.DOShakePosition(0.2f, 0.01f, 1, 0);
                    IsTriggered = false;
                }
                else
                {
                    // Reset gun position.
                    //transform.localPosition = Vector3.zero;
                    transform.DOLocalMove(Vector3.zero, 0.2f);
                }
            }
        }

        public void Reload()
        {
            if (_ammoPerMagazines < _maxAmmo)
            {
                _maxAmmo -= _ammoPerMagazines - _curAmmo;
                _curAmmo = _ammoPerMagazines;
            }
            else
            {
                _curAmmo = _maxAmmo;
            }
        }

        public void Active()
        {
            gameObject.SetActive(true);
            IsScope = false;
        }

        public void InActive()
        {
            gameObject.SetActive(false);
            IsTriggered = false;
        }
    }
}
