using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public partial class WeaponControl
    {
        [Button, TabGroup("Debug")]
        public void Test_Fire()
        {
            var recoidData = new RecoidData
            {
                duration = 0.5f,
                strength = 0.05f,
                vibrato = 1,
                randomness = 20
            };
            Fire(recoidData);
        }
    }
}
