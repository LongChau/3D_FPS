using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private Animator _anim;
        [SerializeField]
        private EEnemyState _currentState;
        [SerializeField]
        private bool _isAttack;
        [SerializeField]
        private NavMeshAgent _agent;
        [SerializeField]
        private float _wanderRadius;

        private WanderBehaviour _wanderBehaviour;

        private float _idleTime;
        private float _wanderTime;
        private float _eatingTime;

        private Coroutine _idle;
        private Coroutine _wander;
        private Coroutine _eat;

        public EEnemyState CurrentState 
        { 
            get => _currentState;
            set
            {
                _currentState = value;
                UpdateState(_currentState);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            //CurrentState = EEnemyState.Idle;
            RandomState();
            _wanderBehaviour = _anim.GetBehaviour<WanderBehaviour>();
            _wanderBehaviour.Init(_agent, transform, _wanderRadius);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void UpdateState(EEnemyState state)
        {
            switch (CurrentState)
            {
                case EEnemyState.Idle:
                    _anim.SetTrigger("TriggerIdle");
                    StartIdling();
                    break;
                case EEnemyState.Wander:
                    _anim.SetTrigger("TriggerWalk");
                    StartWandering();
                    break;
                case EEnemyState.Eating:
                    _anim.SetTrigger("TriggerEating");
                    StartEating();
                    break;
                case EEnemyState.Attack:
                    _anim.SetTrigger("TriggerAttack");

                    break;
                case EEnemyState.Chasing:
                    _anim.SetTrigger("TriggerChasing");

                    break;
                case EEnemyState.Die:
                    //_anim.SetTrigger("TriggerDie");

                    break;
                default:
                    break;
            }
        }

        private void StartIdling()
        {
            _idle = StartCoroutine(IEStartIdling());
        }

        private IEnumerator IEStartIdling()
        {
            _idleTime = Random.Range(2f, 5f);
            var wait = new WaitForSecondsRealtime(_idleTime);
            yield return wait;
            RandomState();
        }

        private void StartWandering()
        {
            _wander = StartCoroutine(IEStartWandering());
        }

        private IEnumerator IEStartWandering()
        {
            _wanderTime = Random.Range(10, 20f);
            var wait = new WaitForSecondsRealtime(_wanderTime);
            yield return wait;
            RandomState();
        }

        private void StartEating()
        {
            _wander = StartCoroutine(IEStartEating());
        }

        private IEnumerator IEStartEating()
        {
            _eatingTime = Random.Range(5, 10f);
            var wait = new WaitForSecondsRealtime(_eatingTime);
            yield return wait;
            RandomState();
        }

        private void RandomState()
        {
            var stateNum = Random.Range(0, 3);
            var state = (EEnemyState)stateNum;
            CurrentState = state;
        }
    }
}
