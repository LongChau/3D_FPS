using LC.Ultility;
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

        public event Action Event_ClosedDistance;

        public void Init(EnemyController enemy)
        {
            _enemy = enemy;
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Log.Info("Enter Chasing state");
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            bool isClosed = Vector3.Distance(_enemy.transform.position, CharacterControl.CharacterPosition) <= _enemy.CloseDistance;
            if (isClosed)
            {
                _enemy.Agent.isStopped = true;
                Event_ClosedDistance?.Invoke();
            }
            else
            {
                // Chasing the target.
                _enemy.Agent.isStopped = false;
                _enemy.Agent.SetDestination(CharacterControl.CharacterPosition);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Log.Info("Exit Chasing state");
        }

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
