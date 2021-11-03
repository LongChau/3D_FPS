using FPS.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class BattleSceneControl : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void AfterSceneLoaded()
        {
            GCControl.DisableGC();
        }
    }
}
