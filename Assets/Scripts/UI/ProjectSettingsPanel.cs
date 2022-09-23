using System;
using System.Globalization;
using Sever.Gridder.Data;
using Sever.Gridder.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.Gridder.UI
{
    public class ProjectSettingsPanel : MonoBehaviour, IInitializable
    {
        [SerializeField] private TMP_InputField _projectNameInput;
        [SerializeField] private TMP_InputField _canvasWidthInput;
        [SerializeField] private TMP_InputField _canvasHeightInput;
        [SerializeField] private TMP_InputField _gridStepInput;
        [SerializeField] private Button _apply;


        private SliderScreen _sliderScreen;
        private Action _onApplied;
        private Project _project;
        private string _name;
        private int? _canvasWidth;
        private int? _canvasHeight;
        private int? _gridStep;


        public void Init()
        {
            _sliderScreen = GetComponentInChildren<SliderScreen>(true);
            if (_sliderScreen)
            {
                _sliderScreen.Init(.5f, ScreenPosition.Right, ScreenPosition.Right);
            }

            _projectNameInput.characterValidation = TMP_InputField.CharacterValidation.None;
            _canvasWidthInput.characterValidation = TMP_InputField.CharacterValidation.Decimal;
            _canvasHeightInput.characterValidation = TMP_InputField.CharacterValidation.Decimal;
            _gridStepInput.characterValidation = TMP_InputField.CharacterValidation.Integer;

            _projectNameInput.onEndEdit.AddListener(OnEndEditName);
            _canvasWidthInput.onEndEdit.AddListener(OnEndEditCanvasWidth);
            _canvasHeightInput.onEndEdit.AddListener(OnEndEditCanvasHeight);
            _gridStepInput.onEndEdit.AddListener(OnEndEditGridStep);

            _apply.onClick.AddListener(Apply);
        }

        private void OnEnable()
        {
            _projectNameInput.ActivateInputField();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                _canvasWidthInput.ActivateInputField();
            }
        }

        public void SetProject(Project project, Action onApplied)
        {
            _project = project;
            _onApplied = onApplied;

            _name = _project.Name;
            _canvasWidth = _project.CanvasWidth;
            _canvasHeight = _project.CanvasHeight;
            _gridStep = _project.GridStep;

            _projectNameInput.text = _name;
            _canvasWidthInput.text = _canvasWidth.Value > 0 ? _canvasWidth.Value.ToString(CultureInfo.InvariantCulture) : null;
            _canvasHeightInput.text = _canvasHeight.Value > 0 ? _canvasHeight.Value.ToString(CultureInfo.InvariantCulture) : null;
            _gridStepInput.text = _gridStep.Value > 0 ? _gridStep.Value.ToString(CultureInfo.InvariantCulture) : null;

            ValidateInput();
        }

        public void Open()
        {
            if (_sliderScreen)
            {
                _sliderScreen.Open();
            }
        }

        public void Close()
        {
            if (_sliderScreen)
            {
                _sliderScreen.Close();
            }
        }

        public void UpdateCanvasSize(int width, int height)
        {
            if (_canvasWidth is not > 0)
            {
                return;
            }

            _canvasHeight = height / width * _canvasWidth.Value;
            _canvasHeightInput.text = _canvasHeight.Value.ToString(CultureInfo.InvariantCulture);
        }

        private void OnEndEditName(string value)
        {
            _name = value;
            ValidateInput();
        }

        private void OnEndEditCanvasWidth(string value)
        {
            int result;
            if (!int.TryParse(value, out result))
            {
                ResetCanvasSize();
                return;
            }

            _canvasWidth = Mathf.Clamp(result, 50, int.MaxValue);
            _canvasWidthInput.text = _canvasWidth.Value.ToString(CultureInfo.InvariantCulture);

            var pixelsPerMm = _project.Image.rect.size.x / _canvasWidth.Value;
            _canvasHeight = Mathf.RoundToInt(_project.Image.rect.size.y / pixelsPerMm);
            _canvasHeightInput.text = _canvasHeight.Value.ToString(CultureInfo.InvariantCulture);

            ValidateInput();
        }

        private void OnEndEditCanvasHeight(string value)
        {
            int result;
            if (!int.TryParse(value, out result))
            {
                ResetCanvasSize();
                return;
            }

            _canvasHeight = Mathf.Clamp(result, 50, int.MaxValue);
            _canvasHeightInput.text = _canvasHeight.Value.ToString(CultureInfo.InvariantCulture);

            var pixelsPerMm = _project.Image.rect.size.y / _canvasHeight.Value;
            _canvasWidth = Mathf.RoundToInt(_project.Image.rect.size.x / pixelsPerMm);
            _canvasWidthInput.text = _canvasWidth.Value.ToString(CultureInfo.InvariantCulture);

            ValidateInput();
        }

        private void OnEndEditGridStep(string value)
        {
            int result;
            if (!int.TryParse(value, out result))
            {
                _gridStep = null;
                _gridStepInput.text = null;
                return;
            }

            _gridStep = Mathf.Clamp(result, 10, int.MaxValue);
            _gridStepInput.text = _gridStep.Value.ToString(CultureInfo.InvariantCulture);

            ValidateInput();
        }

        private void Apply()
        {
            if (_sliderScreen)
            {
                _sliderScreen.Close();
            }

            var settings = new ProjectSettings()
            {
                name = _name,
                canvasWidth = _canvasWidth.Value,
                canvasHeight = _canvasHeight.Value,
                gridStep = _gridStep.Value
            };

            EditorController.ChangeSettings(_project, settings, _onApplied);
        }

        private void ValidateInput()
        {
            _apply.interactable = _name != null && _canvasWidth is > 0 && _gridStep is > 0;
        }

        private void ResetCanvasSize()
        {
            _canvasWidth = null;
            _canvasHeight = null;
            _canvasWidthInput.text = null;
            _canvasHeightInput.text = null;
        }
    }
}