using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GridMapper.UI
{
    [RequireComponent(typeof(Button))]
    public class KnobButton : MonoBehaviour
    {
        [SerializeField] private RectTransform _panel;
        [SerializeField] private TMP_Text _coordinates;

        [Space, SerializeField] private Color _defaultColor = Color.cyan;
        [SerializeField] private Color _selectedColor = Color.green;
        [SerializeField] private Color _finishedColor = Color.red;

        private Project _project;
        private readonly Vector3 _panelRightPosition = new Vector3(90, 8, 0);
        private readonly Vector3 _panelLeftPosition = new Vector3(-106, 8, 0);
        private float _maxPanelPositionX;
        private Vector2 _coordToClosestNode;

        private Action<KnobButton> OnSelected;
        private Action<KnobButton> OnDeleted;

        private Button _button;
        private Button Button => _button ??= GetComponent<Button>();

        private RectTransform _rectTransform;
        private RectTransform Rect => _rectTransform ??= GetComponent<RectTransform>();

        public Vector2 AnchoredPosition
        {
            get => Rect.anchoredPosition;
            private set => Rect.anchoredPosition = value;
        }


        private bool _isFinished;

        public bool IsFinished
        {
            get => _isFinished;
            private set
            {
                if (_isFinished == value)
                {
                    return;
                }

                _isFinished = value;
                Button.image.color = IsFinished ? _finishedColor : _selectedColor;
            }
        }


        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                {
                    return;
                }

                _isSelected = value;

                if (_isSelected)
                {
                    OnSelected?.Invoke(this);
                }

                ShowCoordinatesPanel();
                Button.image.color = IsFinished ? _finishedColor :
                    _isSelected ? _selectedColor : _defaultColor;
            }
        }


        private void Awake()
        {
            ShowCoordinatesPanel();
            Button.onClick.AddListener(OnClick);
        }

        private void Update()
        {
            if (!IsSelected)
            {
                return;
            }

            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                OnDeleted?.Invoke(this);
                return;
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                IsFinished = !IsFinished;
            }
        }


        public void Init(Project project, float maxPositionX, Action<KnobButton> onSelectedCallback, Action<KnobButton> onDeletedCallback)
        {
            _project = project;
            _maxPanelPositionX = maxPositionX;
            OnSelected = onSelectedCallback;
            OnDeleted = onDeletedCallback;
        }

        public void Init(Project project, bool isFinished, Vector2 anchoredPosition, float maxPositionX, Action<KnobButton> onSelectedCallback,
            Action<KnobButton> onDeletedCallback)
        {
            IsFinished = isFinished;
            AnchoredPosition = anchoredPosition;
            Init(project, maxPositionX, onSelectedCallback, onDeletedCallback);
        }

        private void OnClick()
        {
            IsSelected = !IsSelected;
        }

        private void ShowCoordinatesPanel()
        {
            _panel.gameObject.SetActive(_isSelected);

            if (!_isSelected)
            {
                return;
            }

            var coordPixel = new Vector2(Mathf.Abs(Rect.anchoredPosition.x), Mathf.Abs(Rect.anchoredPosition.y));
            var pixelsPerMm = _project.ImageWidth / _project.CanvasWidth;
            var coordMm = coordPixel / pixelsPerMm;
            _coordToClosestNode = new((int) coordMm.x % _project.GridStep, (int) coordMm.y % _project.GridStep);

            _panel.anchoredPosition = coordPixel.x < _maxPanelPositionX ? _panelRightPosition : _panelLeftPosition;
            _coordinates.text = $"x: {_coordToClosestNode.x}, y: {_coordToClosestNode.y}";
        }
    }
}