using LC.Ultility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS
{
    public class WanderBehaviour : StateMachineBehaviour
    {
        private EnemyController _enemy;
        private float _randomTimer;
        [SerializeField]
        private float _maxRandomTimer = 5;

        public void Init(EnemyController enemy)
        {
            _enemy = enemy;
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Log.Info("Enter Wander state");
            _enemy.Agent.isStopped = false;
            _randomTimer = _maxRandomTimer;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _enemy.Agent.isStopped = false;
            if ((_randomTimer -= Time.deltaTime) <= 0 ||  
                Vector3.Distance(_enemy.transform.position, _enemy.Agent.destination) <= 0.01f)
            {
                Vector3 newPos = RandomNavSphere(_enemy.transform.position, _enemy.WanderRadius, -1);
                _enemy.Agent.SetDestination(newPos);
                _randomTimer = _maxRandomTimer;
            }
        }

        public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;
            randDirection += origin;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

            return navHit.position;
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Log.Info("Exit Wander state");
            _enemy.Agent.isStopped = true;
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
