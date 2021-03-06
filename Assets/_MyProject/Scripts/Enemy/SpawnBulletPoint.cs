using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace FPS
{
    public class SpawnBulletPoint : MonoBehaviour
    {
        [SerializeField]
        private GameObject _bulletPrefab;

        private BlobAssetStore _store;
        private Entity _enemyEnity;
        private GameObjectConversionSettings _setting;

        // Start is called before the first frame update
        void Start()
        {
            _store = new BlobAssetStore();
            _setting = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _store);
            _enemyEnity = GameObjectConversionUtility.ConvertGameObjectHierarchy(_bulletPrefab.gameObject, _setting);
            //SpawnEnity();
        }

        public void SpawnEnity()
        {
            var instance = World.DefaultGameObjectInjectionWorld.EntityManager.Instantiate(_enemyEnity);
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;
            float3 newPos = new float3(x, y, z);
            
            World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(instance, new Translation { Value = newPos });
        }

        private void OnDestroy()
        {
            //if (_enemyEnity != null)
            //    World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(_enemyEnity);
            //if (_store != null)
            //    _store.Dispose();

            _setting.BlobAssetStore.Dispose();
        }
    }
}
