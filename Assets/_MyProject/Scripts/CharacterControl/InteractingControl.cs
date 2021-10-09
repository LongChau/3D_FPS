using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LC.Ultility;

namespace FPS
{
    /// <summary>
    /// check all available interatable items.
    /// </summary>
    public class InteractingControl : MonoBehaviour
    {
        private ItemController _item;
        [SerializeField]
        private Camera _fpsMainCam;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var ray = new Ray(_fpsMainCam.transform.position, _fpsMainCam.transform.forward);
            _item = InteractingItemsManager.Instance.Check(ray);
        }

        public void UseItem()
        {
            if (_item != null)
            {
                _item.Interact();
            }
        }
    }
}
