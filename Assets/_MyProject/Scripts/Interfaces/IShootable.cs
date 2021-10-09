using UnityEngine;

namespace FPS
{
    internal interface IShootable
    {
        float CurrentHp { get; set; }
        void InstantiateEffect(GameObject effectPrefab, Vector3 hitPosition, Quaternion rotation, float destroyTime);
    }
}