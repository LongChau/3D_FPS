using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LC.Ultility;
using System;

namespace FPS
{
    public class AttackBehaviour : StateMachineBehaviour
    {
        private EnemyController _enemy;

        public event Action Event_FarDistance;

        public void Init(EnemyController enemy)
        {
            _enemy = enemy;
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Log.Info("Enter Attack state");
            _enemy.Agent.isStopped = false;
            EventManager.Instance.PostEvent(EventID.AttackCharacter, -_enemy.Damage);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            bool isClosed = Vector3.Distance(_enemy.transform.position, CharacterControl.CharacterPosition) <= _enemy.CloseDistance;
            if (!isClosed)
            {
                Event_FarDistance?.Invoke();
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Log.Info("Exit Attack state");
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
