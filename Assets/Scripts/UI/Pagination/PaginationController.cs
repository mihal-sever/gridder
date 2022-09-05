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

        [Space, SerializeField] private Button _openPreviousPage;
        [SerializeField] private Button _openNextPage;

        private readonly List<Page> _pages = new();

        private RectTransform _rectTransform;
        private Page _lastPage;
        private int _openedPageIndex;
        private float _swipeOffset;


        public void Init()
        {
            _rectTransform = GetComponent<RectTransform>();
            _swipeOffset = GetComponent<HorizontalLayoutGroup>().spacing + _rectTransform.rect.size.x;

            _openPreviousPage.onClick.AddListener(OpenPrevious);
            _openNextPage.onClick.AddListener(OpenNext);

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
            UpdateOpenPageButtons();
            
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
            var index = _openedPageIndex;
            _pages[index].DeleteItem(item);

            if (index < _pages.Count - 1)
            {
                foreach (var page in _pages.Skip(index + 1)) // move first page item to the previous page
                {
                    _pages[index].PushLast(page.PopFirst());
                    index++;
                }
            }
            
            if (!_lastPage.IsEmpty)
            {
                return;
            }

            if (_openedPageIndex == _pages.Count - 1)
            {
                _openedPageIndex--;
                SwipePage();
            }

            _pages.Remove(_lastPage);
            Destroy(_lastPage.gameObject);
            _lastPage = _pages.Last();
            UpdateOpenPageButtons();
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
                    OpenNext();
                    break;
                }
                case Swipe.Right:
                {
                    OpenPrevious();
                    break;
                }
            }
        }

        private Page AddPage()
        {
            var page = Instantiate(_pagePrefab, transform).GetComponent<Page>();
            page.Init(_maxItemsPerPage);
            _pages.Add(page);
            UpdateOpenPageButtons();
            return page;
        }

        private void OpenPrevious()
        {
            if (_openedPageIndex == 0)
            {
                return;
            }

            _openedPageIndex--;
            SwipePage();
        }

        private void OpenNext()
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
            
            UpdateOpenPageButtons();
        }

        private void UpdateOpenPageButtons()
        {
            _openPreviousPage.gameObject.SetActive(_openedPageIndex > 0);
            _openNextPage.gameObject.SetActive(_openedPageIndex < _pages.Count - 1);
        }
    }
}