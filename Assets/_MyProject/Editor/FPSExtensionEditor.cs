using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FPS.Editor
{
    public class FPSExtensionEditor
    {
        [MenuItem("Tools/FPS/OpenPersistentDataPath")]
        static void OpenPersistentDataPath()
        {
            Debug.Log("OpenPersistentDataPath");
            Application.OpenURL(Application.persistentDataPath);
        }
    }
}
