using LC.Ultility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using FPS.AssetData;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace FPS.BootScene
{
    public class BootScene : MonoBehaviour
    {
        [SerializeField, InlineEditor]
        private BootSceneSetting _bootSceneSetting;

        private void Start()
        {
            Debug.Log("BootScene.Start()");
            DOTween.defaultRecyclable = true;
            GameManager.Instance.LoadSceneWithName(GameScenes.MENU_SCENE);
        }

        /// <summary>
        /// This will force all scene begins with Menu scene.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        void GoToBootScene()
        {
            if (!_bootSceneSetting.allowBootSceneOnLoad) return;
            if (SceneManager.GetActiveScene().name == GameScenes.BOOT_SCENE) return;
            Debug.Log("BootScene.GoToBootScene()");
            GameManager.Instance.LoadSceneWithName(GameScenes.BOOT_SCENE);
        }
    }
}
