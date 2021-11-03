using LC.Ultility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class ItemController : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private EItemType _itemType;
        public float DotPercentage { get; set; }

        private void Start()
        {
            InteractingItemsManager.Instance.AddItem(this);
        }

        public void Interact()
        {
            Log.Info($"Interact with {name}");

            switch (_itemType)
            {
                case EItemType.HealthBag:
                    EventManager.Instance.PostEvent(EventID.GetHealth, 25);
                    break;
                case EItemType.AmmoBag:
                    EventManager.Instance.PostEvent(EventID.GetAmmo, 30);
                    break;
                default:
                    break;
            }

            InteractingItemsManager.Instance.RemoveItem(this);
            Destroy(gameObject);
        }
    }
}
