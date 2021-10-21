using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

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

        public Vector3 TargetPos { get; private set; }

        public bool IsInSight { get; private set; }

        [Header("---TEST ONLY---")]
        public bool test;
        public Transform targetTest;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (test) TargetPos = targetTest.position;
            else TargetPos = CharacterControl.CharacterPosition;
            //IsInSight = CheckInSight();

            ExecuteCheckInSightJob();
        }

        //private bool CheckInSight()
        //{
        //    // Check the distance.
        //    var targetPos = new Vector2(TargetPos.x, TargetPos.z);
        //    var myPos = new Vector2(transform.position.x, transform.position.z);
        //    bool isClosedRange = Vector2.Distance(targetPos, myPos) <= _range;

        //    var leftLine = _leftSight.forward * _range;
        //    var rightLine = _rightSight.forward * _range;

        //    // Check the angle.
        //    var leftLine2D = new Vector2(leftLine.x, leftLine.z);
        //    var rightLine2D = new Vector2(rightLine.x, rightLine.z);
        //    var targetDirection = targetPos - myPos;

        //    float angleArea = Vector2.Angle(leftLine2D, rightLine2D);
        //    bool withinLeft = Vector2.Angle(leftLine2D, targetDirection) < angleArea;
        //    bool withinRight = Vector2.Angle(rightLine2D, targetDirection) < angleArea;

        //    bool isInLine = withinLeft && withinRight;

        //    return isClosedRange && isInLine;
        //}

        public void ExecuteCheckInSightJob()
        {
            CheckInSightJob job = new CheckInSightJob(new float2(TargetPos.x, TargetPos.z), 
                new float2(transform.position.x, transform.position.z), 
                _leftSight.forward, _rightSight.forward, _range);
            var jobHandle = job.Schedule();
            jobHandle.Complete();
            IsInSight = job.results[0];
            job.results.Dispose();
        }

        //[BurstCompile]
        struct CheckInSightJob : IJob
        {
            public float2 _targetPos;
            public float2 _currentPos;

            public float3 _leftSideForward;
            public float3 _rightSideForward;

            public float _range;

            public NativeArray<bool> results;

            public CheckInSightJob(float2 targetPos, float2 currentPos, float3 leftSideForward, float3 rightSideForward, float range)
            {
                this._targetPos = targetPos;
                this._currentPos = currentPos;
                this._leftSideForward = leftSideForward;
                this._rightSideForward = rightSideForward;
                this._range = range;

                results = new NativeArray<bool>(1, Allocator.Persistent);
            }

            public void Execute()
            {
                // Check the distance.
                bool isCloseRange = math.distance(_targetPos, _currentPos) <= _range;

                var leftLine = _leftSideForward * _range;
                var rightLine = _rightSideForward * _range;

                // Check the angle.
                var leftLine2D = new float2(leftLine.x, leftLine.z);
                var rightLine2D = new float2(rightLine.x, rightLine.z);
                var targetDirection = _targetPos - _currentPos;
                
                float angleArea = Vector2.Angle(leftLine2D, rightLine2D);
                bool withinLeft = Vector2.Angle(leftLine2D, targetDirection) < angleArea;
                bool withinRight = Vector2.Angle(rightLine2D, targetDirection) < angleArea;

                bool isInLine = withinLeft && withinRight;

                results[0] = isCloseRange && isInLine;
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(_leftSight.transform.position, _leftSight.forward * _range);
            Gizmos.DrawRay(_rightSight.transform.position, _rightSight.forward * _range);
        }
    }
}
