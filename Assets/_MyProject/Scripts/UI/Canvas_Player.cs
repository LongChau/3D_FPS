using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class Canvas_Player : MonoBehaviour
    {
        [SerializeField]
        private CrossHairUI _crossHair;
        [SerializeField]
        private Button _btnUse;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {
            _btnUse.gameObject.SetActive(InteractingItemsManager.Instance.HasItemInSight);
        }
    }
}
