using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor Script for AssetDataControl
/// </summary>
namespace FPS.AssetData.Control
{
    public partial class AssetDataControl
    {
        [Button]
        void FindAssets()
        {
            Debug.Log("FindAssets");

            _characterAssets.AssetDatas.Clear();
            _enemyAssets.AssetDatas.Clear();
            _settingAssets.AssetDatas.Clear();
            _weaponAssets.AssetDatas.Clear();

            Find(_characterAssets);
            Find(_enemyAssets);
            Find(_settingAssets);
            Find(_weaponAssets);
        }

        private void Find(AssetDataInfo assetData)
        {
            // Get the GUID
            var assetIds = AssetDatabase.FindAssets(assetData.Label);

            // Iterate through the asset GUIDs and mapping them.
            for (int i = 0; i < assetIds.Length; i++)
            {
                var guid = assetIds[i];
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                assetData.AssetDatas.Add(asset);
            }
        }
    }
}
