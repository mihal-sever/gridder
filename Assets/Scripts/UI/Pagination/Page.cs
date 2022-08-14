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

        private RectTransform _rectTransform;
        
        public bool IsFinished => _items.Count == _maxItems;
        public bool IsEmpty => _items.Count == 0;
        
        
        public void Init(int maxItems)
        {
            _rectTransform = GetComponent<RectTransform>();
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
            var anchoredPosition = item.anchoredPosition;
            item.parent = _rectTransform;
            item.anchoredPosition = anchoredPosition;
            _items.Add(item);
        }
    }
}