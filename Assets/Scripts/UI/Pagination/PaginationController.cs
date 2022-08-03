using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        private Page _currentPage;
        private int _openedPageIndex;


        public void Init()
        {
            _rectTransform = GetComponent<RectTransform>();
            _currentPage = AddPage();
        }

        private void OnEnable()
        {
            SwipeManager.OnSwipeDetected += OnSwipeDetected;
        }

        private void OnDisable()
        {
            SwipeManager.OnSwipeDetected -= OnSwipeDetected;
        }

        public T AddItem<T>(GameObject itemPrefab) where T : MonoBehaviour
        {
            if (_currentPage.IsFinished)
            {
                _currentPage = AddPage();
            }

            var item = _currentPage.AddItem(itemPrefab);

            return item.GetComponent<T>();
        }

        public void DeleteItem(RectTransform item)
        {
            var index = _pages.FindIndex(x => x.Contains(item));
            _pages[index].DeleteItem(item);

            if (index == _pages.Count - 1)
            {
                return;
            }

            foreach (var page in _pages.Skip(index + 1))
            {
                _pages[index].Push(page.Pop());
                index++;
            }

            if (!_currentPage.IsEmpty)
            {
                return;
            }

            _pages.Remove(_currentPage);
            Destroy(_currentPage.gameObject);
            _currentPage = _pages.Last();
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
            _rectTransform.Move(_rectTransform.anchoredPosition, new Vector2(-_openedPageIndex * _rectTransform.rect.size.x, 0), _moveDuration,
                LeanTweenType.easeInOutCubic);
        }
    }
}