using LC.Ultility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private Animator _anim;
        [SerializeField]
        private EEnemyState _currentState;
        [SerializeField]
        private NavMeshAgent _agent;
        [SerializeField]
        private LineOfSight _lineOfSight;
        [SerializeField]
        private Collider _col;

        [Header("--Audio--")]
        [SerializeField]
        private AudioSource _hitAudio;

        [Header("---Setting---")]
        [SerializeField]
        private float _wanderRadius;
        [SerializeField]
        private float _closeDistance;
        [SerializeField]
        private int _maxHp;
        [SerializeField]
        private int _score;
        [SerializeField]
        private int _damage = 5;

        private int _currentHp;

        private WanderBehaviour _wanderBehaviour;
        private ChasingBehaviour _chasingBehaviour;
        private AttackBehaviour _attackBehaviour;
        private DeadBehaviour _deadBehaviour;

        private Coroutine _unharmedCoroutine;

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
                            StartUnharmedAction(3, 5);
                            break;
                        case EEnemyState.Wander:
                            _anim.SetTrigger("TriggerWalk");
                            StartUnharmedAction(3, 5);
                            break;
                        case EEnemyState.Eating:
                            _anim.SetTrigger("TriggerEating");
                            StartUnharmedAction(3, 5);
                            break;
                        case EEnemyState.Attack:
                            _anim.SetTrigger("TriggerAttack");
                            StopUnharmedActions();
                            break;
                        case EEnemyState.Chasing:
                            _anim.SetTrigger("TriggerChasing");
                            StopUnharmedActions();
                            break;
                        case EEnemyState.Die:
                            _anim.SetTrigger("TriggerDie");
                            StopUnharmedActions();
                            _lineOfSight.enabled = false;
                            _col.enabled = false;
                            this.PostEvent(EventID.GainScore, _score);
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

        public int CurrentHp 
        { 
            get => _currentHp; 
            set
            {
                _currentHp = value;
                _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);
                if (_currentHp == 0 && CurrentState != EEnemyState.Die)
                    CurrentState = EEnemyState.Die;
            }
        }

        public int Damage => _damage;

        private void Awake()
        {
            CurrentHp = _maxHp;

            //CurrentState = EEnemyState.Idle;
            RandomNextUnharmedState();

            _wanderBehaviour = _anim.GetBehaviour<WanderBehaviour>();
            _chasingBehaviour = _anim.GetBehaviour<ChasingBehaviour>();
            _attackBehaviour = _anim.GetBehaviour<AttackBehaviour>();
            _deadBehaviour = _anim.GetBehaviour<DeadBehaviour>();
            _wanderBehaviour.Init(this);
            _chasingBehaviour.Init(this);
            _attackBehaviour.Init(this);
            _deadBehaviour.Init(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            _chasingBehaviour.Event_ClosedDistance += Handle_Event_ClosedDistance;
            _attackBehaviour.Event_FarDistance += Handle_Event_FarDistance;
        }

        private void Handle_Event_FarDistance()
        {
            CurrentState = EEnemyState.Chasing;
        }

        private void Handle_Event_ClosedDistance()
        {
            CurrentState = EEnemyState.Attack;
        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentState == EEnemyState.Die) return;
            if (_lineOfSight.enabled && _lineOfSight.IsInSight && CurrentState != EEnemyState.Chasing &&
                CurrentState != EEnemyState.Attack)
            {
                CurrentState = EEnemyState.Chasing;
            }
        }

        public void StopUnharmedActions()
        {
            if (_unharmedCoroutine != null) StopCoroutine(_unharmedCoroutine);
        }

        private void StartUnharmedAction(int minTime, int maxTime)
        {
            Log.Info("StartUnharmedAction()");
            _unharmedCoroutine = StartCoroutine(IEUnharmedAction(minTime, maxTime));
        }

        private IEnumerator IEUnharmedAction(int minTime, int maxTime)
        {
            var timer = Random.Range(minTime, maxTime);
            var wait = new WaitForSecondsRealtime(1);
            while (!_lineOfSight.IsInSight && timer > 0)
            {
                timer--;
                yield return wait;
            }
            RandomNextUnharmedState();
        }

        private void RandomNextUnharmedState()
        {
            var stateNum = Random.Range(0, 3);
            var state = (EEnemyState)stateNum;
            CurrentState = state;
            Log.Info($"RandomUnharmedStates() => {CurrentState}");
        }

        public void ChangeAttackAnimationMultiplier(float speed)
        {
            _anim.SetFloat("AttackMultiplie", speed);
        }

        public void InstantiateEffect(GameObject effectPrefab, Vector3 hitPosition, Quaternion rotation, float destroyTime)
        {
            var fx = Instantiate(effectPrefab, hitPosition, rotation);
            Destroy(fx, destroyTime);
        }

        public void TakeDamage(int dmg)
        {
            CurrentHp -= dmg;
            _hitAudio.Play();
        }

        private void OnDestroy()
        {
            _chasingBehaviour.Event_ClosedDistance -= Handle_Event_ClosedDistance;
            _attackBehaviour.Event_FarDistance -= Handle_Event_FarDistance;
        }
    }
}
