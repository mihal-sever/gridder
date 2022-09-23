using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.Gridder.UI
{
    public enum ScreenPosition
    {
        Right,
        Left,
        None
    }

    public class SliderScreen : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        private float _transitionDuration;
        private Vector2 _fromPosition;
        private Vector2 _toPosition;
        private bool _enterCompleted;

        private RectTransform _rectTransform;
        private RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();

        private Canvas _canvas;
        private Canvas Canvas => _canvas ??= GetComponent<Canvas>();
        
        private Vector2 _offset;
        private Vector2 Offset
        {
            get
            {
                if (_offset == Vector2.zero)
                {
                    _offset = new Vector2(RectTransform.rect.size.x, 0);
                }

                return _offset;
            }
        }


        public void Init(float transitionDuration, ScreenPosition from, ScreenPosition to, Action closeCallback = null)
        {
            _transitionDuration = transitionDuration;
            if (_closeButton)
            {
                _closeButton.onClick.AddListener(() => Close(closeCallback));
            }

            _fromPosition = GetPosition(from);
            _toPosition = GetPosition(to);

            RectTransform.Move(RectTransform.anchoredPosition, _fromPosition);
            SetActive(false);
        }

        public void Open(Action callback = null)
        {
            float duration = _fromPosition == Vector2.zero ? 0 : _transitionDuration;
            _enterCompleted = false;
            SetActive(true);
            RectTransform.Move(RectTransform.anchoredPosition, Vector2.zero, duration, LeanTweenType.notUsed,
                () =>
                {
                    _enterCompleted = true;
                    callback?.Invoke();
                });
        }

        public void Close(Action callback = null)
        {
            float duration = _toPosition == Vector2.zero ? 0 : _transitionDuration;
            RectTransform.Move(RectTransform.anchoredPosition, _toPosition, duration, LeanTweenType.notUsed,
                () =>
                {
                    RectTransform.Move(RectTransform.anchoredPosition, _fromPosition);
                    SetActive(false);
                    callback?.Invoke();
                },
                _enterCompleted ? 0 : _transitionDuration);
        }

        private void SetActive(bool isActive)
        {
            if (Canvas)
            {
                if (!gameObject.activeSelf)
                {
                    gameObject.SetActive(true);
                }
                
                Canvas.enabled = isActive;
                return;
            }
            
            gameObject.SetActive(isActive);
        }

        private Vector2 GetPosition(ScreenPosition position)
        {
            return position switch
            {
                ScreenPosition.Right => Offset,
                ScreenPosition.Left => -Offset,
                ScreenPosition.None => Vector2.zero
            };
        }
    }
}