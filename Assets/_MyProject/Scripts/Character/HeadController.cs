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

        private Vector2 _lookInput;

        // Start is called before the first frame update
        void Start()
        {
#if UNITY_EDITOR
            _lookSpeed = 100f;
#endif
        }

        private void Update()
        {
#if UNITY_EDITOR
            LookRotation();
#else
            if (!_touchRotate.IsTouchDrag) return;
            GetTouchInput();
            LookAround();
#endif
        }

        private void LookAround()
        {
            //Debug.Log("LookAround()");
            _xAxisRotation = Mathf.Clamp(_xAxisRotation - _lookInput.y, -90f, 90f);
            transform.localRotation = Quaternion.Euler(_xAxisRotation, 0f, 0f);
            // Should also rotate the character.
            _charCtrl.transform.Rotate(Vector3.up, _lookInput.x);
        }

        private void GetTouchInput()
        {
            //Debug.Log("GetTouchInput()");
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);
                switch (t.phase)
                {
                    case TouchPhase.Began:
                        break;
                    case TouchPhase.Moved:
                        _lookInput = t.deltaPosition * _lookSpeed * Time.deltaTime;
                        break;
                    case TouchPhase.Stationary:
                        _lookInput = Vector2.zero;
                        break;
                    case TouchPhase.Ended:
                        break;
                    case TouchPhase.Canceled:
                        break;
                    default:
                        break;
                }
            }
        }

        private void LookRotation()
        {
            var lookSensitiveX = _touchRotate.Joystick.Horizontal * _lookSpeed * Time.deltaTime;
            var lookSensitiveY = _touchRotate.Joystick.Vertical * _lookSpeed * Time.deltaTime;

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
