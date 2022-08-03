using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GridMapper
{
    public class GridController : MonoBehaviour, IInitializable
    {
        [SerializeField] private Image _image;
        [SerializeField] private Transform _grid;
        [SerializeField] private GameObject _gridPrefab;

        private const int CellsInGridPart = 5;
        private const float GridSizePixel = 250;

        private Sprite _sprite;
        private Project _project;
        

        public void Init()
        {
            EventBus.ProjectSelected += project => StartCoroutine(OpenProject(project));
        }

        private IEnumerator OpenProject(Project project)
        {
            if (_project != project)
            {
                // clear previous project
            }

            _project = project;
            _image.sprite = _project.Image;
            
            yield return null;
            
            float imageScaleFactor = GetComponent<RectTransform>().rect.size.x / _image.sprite.texture.width;
            _image.rectTransform.sizeDelta = new Vector2(0, _image.sprite.texture.height * imageScaleFactor);

            _project.UpdateImageSize(_image.rectTransform.rect.size);
        }

        private void CreateGrid()
        {
            var _canvasWidth = _project.CanvasWidth;

            var gridSizeMm = _project.GridStep * CellsInGridPart;
            var gridSizePixel = GridSizePixel;

            int horizontalGrids = Mathf.CeilToInt(_canvasWidth / gridSizeMm);
            int verticalGrids = Mathf.CeilToInt(_project.CanvasHeight / gridSizeMm);

            var gridScaleFactor = _project.ImageWidth / (_canvasWidth / gridSizeMm * gridSizePixel);
            _grid.localScale = Vector3.one * gridScaleFactor;

            for (int i = 0; i < horizontalGrids; i++)
            {
                for (int j = 0; j < verticalGrids; j++)
                {
                    var newGrid = Instantiate(_gridPrefab, _grid).GetComponent<RectTransform>();
                    newGrid.anchoredPosition = new Vector2(gridSizePixel * i, -gridSizePixel * j);
                }
            }
        }
    }
}