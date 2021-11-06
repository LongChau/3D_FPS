using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.AssetData
{
    [CreateAssetMenu(fileName = "BootSceneSetting", menuName = "FPS/AssetData/Setting")]
    public sealed class BootSceneSetting : ScriptableObject
    {
        public bool allowBootSceneOnLoad;
    }
}
