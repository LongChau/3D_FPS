using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class Canvas_LoadingScene : MonoBehaviour
    {
        [SerializeField]
        private Image _imgLoadingBar;

        private void Awake()
        {
            _imgLoadingBar.fillAmount = 0f;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {
            if (GameManager.Instance.isLoadingScene)
                _imgLoadingBar.fillAmount = GameManager.Instance.loadingScenePercentage;
        }
    }
}
