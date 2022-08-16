using System;
using Sever.Gridder.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.Gridder.Editor
{
    [RequireComponent(typeof(Button))]
    public class KnobButton : MonoBehaviour
    {
        [SerializeField] private KnobInfoPanel _infoPanel;

        [Space, SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Sprite _selectedSprite;
        [SerializeField] private Sprite _finishedSprite;

        private Button _button;
        private Button Button => _button ??= GetComponent<Button>();

        private RectTransform _rectTransform;
        private RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();

        private Action<KnobButton> _onSelected;
        private KnobColor _color;

        private bool _isFinished;

        private bool IsFinished
        {
            get => _isFinished;
            set
            {
                if (_isFinished == value)
                {
                    return;
                }

                _isFinished = value;
                UpdateSprite();
            }
        }

        private bool _isSelected;

        private bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                {
                    return;
                }

                _isSelected = value;
                UpdateSprite();
                _infoPanel.SetEnabled(_isSelected);
                _onSelected?.Invoke(_isSelected ? this : null);
            }
        }


        private void Init()
        {
            Button.onClick.AddListener(OnClick);
        }

        public void Init(Project project, KnobColor color, float maxPositionX, Action<KnobButton> onSelected)
        {
            Init();
            SetColor(color);
            _onSelected = onSelected;

            _infoPanel.Init(project, RectTransform.anchoredPosition.Abs(), maxPositionX);
        }

        public void Init(Project project, KnobDto knobDto, float maxPositionX, Action<KnobButton> onSelected)
        {
            IsFinished = knobDto.isFinished;
            RectTransform.anchoredPosition = new Vector2(knobDto.x, knobDto.y);
            Init(project, knobDto.color, maxPositionX, onSelected);
        }

        public void SetColor(KnobColor color)
        {
            _color = color;
            _defaultSprite = KnobColorSelector.GetSprite(_color);
            UpdateSprite();
        }

        public void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;
        }

        public void ToggleFinished()
        {
            IsFinished = !IsFinished;
        }

        public KnobDto GetKnobDto()
        {
            return new KnobDto
            {
                isFinished = IsFinished,
                color = _color,
                x = RectTransform.anchoredPosition.x,
                y = RectTransform.anchoredPosition.y
            };
        }

        private void OnClick()
        {
            IsSelected = !IsSelected;
        }

        private void UpdateSprite()
        {
            Button.image.sprite = IsFinished ? _finishedSprite :
                IsSelected ? _selectedSprite : _defaultSprite;
        }
    }
}