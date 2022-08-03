using System;
using System.Globalization;
using Sever.Gridder.Data;
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

        private Action _onApplied;
        private Project _project;
        private string _name;
        private float? _canvasWidth;
        private float? _canvasHeight;
        private int? _gridStep;


        public void Init()
        {
            gameObject.SetActive(false);

            _projectNameInput.characterValidation = TMP_InputField.CharacterValidation.None;
            _canvasWidthInput.characterValidation = TMP_InputField.CharacterValidation.Decimal;
            _canvasHeightInput.characterValidation = TMP_InputField.CharacterValidation.Decimal;
            _gridStepInput.characterValidation = TMP_InputField.CharacterValidation.Integer;

            _projectNameInput.onEndEdit.AddListener(OnEndEditName);
            _canvasWidthInput.onEndEdit.AddListener(OnEndEditCanvasWidth);
            _gridStepInput.onEndEdit.AddListener(OnEndEditGridStep);

            _apply.onClick.AddListener(Apply);
        }

        public void SetProject(Project project, Action onApplied)
        {
            _project = project;
            _onApplied = onApplied;
        }

        public void Open()
        {
        }

        public void Close()
        {
        }

        private void OnEndEditName(string value)
        {
            _name = value;
            ValidateInput();
        }

        private void OnEndEditCanvasWidth(string value)
        {
            float result;
            if (!float.TryParse(value, out result))
            {
                _canvasWidth = null;
                _canvasHeight = null;
                _canvasWidthInput.text = null;
                _canvasHeightInput.text = null;
                return;
            }

            _canvasWidth = Mathf.Clamp(result, 50, 5000);
            _canvasWidthInput.text = _canvasWidth.Value.ToString(CultureInfo.InvariantCulture);

            var pixelsPerMm = _project.ImageWidth / _canvasWidth.Value;
            _canvasHeight = _project.ImageHeight / pixelsPerMm;
            _canvasHeightInput.text = Mathf.RoundToInt(_canvasHeight.Value).ToString(CultureInfo.InvariantCulture);

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

            _gridStep = Mathf.Clamp(result, 10, 100);
            _gridStepInput.text = _gridStep.Value.ToString(CultureInfo.InvariantCulture);

            ValidateInput();
        }

        private void Apply()
        {
            _project.UpdateSettings(_name, _canvasWidth.Value, _canvasHeight.Value, _gridStep.Value);
            _onApplied?.Invoke();
        }

        private void ValidateInput()
        {
            _apply.interactable = _name != null && _canvasWidth is > 0 && _gridStep is > 0;
        }
    }
}