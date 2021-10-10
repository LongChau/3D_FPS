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
        private NavMeshAgent _agent;
        [SerializeField]
        private LineOfSight _lineOfSight;

        [Header("---Setting---")]
        [SerializeField]
        private float _wanderRadius;
        [SerializeField]
        private float _closeDistance;

        private WanderBehaviour _wanderBehaviour;
        private ChasingBehaviour _chasingBehaviour;
        private AttackBehaviour _attackBehaviour;

        private int _idleTime;
        private int _wanderTime;
        private int _eatingTime;

        private Coroutine _idle;
        private Coroutine _wander;
        private Coroutine _eat;

        public EEnemyState CurrentState 
        { 
            get => _currentState;
            private set
            {
                _currentState = value;
                UpdateState(_currentState);
                void UpdateState(EEnemyState state)
                {
                    switch (_currentState)
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
            }
        }

        public NavMeshAgent Agent => _agent;
        public LineOfSight LineOfSight => _lineOfSight;
        public float WanderRadius => _wanderRadius;
        public float CloseDistance => _closeDistance;

        // Start is called before the first frame update
        void Start()
        {
            //CurrentState = EEnemyState.Idle;
            RandomState();

            _wanderBehaviour = _anim.GetBehaviour<WanderBehaviour>();
            _chasingBehaviour = _anim.GetBehaviour<ChasingBehaviour>();
            _attackBehaviour = _anim.GetBehaviour<AttackBehaviour>();
            _wanderBehaviour.Init(this);
            _chasingBehaviour.Init(this);
            //_attackBehaviour.Init(_agent, transform, _wanderRadius);

            _chasingBehaviour.Event_ClosedDistance += Handle_Event_ClosedDistance;
        }

        private void Handle_Event_ClosedDistance()
        {
            CurrentState = EEnemyState.Attack;
        }

        // Update is called once per frame
        void Update()
        {
            if (_lineOfSight.IsInSight && CurrentState != EEnemyState.Chasing)
            {
                CurrentState = EEnemyState.Chasing;
            }
            else if (!_lineOfSight.IsInSight && CurrentState != EEnemyState.Chasing)
            {

            }
        }

        private void StopStaticActions()
        {

        }

        private void StartIdling()
        {
            _idle = StartCoroutine(IEStartIdling());
        }

        private IEnumerator IEStartIdling()
        {
            _idleTime = Random.Range(2, 5);
            var wait = new WaitForSecondsRealtime(1);
            while (!_lineOfSight.IsInSight || _idleTime > 0)
            {
                _idleTime--;
                yield return wait;
            }
            RandomState();
        }

        private void StartWandering()
        {
            _wander = StartCoroutine(IEStartWandering());
        }

        private IEnumerator IEStartWandering()
        {
            _wanderTime = Random.Range(10, 20);
            var wait = new WaitForSecondsRealtime(1);
            while (!_lineOfSight.IsInSight || _wanderTime > 0)
            {
                _wanderTime--;
                yield return wait;
            }
            RandomState();
        }

        private void StartEating()
        {
            _wander = StartCoroutine(IEStartEating());
        }

        private IEnumerator IEStartEating()
        {
            _eatingTime = Random.Range(5, 10);
            var wait = new WaitForSecondsRealtime(_eatingTime);
            while (!_lineOfSight.IsInSight || _eatingTime > 0)
            {
                _eatingTime--;
                yield return wait;
            }
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
