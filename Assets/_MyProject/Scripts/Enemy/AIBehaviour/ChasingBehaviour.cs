using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS
{
    public class ChasingBehaviour : StateMachineBehaviour
    {
        private EnemyController _enemy;
        private Vector3 _targetPos;

        public event Action Event_ClosedDistance;

        public void Init(EnemyController enemy)
        {
            _enemy = enemy;
            _targetPos = CharacterControl.CharacterPosition;
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Chasing the target.
            _enemy.Agent.SetDestination(_targetPos);
            bool isClosed = Vector3.Distance(_enemy.transform.position, _targetPos) <= _enemy.CloseDistance;
            if (isClosed)
            {
                Event_ClosedDistance?.Invoke();
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
