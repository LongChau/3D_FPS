using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Linq;
using System;

namespace FPS.AssetData.Control
{
    [CreateAssetMenu(fileName = "AssetDataControl", menuName = "FPS/AssetData/AssetDataControl")]
    public partial class AssetDataControl : SerializedScriptableObject
    {
        [SerializeField, TabGroup("Characters")]
        private AssetDataInfo _characterAssets;
        [SerializeField, TabGroup("Enemies")]
        private AssetDataInfo _enemyAssets;
        [SerializeField, TabGroup("Settings")]
        private AssetDataInfo _settingAssets;
        [SerializeField, TabGroup("Weapons")]
        private AssetDataInfo _weaponAssets;
    }

    [Serializable]
    public struct AssetDataInfo
    {
        [SerializeField, FolderPath]
        private string _label;
        [SerializeField]
        private List<ScriptableObject> _assetDatas;

        public string Label { get => _label; set => _label = value; }
        public List<ScriptableObject> AssetDatas { get => _assetDatas; set => _assetDatas = value; }
    }
}
