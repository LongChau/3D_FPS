using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "FPS/AssetData/Enemy")]
    public class Enemy : ScriptableObject
    {
        [SerializeField]
        private int _maxHp;
        [SerializeField]
        private float _sightRange;
        [SerializeField]
        private int _score;
        [SerializeField]
        private int _damage = 5;
        [SerializeField]
        private float _wanderSpeed;
        [SerializeField]
        private float _chasingSpeed;
        [SerializeField]
        private float _wanderRadius;
        [SerializeField]
        private float _closeDistance;
    }
}
