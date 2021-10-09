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

        public bool IsDead { get; private set; }

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

        // Start is called before the first frame update
        void Start()
        {
            // Set as init...
            CurHealth = _maxHealth / 2;

            
            this.RegisterListener(EventID.GetHealth, Handle_GetHealth);
        }

        private void Handle_GetHealth(object hp)
        {
            CurHealth += (int)hp;
        }



        // Update is called once per frame
        void Update()
        {

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
    }
}
