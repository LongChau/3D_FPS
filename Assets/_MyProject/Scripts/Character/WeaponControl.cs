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
        private Animator _anim;
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
        [SerializeField]
        private Transform _muzzle;

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
        private Vector3 _camScopePos;
        [SerializeField]
        private Vector3 _camUnScopePos;
        [SerializeField]
        private float _reloadTime;
        [SerializeField]
        private float _takeOutTime;

        [Header("Audio")]
        [SerializeField]
        private AudioSource _reloadAudio;
        [SerializeField]
        private AudioSource _fireAudio;
        [SerializeField]
        private AudioSource _outOfAmmoAudio;

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
                    _weaponCam.transform.DOLocalMove(_camScopePos, 0.5f);
                else
                    _weaponCam.transform.DOLocalMove(_camUnScopePos, 0.5f);
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

        public int AmmoPerMagazines => _ammoPerMagazines;

        public bool IsReloading { get; private set; }
        public bool IsTakingOut { get; private set; }

        private void OnValidate()
        {
            _anim = GetComponent<Animator>();
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
            if (IsReloading || IsTakingOut) return;

            if (IsTriggered && CurAmmo == 0)
                _outOfAmmoAudio.Play();

            //Shoot
            if (_weaponType == EWeaponType.AutoRifle)
            {
                if (IsTriggered && Time.time >= _nextShootTime && CurAmmo > 0)
                {
                    _nextShootTime = Time.time + 1.0f / _fireRate;

                    var recoidData = new RecoidData
                    {
                        duration = 0.5f,
                        strength = 0.05f,
                        vibrato = 1,
                        randomness = 20
                    };
                    Fire(recoidData);
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
                    var recoidData = new RecoidData
                    {
                        duration = 0.2f,
                        strength = 0.01f,
                        vibrato = 5,
                        randomness = 30
                    };
                    Fire(recoidData);
                    IsTriggered = false;
                }
                else
                {
                    // Reset gun position.
                    transform.DOLocalMove(Vector3.zero, 0.2f);
                }
            }
        }

        private void Fire(RecoidData recoidData)
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
                    damageable.InstantiateEffect(_hitEffect, hit.point, Quaternion.identity, 5.0f);
                }
            }

            // Apply gun recoil
            transform.DOShakePosition(recoidData.duration, recoidData.strength, recoidData.vibrato, recoidData.randomness);
            CurAmmo--;
            _spawnBulletPoint.SpawnEnity();
            _muzzle.gameObject.SetActive(true);
            DOVirtual.DelayedCall(0.1f, () => _muzzle.gameObject.SetActive(false));

            _fireAudio.Play();
        }

        public void Reload()
        {
            if (AmmoLeft > 0)
            {
                _anim.SetTrigger("TriggerReload");
                _reloadAudio.Play();
                IsReloading = true;
                DOVirtual.DelayedCall(_reloadTime, () =>
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

                    _anim.SetTrigger("TriggerIdle");
                    IsReloading = false;
                });
            }
        }

        public void Active()
        {
            gameObject.SetActive(true);
            IsScope = false;
            TakingOut();
        }

        private void TakingOut()
        {
            _anim.SetTrigger("TriggerTakeOut");
            IsTakingOut = true;
            DOVirtual.DelayedCall(_takeOutTime, () =>
            {
                _anim.SetTrigger("TriggerIdle");
                IsTakingOut = false;
            });
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

        private struct RecoidData
        {
            public float duration;
            public float strength;
            public int vibrato;
            public float randomness;
        }
    }
}
