using FPS.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class MenuSceneControl : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void AfterSceneLoaded()
        {
            Resources.UnloadUnusedAssets();
            GCControl.EnableGC();
        }
    }
}
