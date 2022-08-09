using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.Gridder.UI
{
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
        
        public RectTransform AddItem(GameObject itemPrefab)
        {
            var item = Instantiate(itemPrefab, transform).GetComponent<RectTransform>();
            _items.Add(item);
            return item;
        }
        
        public void DeleteItem(RectTransform item)
        {
            Destroy(item.gameObject);
            _items.Remove(item);
        }

        public RectTransform Pop()
        {
            var item = _items.First();
            _items.Remove(item);
            return item;
        }

        public void Push(RectTransform item)
        {
            _items.Add(item);
        }
    }
}