using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace FPS.Manager
{
    public class GCControl : MonoBehaviour
    {
        private void Start()
        {
            ListenForGCModeChange();
        }

        public static void ListenForGCModeChange()
        {
            // Listen on garbage collector mode changes.
            GarbageCollector.GCModeChanged += (GarbageCollector.Mode mode) =>
            {
                Debug.Log("GCModeChanged: " + mode);
            };
        }

        static void LogMode()
        {
            Debug.Log("GCMode: " + GarbageCollector.GCMode);
        }

        public static void EnableGC()
        {
#if !UNITY_EDITOR
            GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
#endif
            // Trigger a collection to free memory.
            GC.Collect();
        }

        public static void DisableGC()
        {
#if !UNITY_EDITOR
            GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
#endif
        }

        private void OnDestroy()
        {
        }
    }
}
