using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LC.Ultility;
using System;

namespace FPS
{
    public class CharacterControl : MonoBehaviour
    {
        [SerializeField]
        private int _curHealth;
        [SerializeField]
        private int _maxHealth;

        [SerializeField]
        private CharacterMovement _charMovement;
        [SerializeField]
        private CharacterWeapon _charWeapon;
        [SerializeField]
        private InteractingControl _interactingControl;
        [SerializeField]
        private Canvas_Player _canvasPlayer;

        private int _score;

        private bool _isDead;

        public bool IsDead 
        {
            get => _isDead;
            private set
            {
                _isDead = value;
                if (_isDead)
                    this.PostEvent(EventID.PlayerLoose);
            }
        }

        public int CurHealth 
        { 
            get => _curHealth; 
            set 
            {
                _curHealth = Mathf.Clamp(value, 0, _maxHealth);
                IsDead = _curHealth == 0;
                _canvasPlayer.UpdateHealthUI(_curHealth, _maxHealth);
            }
        }

        public int Score 
        { 
            get => _score;
            set
            {
                _score = value;
                _canvasPlayer.UpdateScore(_score);
            }
        }

        public static Vector3 CharacterPosition;

        private void Awake()
        {
            // Set as init...
            Score = 0;
            CurHealth = _maxHealth / 2;
        }

        // Start is called before the first frame update
        void Start()
        {
            this.RegisterListener(EventID.GainScore, Handle_UpdateScore);
            this.RegisterListener(EventID.GetHealth, Handle_UpdateHealth);
            this.RegisterListener(EventID.AttackCharacter, Handle_UpdateHealth);
        }

        private void Handle_UpdateScore(object score)
        {
            Score += (int)score;
        }

        private void Handle_UpdateHealth(object hp)
        {
            CurHealth += (int)hp;
        }

        // Update is called once per frame
        void Update()
        {
            CharacterPosition = transform.position;
        }

        [ContextMenu("TakeDamage")]
        private void TestTakeDamage10()
        {
            CurHealth -= 10;
        }
        [ContextMenu("TestToggleDie")]
        private void TestToggleDie()
        {
            IsDead = !IsDead;
            _charMovement.enabled = false;
            _charWeapon.enabled = false;
            _interactingControl.enabled = false;
            //_canvasPlayer.enabled = false;
        }

        public void TakeDamage(int damage)
        {
            CurHealth -= damage;
        }

        private void OnDestroy()
        {
            EventManager.Instance?.RemoveListener(EventID.GetHealth, Handle_UpdateHealth);
            EventManager.Instance?.RemoveListener(EventID.AttackCharacter, Handle_UpdateHealth);
        }

        [ContextMenu("TestWin")]
        private void TestWin()
        {
            this.PostEvent(EventID.PlayerWin, 10);
        }

        [ContextMenu("TestLoose")]
        private void TestLoose()
        {
            this.PostEvent(EventID.PlayerLoose);
        }
    }
}
