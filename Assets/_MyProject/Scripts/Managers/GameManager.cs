using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LC.Ultility;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace FPS
{
    /// <summary>
    /// Manage through out the game
    /// </summary>
    public class GameManager : MonoSingletonExt<GameManager>
    {
        public float loadingScenePercentage;
        public bool isLoadingScene;

        public override void Init()
        {
            base.Init();
            DontDestroyOnLoad(this);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void LoadScene(int sceneIndex)
        {
            StartCoroutine(IELoadScene(sceneIndex));
        }

        IEnumerator IELoadScene(int sceneIndex)
        {
            isLoadingScene = true;
            // Load loading scene.
            var asyncOperation = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
            yield return new WaitUntil(() => asyncOperation.isDone);
            // Then load main scene.
            asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            //asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                loadingScenePercentage = asyncOperation.progress;
                //Log.Info($"Percent: {loadingScenePercentage}");
                yield return new WaitForEndOfFrame();
            }

            isLoadingScene = false;
            //asyncOperation.allowSceneActivation = true;
            SceneManager.UnloadSceneAsync(2);
        }
    }
}