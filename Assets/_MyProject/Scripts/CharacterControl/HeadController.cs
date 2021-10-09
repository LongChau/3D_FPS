using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FPS
{
    public class HeadController : MonoBehaviour
    {
        [SerializeField]
        private float _lookSpeed;
        [SerializeField]
        private CharacterController _charCtrl;
        private float _xAxisRotation = 0f;

        [SerializeField]
        private TouchRotate _touchRotate;
        [SerializeField]
        private float _touchLengthThreshold;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {
            LookRotation();
        }

        private void LookRotation()
        {
            if (_touchRotate.IsTouchDrag && _touchRotate.TouchLength >= _touchLengthThreshold)
            {
                // Calculate the camera sensitive in both horizontal, vertical.
                var lookSensitiveX = _touchRotate.NormalizedDirection.x * _lookSpeed * Time.deltaTime;
                var lookSensitiveY = _touchRotate.NormalizedDirection.y * _lookSpeed * Time.deltaTime;

                _xAxisRotation -= lookSensitiveY;
                // Lock the rotate -90 -> 90
                _xAxisRotation = Mathf.Clamp(_xAxisRotation, -90f, 90f);

                // Apply rotation to head.
                transform.localRotation = Quaternion.Euler(_xAxisRotation, 0f, 0f);
                // Should also rotate the character.
                _charCtrl.transform.Rotate(Vector3.up, lookSensitiveX);
            }
        }
    }
}
