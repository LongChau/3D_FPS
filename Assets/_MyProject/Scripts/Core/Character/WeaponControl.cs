using DG.Tweening;
using FPS.AssetData;
using LC.Ultility;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;

namespace FPS
{
    public partial class WeaponControl : MonoBehaviour
    {
        [SerializeField, TabGroup("HierachyReference")]
        private Animator _anim;
        [SerializeField, TabGroup("HierachyReference")]
        private CharacterWeapon _charWeapon;
        [SerializeField, TabGroup("HierachyReference")]
        private Transform _head;
        [SerializeField, TabGroup("HierachyReference")]
        private Camera _fpsCam;
        [SerializeField, TabGroup("HierachyReference")]
        private Transform _gunPlacement;
        [SerializeField, TabGroup("HierachyReference")]
        private Camera _weaponCam;

        [Header("---FX---")]
        [SerializeField, TabGroup("FX")]
        private GameObject _hitEffect;
        [SerializeField, TabGroup("FX")]
        private GameObject _bulletHole;

        [Header("---UI---")]
        [SerializeField, TabGroup("UI")]
        private TextMeshProUGUI _txtAmmo;
        [SerializeField, TabGroup("UI")]
        private TextMeshProUGUI _txtAmmoLeft;
        [SerializeField, TabGroup("UI")]
        private CrossHairUI _crossHair;

        [Header("---Gun info---")]
        [SerializeField, TabGroup("GunInfo")]
        private Vector3 _camScopePos;
        [SerializeField, TabGroup("GunInfo")]
        private Vector3 _camUnScopePos;
        [SerializeField, TabGroup("GunInfo")]
        private int _ammoLeft;
        [SerializeField, TabGroup("GunInfo")]
        private SpawnBulletPoint _spawnBulletPoint;
        [SerializeField, TabGroup("GunInfo")]
        private GameObject _muzzle;
        [SerializeField, InlineEditor, TabGroup("GunInfo")]
        private Weapon _weaponData;

        [Header("---Audio---")]
        [SerializeField, TabGroup("Audio")]
        private AudioSource _reloadAudio;
        [SerializeField, TabGroup("Audio")]
        private AudioSource _fireAudio;
        [SerializeField, TabGroup("Audio")]
        private AudioSource _outOfAmmoAudio;

        private int _curAmmo;
        private float _nextShootTime;
        private bool _isScope;

        public bool IsTriggered { get; set; }

        [ReadOnly, ShowInInspector, TabGroup("Debug")]
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

        public EWeaponType WeaponType => _weaponData.WeaponType;

        [ReadOnly, ShowInInspector, TabGroup("Debug")]
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

        public int AmmoPerMagazines => _weaponData.AmmoPerMagazines;

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
            CurAmmo = _weaponData.AmmoPerMagazines;
            AmmoLeft = _weaponData.MaxAmmo;
        }

        private void Handle_GetAmmo(object obj)
        {
            int addAmmo = (int)obj;
            AmmoLeft = Mathf.Clamp(AmmoLeft + addAmmo, 0, _weaponData.MaxAmmo);
        }

        // Update is called once per frame
        void Update()
        {
            if (IsReloading || IsTakingOut) return;

            if (IsTriggered && CurAmmo == 0)
                _outOfAmmoAudio.Play();

            _muzzle.SetActive(false);

            //m_Accuracy = Mathf.Clamp(Mathf.MoveTowards(m_Accuracy, GetCurrentAccuracy(), Time.deltaTime *
            //                        (m_IsShooting > 0 ? m_GunData.DecreaseRateByShooting : m_GunData.DecreaseRateByWalking)),
            //                        m_GunData.BaseAccuracy, m_GunData.AIMAccuracy);
            _crossHair.Move(0.8f);
            //Shoot
            if (_weaponData.WeaponType == EWeaponType.AutoRifle)
            {
                if (IsTriggered && Time.time >= _nextShootTime && CurAmmo > 0)
                {
                    _nextShootTime = Time.time + 1.0f / _weaponData.FireRate;

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
                    if (transform.localPosition != Vector3.zero)
                    {
                        ResetGunPosition();
                    }
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
                    if (transform.localPosition != Vector3.zero)
                    {
                        ResetGunPosition();
                    }
                }
            }
        }

        private void ResetGunPosition()
        {
            // Reset gun position.
            //transform.position = Vector3.zero;
            //_fpsCam.transform.position = Vector3.zero;
            transform.DOLocalMove(Vector3.zero, 0.2f);
            _fpsCam.transform.DOLocalMove(Vector3.zero, 0.2f);
        }

        RaycastHit[] _hits = new RaycastHit[1];
        RaycastHit _hit;
        Vector3 _shakeCam;
        Ray _ray;
        bool _isHitSomething;
        private void Fire(RecoidData recoidData)
        {
            _shakeCam = new Vector3(UnityEngine.Random.Range(0.05f, 0.2f), UnityEngine.Random.Range(0.05f, 0.2f), 0f);
            _ray = new Ray(_fpsCam.transform.position + _shakeCam, _fpsCam.transform.forward);
            _isHitSomething = Physics.SphereCast(_ray, 0.1f, out _hit, float.PositiveInfinity);
            //int hitIndex = Physics.SphereCastNonAlloc(ray, 0.1f, _hits, _fpsCam.farClipPlane);
            //bool isHitSomething = _hits[hitIndex - 1].collider != null;
            //Debug.DrawRay(_fpsCam.transform.position, _fpsCam.transform.forward * 10f, Color.blue);
            if (_isHitSomething)
            {
                int id = _hit.collider.gameObject.GetInstanceID();
                if (DamagableControl.Instance.DictDamageables.ContainsKey(id))
                {
                    DamagableControl.Instance.DictDamageables[id].TakeDamage(_weaponData.Damage);
                    DamagableControl.Instance.DictDamageables[id].InstantiateEffect(_hitEffect, _hit.point, Quaternion.identity, 5.0f);
                }
                else
                {
                    // Hit environment
                    var hole = Instantiate(_bulletHole, _hit.point, Quaternion.identity);
                    Destroy(hole, 1f);
                }
            }

            // Apply gun recoil
            transform.DOShakePosition(recoidData.duration, recoidData.strength, recoidData.vibrato, recoidData.randomness);
            CurAmmo--;
            //_spawnBulletPoint.SpawnEnity();
            _muzzle.SetActive(true);
            //_crossHair.PlayCrossHairEffect(10f, 0.2f, 1f, 0.5f);
            _crossHair.Move(null);
            _fireAudio.Play();
        }

        public void Reload()
        {
            if (AmmoLeft > 0)
            {
                _anim.SetTrigger("TriggerReload");
                _reloadAudio.Play();
                IsReloading = true;
                DOVirtual.DelayedCall(_weaponData.ReloadTime, () =>
                {
                    var ammoNeeded = _weaponData.AmmoPerMagazines - CurAmmo;
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
            DOVirtual.DelayedCall(_weaponData.TakeOutTime, () =>
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
