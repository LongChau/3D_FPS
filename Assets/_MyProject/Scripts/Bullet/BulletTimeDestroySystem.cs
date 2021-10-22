using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace FPS
{
    public class BulletTimeDestroySystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;

            //Entities.WithBurst().WithStructuralChanges()
            //     .ForEach((Entity entity, ref Translation position, ref BulletLifeTimeData lifeTimeData) =>
            //     {
            //         lifeTimeData.lifeTime -= deltaTime;
            //         if (lifeTimeData.lifeTime <= 0)
            //         {
            //             EntityManager.DestroyEntity(entity);
            //         }
            //     }).Run();

            Entities
                .WithoutBurst().WithStructuralChanges()
                .ForEach((Entity entity, ref Translation position, ref BulletLifeTimeData lifeTimeData) =>
                {
                    lifeTimeData.lifeTime -= deltaTime;
                    if (lifeTimeData.lifeTime <= 0)
                    {
                        EntityManager.DestroyEntity(entity);
                    }
                }).Run();

            return inputDeps;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
        }
    }
}
