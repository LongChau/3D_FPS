using LC.Ultility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    /// <summary>
    /// Control movement and head rotation.
    /// </summary>
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField]
        private FixedJoystick _joystick;
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private Transform _head;
        [SerializeField]
        private Camera _gunCam;

        [SerializeField]
        private CharacterController _charCtrl;
        [SerializeField]
        private float _movementSpeed;
        [SerializeField]
        private float _jumpHeight;
        [SerializeField]
        private float _gravityValue = 9.81f;

        [SerializeField]
        private Transform _groundCheck;
        [SerializeField]
        private float _checkGroundLength = 1f;
        [SerializeField]
        private LayerMask _groundMask;

        private float _dirY;
        private bool _isJumped;
        private bool _isGrounded;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            float x = _joystick.Horizontal;
            float z = _joystick.Vertical;

            var move = transform.forward * z + transform.right * x;
            //Debug.Log($"Horizontal: {x} Vertical: {z}");

            // Check if player stands on ground.
            var ray = new Ray(_groundCheck.position, Vector3.down);
            var hitInfo = new RaycastHit();
            _isGrounded = Physics.Raycast(ray, out hitInfo, _checkGroundLength, _groundMask);

            // Apply jump action.
            if (_isJumped && _isGrounded)
            {
                _dirY = _jumpHeight;
                _isJumped = false;
            }
            // Apply gravity here.
            _dirY -= _gravityValue * Time.fixedDeltaTime;
            move.y = _dirY;

            _charCtrl.Move(move * _movementSpeed * Time.fixedDeltaTime);
        }

        public void Jump()
        {
            Log.Info("Jump()");
            _isJumped = true;
        }
    }
}
