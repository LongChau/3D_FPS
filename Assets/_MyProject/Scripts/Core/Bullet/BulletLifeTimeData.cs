using Unity.Entities;

namespace FPS
{
    [GenerateAuthoringComponent]
    public struct BulletLifeTimeData : IComponentData
    {
        public float lifeTime;
    }
}
