using LC.Ultility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FPS.BootScene
{
    public class BootScene : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("BootScene.Start()");

        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void GoToMenuScene()
        {
            Debug.Log("BootScene.GoToMenuScene()");
            GameManager.Instance.LoadSceneWithName(GameScenes.MENU_SCENE);
        }
    }
}
