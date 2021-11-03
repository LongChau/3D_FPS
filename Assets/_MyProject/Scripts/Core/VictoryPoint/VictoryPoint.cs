using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LC.Ultility;

namespace FPS
{
    public class VictoryPoint : MonoBehaviour
    {
        private bool _isEntered;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_isEntered)
            {
                var charCtrl = other.GetComponent<CharacterControl>();
                this.PostEvent(EventID.PlayerWin, charCtrl.CurHealth);
                _isEntered = true;
            }
        }
    }
}
