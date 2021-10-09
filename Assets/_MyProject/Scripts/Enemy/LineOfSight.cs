using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class LineOfSight : MonoBehaviour
    {
        [SerializeField]
        private Transform _leftSight;
        [SerializeField]
        private Transform _rightSight;

        [SerializeField]
        private float _range;

        public Transform Target;

        public bool InSight;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            InSight = IsInSight();
        }

        private bool IsInSight()
        {
            var targetPos = new Vector2(Target.position.x, Target.position.z);
            var myPos = new Vector2(transform.position.x, transform.position.z);
            bool isClosedRange = Vector2.Distance(targetPos, myPos) <= _range;

            var leftLine = _leftSight.forward * _range;
            var rightLine = _rightSight.forward * _range;

            var leftLine2D = new Vector2(leftLine.x, leftLine.z);
            var rightLine2D = new Vector2(rightLine.x, rightLine.z);
            var targetDirection = targetPos - myPos;

            float angleArea = Vector2.Angle(leftLine2D, rightLine2D);
            bool withinLeft = Vector2.Angle(leftLine2D, targetDirection) < angleArea;
            bool withinRight = Vector2.Angle(rightLine2D, targetDirection) < angleArea;

            bool isInLine = withinLeft && withinRight;

            return isClosedRange && isInLine;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(_leftSight.transform.position, _leftSight.forward * _range);
            Gizmos.DrawRay(_rightSight.transform.position, _rightSight.forward * _range);
        }


    }
}
