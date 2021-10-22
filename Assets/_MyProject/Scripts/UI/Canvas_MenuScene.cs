using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FPS
{
    public class Canvas_MenuScene : MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void OnBtnPlayClicked()
        {
            //GameManager.Instance.LoadSceneWithName(GameScenes.BATTLE_SCENE);
            _canvas.enabled = false;
            GameManager.Instance.LoadBattleScene();
        }
    }
}
