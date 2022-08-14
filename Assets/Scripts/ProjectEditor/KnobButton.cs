using System;
using Sever.Gridder.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.Gridder.Editor
{
    [RequireComponent(typeof(Button))]
    public class KnobButton : MonoBehaviour
    {
        [SerializeField] private RectTransform _panel;
        [SerializeField] private TMP_Text _coordinates;

        [Space, SerializeField] private Color _defaultColor = Color.cyan;
        [SerializeField] private Color _selectedColor = Color.green;
        [SerializeField] private Color _finishedColor = Color.red;

        private readonly Vector3 _panelRightPosition = new(90, 8, 0);
        private readonly Vector3 _panelLeftPosition = new(-106, 8, 0);
        private float _maxPanelPositionX;
        
        private Project _project;
        private Button _button;
        private RectTransform _rectTransform;

        private Action<KnobButton> _onSelected;

        private bool _isFinished;
        private bool _isSelected;

        
        private void Init()
        {
            _button ??= GetComponent<Button>();
            _rectTransform ??= GetComponent<RectTransform>();

            _button.onClick.AddListener(OnClick);
            ShowCoordinatesPanel();
        }

        public void Init(Project project, float maxPositionX, Action<KnobButton> onSelected)
        {
            Init();
            _project = project;
            _maxPanelPositionX = maxPositionX;
            _onSelected = onSelected;
        }

        public void Init(Project project, bool isFinished, Vector2 anchoredPosition, float maxPositionX, Action<KnobButton> onSelected)
        {
            // Init();
            Init(project, maxPositionX, onSelected);
            _isFinished = isFinished;
            _button.image.color = _isFinished ? _finishedColor : _defaultColor;
            _rectTransform.anchoredPosition = anchoredPosition;
        }

        public KnobDto GetKnobDto()
        {
            return new KnobDto
            {
                isFinished = _isFinished,
                x = _rectTransform.anchoredPosition.x,
                y = _rectTransform.anchoredPosition.y
            };
        }

        public void SetSelected(bool isSelected)
        {
            if (_isSelected == isSelected)
            {
                return;
            }

            _isSelected = isSelected;
            _onSelected?.Invoke(this);
                
            ShowCoordinatesPanel();
            _button.image.color = _isFinished ? _finishedColor :
                _isSelected ? _selectedColor : _defaultColor;
        }

        public void ToggleFinished()
        {
            _isFinished = !_isFinished;
            _button.image.color = _isFinished ? _finishedColor : _selectedColor;
        }

        private void OnClick()
        {
            SetSelected(!_isSelected);
        }

        private void ShowCoordinatesPanel()
        {
            _panel.gameObject.SetActive(_isSelected);

            if (!_isSelected)
            {
                return;
            }

            var coordPixel = new Vector2(Mathf.Abs(_rectTransform.anchoredPosition.x), Mathf.Abs(_rectTransform.anchoredPosition.y));
            var coordMm = coordPixel / _project.PixelsPerMm;
            Vector2Int coordToClosestNode = new((int) coordMm.x % _project.GridStep, (int) coordMm.y % _project.GridStep);
            _panel.anchoredPosition = coordPixel.x < _maxPanelPositionX ? _panelRightPosition : _panelLeftPosition;
            _coordinates.text = $"x: {coordToClosestNode.x}, y: {coordToClosestNode.y}";
        }
    }
}