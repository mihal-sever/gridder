using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.Gridder.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(SwipeManager))]
    public class PaginationController : MonoBehaviour
    {
        [SerializeField] private float _moveDuration = .5f;
        [SerializeField] private int _maxItemsPerPage = 6;
        [SerializeField] private GameObject _pagePrefab;

        private readonly List<Page> _pages = new();

        private RectTransform _rectTransform;
        private Page _lastPage;
        private int _openedPageIndex;
        private float _swipeOffset;


        public void Init()
        {
            _rectTransform = GetComponent<RectTransform>();
            _swipeOffset = GetComponent<HorizontalLayoutGroup>().spacing + _rectTransform.rect.size.x;
            
            _lastPage = AddPage();
        }

        private void OnEnable()
        {
            SwipeManager.OnSwipeDetected += OnSwipeDetected;
        }

        private void OnDisable()
        {
            _openedPageIndex = 0;
            _rectTransform?.Move(_rectTransform.anchoredPosition, new Vector2(-_openedPageIndex * _swipeOffset, 0));
            
            SwipeManager.OnSwipeDetected -= OnSwipeDetected;
        }

        public T AddFirstItem<T>(GameObject itemPrefab) where T : MonoBehaviour
        {
            if (_pages[0].IsFinished)
            {
                ShiftItemsRight();
            }
            
            var createProjectItem = _pages[0].PopFirst();
            var item = _pages[0].AddFirst(itemPrefab);
            _pages[0].PushFirst(createProjectItem);

            return item.GetComponent<T>();
        }
        
        public T AddLastItem<T>(GameObject itemPrefab) where T : MonoBehaviour
        {
            if (_lastPage.IsFinished)
            {
                _lastPage = AddPage();
            }

            var item = _lastPage.AddLast(itemPrefab);

            return item.GetComponent<T>();
        }

        public void SetAsFirstItem(RectTransform item)
        {
            if (_pages[0].IsFinished && !_pages[0].Contains(item))
            {
                ShiftItemsRight();
            }
            
            var createProjectItem = _pages[0].PopFirst();
            _pages[0].PushFirst(item);
            _pages[0].PushFirst(createProjectItem);
        }

        public void DeleteItem(RectTransform item)
        {
            var index = _pages.FindIndex(x => x.Contains(item));
            _pages[index].DeleteItem(item);

            if (index == _pages.Count - 1)
            {
                return;
            }

            foreach (var page in _pages.Skip(index + 1)) // move first page item to the previous page
            {
                _pages[index].PushLast(page.PopFirst());
                index++;
            }

            if (!_lastPage.IsEmpty)
            {
                return;
            }

            _pages.Remove(_lastPage);
            Destroy(_lastPage.gameObject);
            _lastPage = _pages.Last();
        }

        private void ShiftItemsRight()
        {
            if (_lastPage.IsFinished)
            {
                _lastPage = AddPage();
            }
                
            var index = 0;
            foreach (var page in _pages.Skip(1))
            {
                page.PushFirst(_pages[index].PopLast());
                index++;
            }
        }

        private void OnSwipeDetected(Swipe direction, Vector2 swipeVelocity)
        {
            if (_pages.Count == 1)
            {
                return;
            }

            switch (direction)
            {
                case Swipe.Left:
                {
                    SwipeLeft();
                    break;
                }
                case Swipe.Right:
                {
                    SwipeRight();
                    break;
                }
            }
        }

        private Page AddPage()
        {
            var page = Instantiate(_pagePrefab, transform).GetComponent<Page>();
            page.Init(_maxItemsPerPage);
            _pages.Add(page);
            return page;
        }

        private void SwipeRight()
        {
            if (_openedPageIndex == 0)
            {
                return;
            }

            _openedPageIndex--;
            SwipePage();
        }

        private void SwipeLeft()
        {
            if (_openedPageIndex == _pages.Count - 1)
            {
                return;
            }

            _openedPageIndex++;
            SwipePage();
        }

        private void SwipePage()
        {
            _rectTransform.Move(_rectTransform.anchoredPosition, new Vector2(-_openedPageIndex * _swipeOffset, 0), _moveDuration,
                LeanTweenType.easeInOutCubic);
        }
    }
}