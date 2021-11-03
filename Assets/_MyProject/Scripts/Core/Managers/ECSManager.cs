using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace FPS
{
    public class ECSManager : MonoSingletonExt<ECSManager>
    {
        //public EntityManager Manager { get; private set; }

        public override void Init()
        {
            base.Init();
            //Manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
