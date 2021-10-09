using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private Animator _anim;
        [SerializeField]
        private EEnemyState _currentState;
        [SerializeField]
        private float _movementSpeed;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ChangeState(EEnemyState state)
        {
            _currentState = state;
            switch (_currentState)
            {
                case EEnemyState.Idle:
                    _anim.SetTrigger("TriggerIdle");
                    break;
                case EEnemyState.Walking:
                    _anim.SetTrigger("TriggerWalk");
                    break;
                case EEnemyState.Attack:
                    _anim.SetTrigger("TriggerWalk");
                    break;
                case EEnemyState.Eating:
                    _anim.SetTrigger("TriggerEating");
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
