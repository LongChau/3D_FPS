using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FPS
{
    public class Canvas_MenuScene : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        public void OnBtnPlayClicked()
        {
            //var asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            GameManager.Instance.LoadScene(1);
        }
    }
}
