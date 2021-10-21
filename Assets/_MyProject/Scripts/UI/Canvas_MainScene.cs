using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LC.Ultility;
using System;
using TMPro;
using DG.Tweening;

namespace FPS
{
    public class Canvas_MainScene : MonoBehaviour
    {
        [SerializeField]
        private Canvas _mainCanvas;
        [SerializeField]
        private RectTransform _pnlMainMenu;
        [SerializeField]
        private RectTransform _pnlWin;
        [SerializeField]
        private RectTransform _pnlLoose;
        [SerializeField]
        private TextMeshProUGUI _txtScore;

        private void Awake()
        {
            _mainCanvas.enabled = false;
            _pnlMainMenu.gameObject.SetActive(true);
            _pnlWin.gameObject.SetActive(false);
            _pnlLoose.gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            this.RegisterListener(EventID.OpenMainSceneMenu, Handle_OpenMainSceneMenu);
            this.RegisterListener(EventID.PlayerWin, Handle_PlayerWin);
            this.RegisterListener(EventID.PlayerLoose, Handle_PlayerLoose);
        }

        private void Handle_PlayerLoose(object obj)
        {
            _mainCanvas.enabled = true;
            _pnlMainMenu.gameObject.SetActive(false);
            _pnlWin.gameObject.SetActive(false);
            _pnlLoose.gameObject.SetActive(true);
        }

        private void Handle_PlayerWin(object score)
        {
            _mainCanvas.enabled = true;
            _pnlMainMenu.gameObject.SetActive(false);
            _pnlWin.gameObject.SetActive(true);
            _pnlLoose.gameObject.SetActive(false);

            _txtScore.SetText($"Your score: {score}");
        }

        private void Handle_OpenMainSceneMenu(object obj)
        {
            _mainCanvas.enabled = true;
            _pnlMainMenu.gameObject.SetActive(true);
            _pnlWin.gameObject.SetActive(false);
            _pnlLoose.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            EventManager.Instance?.RemoveListener(EventID.OpenMainSceneMenu, Handle_OpenMainSceneMenu);
            EventManager.Instance?.RemoveListener(EventID.PlayerWin, Handle_PlayerWin);
            EventManager.Instance?.RemoveListener(EventID.PlayerLoose, Handle_PlayerLoose);
        }

        public void OnBtnRestartClicked()
        {
            DOTween.Clear();
            GameManager.Instance.LoadSceneWithName(GameScenes.BATTLE_SCENE);
        }

        public void OnBtnMainMenuClicked()
        {
            GameManager.Instance.LoadSceneWithName(GameScenes.MENU_SCENE);
        }

        public void OnBtnBackClicked()
        {
            _mainCanvas.enabled = false;
        }
    }
}
