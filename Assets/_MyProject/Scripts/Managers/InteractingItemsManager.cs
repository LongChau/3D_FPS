using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class InteractingItemsManager : MonoSingletonExt<InteractingItemsManager>
    {
        [SerializeField]
        private List<ItemController> _items = new List<ItemController>();

        public List<ItemController> Items => _items;

        private ItemController _selectableItem;

        [SerializeField]
        private float _threshold = 0.98f;
        [SerializeField]
        private float _closeRange;

        public bool HasItemInSight => _selectableItem != null;

        public override void Init()
        {
            base.Init();
            //DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void AddItem(ItemController item)
        {
            _items.Add(item);
        }

        public void RemoveItem(ItemController item)
        {
            _items.Remove(item);
        }

        public ItemController Check(Ray ray)
        {
            _selectableItem = null;

            var closest = 0f;
            for (int itemIndex = 0; itemIndex < _items.Count; itemIndex++)
            {
                var selectItem = _items[itemIndex];
                var rayDirection = ray.direction;
                var itemDirection = selectItem.transform.position - ray.origin;

                var dotProduct = Vector3.Dot(rayDirection, itemDirection.normalized);
                selectItem.DotPercentage = dotProduct;
                closest = dotProduct;

                var distance = Vector3.Distance(ray.origin, selectItem.transform.position);
                bool isCloseRange = distance <= _closeRange;
                if (dotProduct >= _threshold && dotProduct >= closest && isCloseRange)
                {
                    _selectableItem = selectItem;
                }
            }

            return _selectableItem;
        }
    }
}
