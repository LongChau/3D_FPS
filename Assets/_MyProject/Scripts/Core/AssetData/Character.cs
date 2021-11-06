using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.AssetData
{
    [CreateAssetMenu(fileName = "Character", menuName = "FPS/AssetData/Character")]
    public sealed class Character : ScriptableObject
    {
        [SerializeField]
        private int _maxHealth;
        [SerializeField]
        private float _movementSpeed;
        [SerializeField]
        private float _jumpHeight;
    }
}
