using DG.Tweening;
using LC.Ultility;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField]
        private SpawnBulletPoint _spawnBulletPoint;

        [Header("---UI---")]
        [SerializeField]
        private TextMeshProUGUI _txtAmmo;
        [SerializeField]
        private TextMeshProUGUI _txtAmmoLeft;

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
        private int _ammoLeft;
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

        public int CurAmmo 
        { 
            get => _curAmmo;
            set
            {
                _curAmmo = value;
                _txtAmmo.SetText(_curAmmo.ToString());
            }
        }

        public int AmmoLeft 
        { 
            get => _ammoLeft;
            set
            {
                _ammoLeft = value;
                _txtAmmoLeft.SetText(_ammoLeft.ToString());
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            this.RegisterListener(EventID.GetAmmo, Handle_GetAmmo);
        }

        public void Init()
        {
            _nextShootTime = 0f;
            CurAmmo = _ammoPerMagazines;
            AmmoLeft = _maxAmmo;
        }

        private void Handle_GetAmmo(object obj)
        {
            int addAmmo = (int)obj;
            AmmoLeft = Mathf.Clamp(AmmoLeft + addAmmo, 0, _maxAmmo);
        }

        // Update is called once per frame
        void Update()
        {
            //Shoot
            if (_weaponType == EWeaponType.AutoRifle)
            {
                if (IsTriggered && Time.time >= _nextShootTime && CurAmmo > 0)
                {
                    _nextShootTime = Time.time + 1.0f / _fireRate;
                    var ray = new Ray(_fpsCam.transform.position, _fpsCam.transform.forward);
                    RaycastHit hit;
                    bool isHitSomething = Physics.Raycast(ray, out hit, float.PositiveInfinity);
                    Debug.DrawRay(_fpsCam.transform.position, _fpsCam.transform.forward * 10f, Color.blue);
                    if (isHitSomething)
                    {
                        //Debug.Log($"Hit {hit.collider.name}");
                        var damageable = hit.collider.GetComponent<IDamageable>();
                        if (damageable != null)
                        {
                            //damageable.CurrentHp -= _damage;
                            damageable.TakeDamage(_damage);
                            //damageable.InstantiateEffect(_hitEffect, hit.point, Quaternion.identity, 1.0f);
                        }
                    }

                    // Apply gun recoil
                    transform.DOShakePosition(0.5f, 0.05f, 1, 20);
                    CurAmmo--;
                    _spawnBulletPoint.SpawnEnity();
                }
                else
                {
                    // Reset gun position.
                    transform.DOLocalMove(Vector3.zero, 0.5f);
                }
            }
            else
            {
                if (IsTriggered && CurAmmo > 0)
                {
                    var ray = new Ray(_fpsCam.transform.position, _fpsCam.transform.forward);
                    RaycastHit hit;
                    bool isHitSomething = Physics.Raycast(ray, out hit, float.PositiveInfinity);
                    Debug.DrawRay(_fpsCam.transform.position, _fpsCam.transform.forward * 10f, Color.blue);
                    if (isHitSomething)
                    {
                        Debug.Log($"Hit {hit.collider.name}");
                        var damageable = hit.collider.GetComponent<IDamageable>();
                        if (damageable != null)
                        {
                            damageable.TakeDamage(_damage);
                        }
                    }

                    // Apply gun recoil
                    transform.DOShakePosition(0.2f, 0.01f, 5, 30);
                    CurAmmo--;
                    IsTriggered = false;
                    _spawnBulletPoint.SpawnEnity();
                }
                else
                {
                    // Reset gun position.
                    transform.DOLocalMove(Vector3.zero, 0.2f);
                }
            }
        }

        public void Reload()
        {
            if (AmmoLeft > 0)
            {
                var ammoNeeded = _ammoPerMagazines - CurAmmo;
                if (ammoNeeded <= AmmoLeft)
                {
                    CurAmmo += ammoNeeded;
                    AmmoLeft -= ammoNeeded;
                }
                else if (ammoNeeded > AmmoLeft)
                {
                    ammoNeeded = AmmoLeft;
                    CurAmmo += ammoNeeded;
                    AmmoLeft = 0;
                }
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

        private void OnDestroy()
        {
            DOTween.Clear();
            EventManager.Instance?.RemoveListener(EventID.GetAmmo, Handle_GetAmmo);
        }
    }
}
