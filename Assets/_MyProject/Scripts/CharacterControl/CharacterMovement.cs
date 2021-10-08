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

            _charCtrl.Move(move * _movementSpeed * Time.fixedDeltaTime);
        }
    }
}
