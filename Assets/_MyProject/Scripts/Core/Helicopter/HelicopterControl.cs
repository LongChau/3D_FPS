using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class HelicopterControl : MonoBehaviour
    {
        [SerializeField]
        private Transform _rotorTail;
        [SerializeField]
        private Transform _rotorMain;

        [SerializeField]
        private float _rotorSpeed;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _rotorMain.RotateAroundAxis_Y(15f, _rotorSpeed);
            _rotorTail.RotateAroundAxis_X(15f, _rotorSpeed);
        }
    }
}
