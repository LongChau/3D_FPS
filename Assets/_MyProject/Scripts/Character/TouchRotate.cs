using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FPS
{
    public class TouchRotate : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField]
        private FloatingJoystick _joystick;

        private Vector2 _touchDown;
        private Vector2 _touchUp;

        public float TouchLength { get; private set; }

        public Vector2 RawDirection { get; private set; }
        public Vector2 NormalizedDirection { get; private set; }
        public bool IsTouchDown { get; private set; }
        public bool IsTouchUp { get; private set; }
        public bool IsTouchDrag { get; private set; }
        public FloatingJoystick Joystick => _joystick;

        private void Awake()
        {
#if UNITY_EDITOR
            _joystick.gameObject.SetActive(true);
#else
            _joystick.gameObject.SetActive(false);
#endif
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log($"Touch OnDrag: {eventData.position}");
            IsTouchDrag = true;

            _touchUp = eventData.position;
            RawDirection = _touchUp - _touchDown;
            //NormalizedDirection = RawDirection.normalized;
            float x = _joystick.Horizontal;
            float y = _joystick.Vertical;
            NormalizedDirection = new Vector2(x, y);
            TouchLength = RawDirection.magnitude;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log($"Touch down: {eventData.position}");
            IsTouchDown = true;
            IsTouchDrag = false;
            IsTouchUp = false;

            _touchDown = eventData.position;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log($"Touch exit: {eventData.position}");
            IsTouchDown = false;
            IsTouchDrag = false;
            IsTouchUp = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //Debug.Log($"Touch up: {eventData.position}");
            IsTouchDown = false;
            IsTouchDrag = false;
            IsTouchUp = true;

            //_touchUp = eventData.position;
            //var touchDirection = _touchUp - _touchDown;
            //Direction = touchDirection.normalized;
            //Debug.Log($"Direction: {Direction}");
        }

        // Start is called before the first frame update
        void Start()
        {

        }
    }
}
