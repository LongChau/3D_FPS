using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class DamagableControl : MonoSingletonExt<DamagableControl>
    {
        public Dictionary<int, IDamageable> DictDamageables = new Dictionary<int, IDamageable>();

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        //private void RegisterEvents()
        //{

        //}

        public override void Init()
        {
            base.Init();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
