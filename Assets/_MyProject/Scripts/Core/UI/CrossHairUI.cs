using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace FPS
{
    public class CrossHairUI : MonoBehaviour
    {
        [SerializeField]
        private float m_CrosshairAperture = 192;

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

        //private Tweener tween1;
        //private Tweener tween2;
        //private Tweener tween3;
        //private Tweener tween4;
        //private Tween tween5;

        // Start is called before the first frame update
        void Start()
        {
            _startUp = _upCross.anchoredPosition;
            _startDown = _downCross.anchoredPosition;
            _startLeft = _leftCross.anchoredPosition;
            _startRight = _rightCross.anchoredPosition;

            //_seq = DOTween.Sequence();

            //var tween1 = _upCross.DOPunchAnchorPos(Vector2.up * 10f, 0.2f);
            //var tween2 = _downCross.DOPunchAnchorPos(Vector2.down * 10f, 0.2f);
            //var tween3 = _leftCross.DOPunchAnchorPos(Vector2.left * 10f, 0.2f);
            //var tween4 = _rightCross.DOPunchAnchorPos(Vector2.right * 10f, 0.2f);

            //var tween5 = DOVirtual.DelayedCall(1f, () =>
            //{
            //    _upCross.DOAnchorPos(_startUp, 0.5f);
            //    _downCross.DOAnchorPos(_startDown, 0.5f);
            //    _leftCross.DOAnchorPos(_startLeft, 0.5f);
            //    _rightCross.DOAnchorPos(_startRight, 0.5f);
            //});

            //_seq.Append(tween1).Append(tween2).Append(tween3).Append(tween4).Append(tween5);
            //_seq.Play();
        }

        //public float fixedAcccuracy;
        public void Move(float? accuracy)
        {
            float fixedAcccuracy = accuracy ?? 0f;
            //float fixedAcccuracy = 1.0f;
            _upCross.localPosition = new Vector3(0, m_CrosshairAperture * (1 - fixedAcccuracy));
            _downCross.localPosition = new Vector3(0, -m_CrosshairAperture * (1 - fixedAcccuracy));
            _rightCross.localPosition = new Vector3(m_CrosshairAperture * (1 - fixedAcccuracy), 0);
            _leftCross.localPosition = new Vector3(-m_CrosshairAperture * (1 - fixedAcccuracy), 0);
        }

        public void PlayCrossHairEffect(float punchForce, float duration, float resetDuration, float moveBackDuration)
        {
            //_upCross.DOPunchAnchorPos(Vector2.up * punchForce, duration);
            //_downCross.DOPunchAnchorPos(Vector2.down * punchForce, duration);
            //_leftCross.DOPunchAnchorPos(Vector2.left * punchForce, duration);
            //_rightCross.DOPunchAnchorPos(Vector2.right * punchForce, duration);

            ////Back to starting position.
            //DOVirtual.DelayedCall(resetDuration, () =>
            //{
            //    _upCross.DOAnchorPos(_startUp, moveBackDuration);
            //    _downCross.DOAnchorPos(_startDown, moveBackDuration);
            //    _leftCross.DOAnchorPos(_startLeft, moveBackDuration);
            //    _rightCross.DOAnchorPos(_startRight, moveBackDuration);
            //});
        }
    }
}
