using UnityEngine;

namespace FPS
{
    public interface IDamageable
    {
        int CurrentHp { get; set; }
        void TakeDamage(int dmg);
        void InstantiateEffect(GameObject effectPrefab, Vector3 hitPosition, Quaternion rotation, float destroyTime);
    }
}