using Sever.Gridder.Data;
using TMPro;
using UnityEngine;

namespace Sever.Gridder.Editor
{
    [RequireComponent(typeof(RectTransform))]
    public class KnobInfoPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coordinates;
     
        private readonly Vector3 _panelPosition = new(100, 0, 0);
        private float _maxPositionX;

        private Project _project;
        private Vector2 _knobCoordPixel;
        private RectTransform _rect;
        

        public void Init(Project project, Vector2 knobCoordPixel, float maxPositionX)
        {
            _rect = GetComponent<RectTransform>();

            _project = project;
            _knobCoordPixel = knobCoordPixel;
            _maxPositionX = maxPositionX;

            transform.localScale = new Vector3(0, 1, 1);
        }

        public void SetEnabled(bool isEnabled)
        {
            LeanTween.scaleX(gameObject, isEnabled ? 1 : 0, .3f).setEase(LeanTweenType.easeInOutCubic);
            
            if (!isEnabled)
            {
                return;
            }

            var coordMm = _knobCoordPixel / _project.PixelsPerMm;
            Vector2Int coordToClosestNode = new((int) coordMm.x % _project.GridStep, (int) coordMm.y % _project.GridStep);
            _rect.anchoredPosition = _knobCoordPixel.x < _maxPositionX ? _panelPosition : -_panelPosition;
            _coordinates.text = $"x: {coordToClosestNode.x}, y: {coordToClosestNode.y}";
        }
        
    }
}