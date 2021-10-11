using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace FPS
{
    public class CrossHairUI : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _upCross;
        [SerializeField]
        private RectTransform _downCross;
        [SerializeField]
        private RectTransform _leftCross;
        [SerializeField]
        private RectTransform _rightCross;

        private Vector2 _startUp;
        private Vector2 _startDown;
        private Vector2 _startLeft;
        private Vector2 _startRight;

        //private Sequence _seq;

        // Start is called before the first frame update
        void Start()
        {
            _startUp = _upCross.anchoredPosition;
            _startDown = _downCross.anchoredPosition;
            _startLeft = _leftCross.anchoredPosition;
            _startRight = _rightCross.anchoredPosition;

            //_seq = DOTween.Sequence();
            //_seq.Append(_upCross.DOPunchAnchorPos(Vector2.up * punchForce, duration))
            //    .Append(_downCross.DOPunchAnchorPos(Vector2.down * punchForce, duration))
            //    .Append(_leftCross.DOPunchAnchorPos(Vector2.left * punchForce, duration))
            //    .Append(_rightCross.DOPunchAnchorPos(Vector2.right * punchForce, duration))
            //    .SetDelay(resetDuration)
            //    .Append(_upCross.DOAnchorPos(_startUp, moveBackDuration).SetDelay(moveBackDuration))
            //    .Append(_downCross.DOAnchorPos(_startDown, moveBackDuration).SetDelay(moveBackDuration))
            //    .Append(_leftCross.DOAnchorPos(_startLeft, moveBackDuration).SetDelay(moveBackDuration))
            //    .Append(_rightCross.DOAnchorPos(_startRight, moveBackDuration).SetDelay(moveBackDuration));
        }

        public void PlayCrossHairEffect(float punchForce, float duration, float resetDuration, float moveBackDuration)
        {
            _upCross.DOPunchAnchorPos(Vector2.up * punchForce, duration);
            _downCross.DOPunchAnchorPos(Vector2.down * punchForce, duration);
            _leftCross.DOPunchAnchorPos(Vector2.left * punchForce, duration);
            _rightCross.DOPunchAnchorPos(Vector2.right * punchForce, duration);

            //Back to starting position.
            DOVirtual.DelayedCall(resetDuration, () =>
            {
                _upCross.DOAnchorPos(_startUp, moveBackDuration);
                _downCross.DOAnchorPos(_startDown, moveBackDuration);
                _leftCross.DOAnchorPos(_startLeft, moveBackDuration);
                _rightCross.DOAnchorPos(_startRight, moveBackDuration);
            });
        }
    }
}
