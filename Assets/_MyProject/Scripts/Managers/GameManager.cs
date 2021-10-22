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
        public float LoadingScenePercentage { get; private set; }
        public string LoadingInfo { get; private set; }

        public bool isLoadingScene;

        public override void Init()
        {
            base.Init();
            DontDestroyOnLoad(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            SceneManager.sceneLoaded += Handle_sceneLoaded;
        }

        private void Handle_sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == GameScenes.BATTLE_SCENE)
            {
                Debug.Log("Loaded into BATTLE_SCENE");
                UnloadSceneWithName(GameScenes.LOADING_SCENE);
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= Handle_sceneLoaded;
        }

        public void SetLoadingInfo(int percentage, string info)
        {
            Debug.Log($"{percentage} - {info}");
            LoadingScenePercentage = percentage;
            LoadingInfo = info;
        }

        public void LoadSceneAtIndex(int sceneIndex)
        {
            StartCoroutine(IELoadSceneAtIndex(sceneIndex));
        }

        public void LoadSceneWithName(string sceneName)
        {
            StartCoroutine(IELoadSceneWithName(sceneName));
        }

        public void LoadBattleScene()
        {
            StartCoroutine(IELoadBattleScene());
        }

        IEnumerator IELoadSceneAtIndex(int sceneIndex)
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
                LoadingScenePercentage = asyncOperation.progress;
                //Log.Info($"Percent: {loadingScenePercentage}");
                yield return new WaitForEndOfFrame();
            }

            isLoadingScene = false;
            //asyncOperation.allowSceneActivation = true;
            SceneManager.UnloadSceneAsync(GameScenes.LOADING_SCENE);
        }

        IEnumerator IELoadSceneWithName(string sceneName)
        {
            isLoadingScene = true;
            // Load loading scene.
            var asyncOperation = SceneManager.LoadSceneAsync(GameScenes.LOADING_SCENE, LoadSceneMode.Single);
            var waitForSceneLoaded = new WaitUntil(() => asyncOperation.isDone);
            yield return waitForSceneLoaded;
            // Then load main scene.
            asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!asyncOperation.isDone)
            {
                LoadingScenePercentage = asyncOperation.progress;
                //Log.Info($"Percent: {loadingScenePercentage}");
                yield return null;
            }

            isLoadingScene = false;
            SceneManager.UnloadSceneAsync(GameScenes.LOADING_SCENE);
        }

        /// <summary>
        /// Load the battle scene and the level scene.
        /// </summary>
        /// <returns></returns>
        IEnumerator IELoadBattleScene()
        {
            isLoadingScene = true;

            SetLoadingInfo(20, "Prepare scenes");

            // Load loading scene.
            var loadSceneOperation = SceneManager.LoadSceneAsync(GameScenes.LOADING_SCENE, LoadSceneMode.Additive);
            var waitForLoadingScene = new WaitUntil(() =>
            {
                SetLoadingInfo(30, "Unload unecessary things");
                return loadSceneOperation.isDone;
            });
            yield return waitForLoadingScene;

            // Unload the menu scene.
            var removeMenuSceneOperation = SceneManager.UnloadSceneAsync(GameScenes.MENU_SCENE);
            var waitForRemoveMenuScene = new WaitUntil(() =>
            {
                SetLoadingInfo(40, "Load resources");
                return removeMenuSceneOperation.isDone;
            });
            yield return waitForRemoveMenuScene;

            // Then load the level scene.
            var levelSceneOperation = SceneManager.LoadSceneAsync(GameScenes.LEVEL_SCENE + "01", LoadSceneMode.Additive);
            var waitForLevelScene = new WaitUntil(() =>
            {
                SetLoadingInfo(60, "Load resources");
                return levelSceneOperation.progress >= 0.9f;
            });
            levelSceneOperation.allowSceneActivation = false;
            yield return waitForLevelScene;

            // Then load battle scene.
            var battleSceneOperation = SceneManager.LoadSceneAsync(GameScenes.BATTLE_SCENE, LoadSceneMode.Additive);
            var waitForBattleScene = new WaitUntil(() =>
            {
                SetLoadingInfo(90, "Finalizing");
                return battleSceneOperation.progress >= 0.9f;
            });
            battleSceneOperation.allowSceneActivation = false;
            yield return waitForBattleScene;

            // Activate the previous loading scene.
            levelSceneOperation.allowSceneActivation = true;
            battleSceneOperation.allowSceneActivation = true;
            isLoadingScene = false;

            Debug.Log("Finish load to battle scene and level scene");
            SetLoadingInfo(100, "Set things ready");
        }

        public void UnloadSceneWithName(string sceneName)
        {
            StartCoroutine(IEUnloadSceneWithName(sceneName));
        }

        IEnumerator IEUnloadSceneWithName(string sceneName)
        {
            var unloadSceneOperation = SceneManager.UnloadSceneAsync(sceneName);
            var waitForUnloadScene = new WaitUntil(() => unloadSceneOperation.isDone);
            yield return waitForUnloadScene;
        }
    }
}