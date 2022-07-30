using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.UI
{
    public class MainScreen : BaseScreen
    {
        [SerializeField] private TMP_InputField _canvasWidthInput;
        [SerializeField] private TMP_InputField _canvasHeightInput;
        [SerializeField] private TMP_InputField _gridStepInput;
        [SerializeField] private Button _apply;

        private float? _canvasWidth;
        private float? _canvasHeight;
        private int? _gridStep;


        protected override void Init()
        {
            gameObject.SetActive(false);

            _canvasWidthInput.characterValidation = TMP_InputField.CharacterValidation.Decimal;
            _canvasHeightInput.characterValidation = TMP_InputField.CharacterValidation.Decimal;
            _gridStepInput.characterValidation = TMP_InputField.CharacterValidation.Integer;

            _canvasWidthInput.onEndEdit.AddListener(OnEndEditCanvasWidth);
            _gridStepInput.onEndEdit.AddListener(OnEndEditGridStep);

            _apply.onClick.AddListener(Apply);


            EventBus.ImageChosen += _ => { gameObject.SetActive(true); };

            EventBus.ProjectLoaded += data =>
            {
                if (data.gridStepMm > 0)
                {
                    _gridStep = data.gridStepMm;
                }

                if (data.canvasWidth > 0)
                {
                    _canvasWidth = data.canvasWidth;
                }
            };

            EventBus.ImageSetup += () =>
            {
                if (_gridStep is { })
                {
                    SetGridStep(_gridStep.Value);
                }

                if (_canvasWidth is { })
                {
                    SetCanvasSize(_canvasWidth.Value);
                }

                Apply();
            };
        }

        private void OnEndEditCanvasWidth(string line)
        {
            float result;
            if (!float.TryParse(line, out result))
            {
                _canvasWidth = null;
                _canvasHeight = null;
                _canvasWidthInput.text = null;
                _canvasHeightInput.text = null;
                return;
            }

            SetCanvasSize(result);
        }

        private void SetCanvasSize(float canvasWidth)
        {
            _canvasWidth = Mathf.Clamp(canvasWidth, 50, 5000);

            ProjectData.PixelsPerMm = ProjectData.ImageWidth / _canvasWidth.Value;
            _canvasHeight = ProjectData.ImageHeight / ProjectData.PixelsPerMm;

            _canvasWidthInput.text = _canvasWidth.Value.ToString(CultureInfo.InvariantCulture);
            _canvasHeightInput.text = Mathf.RoundToInt(_canvasHeight.Value).ToString(CultureInfo.InvariantCulture);
        }

        private void OnEndEditGridStep(string line)
        {
            int result;
            if (!int.TryParse(line, out result))
            {
                _gridStep = null;
                _gridStepInput.text = null;
                return;
            }

            SetGridStep(result);
        }

        private void SetGridStep(int step)
        {
            _gridStep = Mathf.Clamp(step, 10, 100);
            _gridStepInput.text = _gridStep.Value.ToString(CultureInfo.InvariantCulture);
        }

        private void Apply()
        {
            if (!_canvasWidth.HasValue || !_canvasHeight.HasValue || !_gridStep.HasValue)
            {
                return;
            }

            ProjectData.CanvasWidth = _canvasWidth.Value;
            ProjectData.CanvasHeight = _canvasHeight.Value;
            ProjectData.GridStepMm = _gridStep.Value;

            EventBus.OnUserInputValidated();
        }
    }
}