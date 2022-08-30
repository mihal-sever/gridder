using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sever.Gridder.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class Page : MonoBehaviour
    {
        private int _maxItems;
        private readonly List<RectTransform> _items = new();
        
        public bool IsFinished => _items.Count == _maxItems;
        public bool IsEmpty => _items.Count == 0;
        
        
        public void Init(int maxItems)
        {
            _maxItems = maxItems;
        }

        public bool Contains(RectTransform item)
        {
            return _items.Contains(item);
        }
        
        public RectTransform AddFirst(GameObject itemPrefab)
        {
            var item = Instantiate(itemPrefab, transform).GetComponent<RectTransform>();
            PushFirst(item);
            return item;
        }
        
        public RectTransform AddLast(GameObject itemPrefab)
        {
            var item = Instantiate(itemPrefab, transform).GetComponent<RectTransform>();
            PushLast(item);
            return item;
        }
        
        public void DeleteItem(RectTransform item)
        {
            Destroy(item.gameObject);
            _items.Remove(item);
        }

        public RectTransform PopFirst()
        {
            var item = _items.First();
            _items.Remove(item);
            return item;
        }
        
        public RectTransform PopLast()
        {
            var item = _items.Last();
            _items.Remove(item);
            return item;
        }
        
        public void PushFirst(RectTransform item)
        {
            item.parent = transform;
            item.SetAsFirstSibling();
            if (Contains(item))
            {
                return;
            }
            _items.Insert(0, item);
        }
        
        public void PushLast(RectTransform item)
        {
            item.parent = transform;
            item.SetAsLastSibling();
            if (Contains(item))
            {
                return;
            }
            _items.Add(item);
        }
    }
}