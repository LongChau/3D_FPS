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
        public override void Init()
        {
            base.Init();
            DontDestroyOnLoad(this);
        }

        // Start is called before the first frame update
        void Start()
        {

        }
    }
}