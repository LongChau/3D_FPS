using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField]
        private Image _imgHp;
        [SerializeField]
        private TextMeshProUGUI _txtScore;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {
            _btnUse.gameObject.SetActive(InteractingItemsManager.Instance.HasItemInSight);
        }

        public void UpdateHealthUI(int curHp, int maxHp)
        {
            _imgHp.fillAmount = (float)curHp / maxHp;
        }
    }
}
