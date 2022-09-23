using System.Collections;
using System.Collections.Generic;
using Sever.Gridder.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.Gridder.Editor
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Transform _grid;
        [SerializeField] private GameObject _verticalLinePrefab;
        [SerializeField] private GameObject _horizontalLinePrefab;

        private Project _project;
        private readonly List<GameObject> _verticalLines = new();
        private readonly List<GameObject> _horizontalLines = new();
        private Vector2 _imageSize;
        

        public void Clear()
        {
            foreach (GameObject gridPart in _verticalLines)
            {
                Destroy(gridPart);
            }

            foreach (GameObject gridPart in _horizontalLines)
            {
                Destroy(gridPart);
            }

            _verticalLines.Clear();
            _horizontalLines.Clear();
        }

        public void SetProject(Project project)
        {
            if (project != null && project == _project)
            {
                return;
            }  
            
            _project = project;
            StartCoroutine(Setup());
        }

        private IEnumerator Setup()
        {
            _image.sprite = _project.Image;

            yield return null;

            float imageScaleFactor = GetComponent<RectTransform>().rect.size.x / _image.sprite.texture.width;
            _image.rectTransform.sizeDelta = new Vector2(0, _image.sprite.texture.height * imageScaleFactor);
            _imageSize = _image.rectTransform.rect.size;
            CreateGrid();
        }

        public void UpdateGrid()
        {
            Clear();
            CreateGrid();
        }

        private void CreateGrid()
        {
            _project.PixelsPerMm = _imageSize.x / _project.CanvasWidth;
            var gridStepPixel = _project.GridStep * _project.PixelsPerMm;
            int horizontalCellsCount = Mathf.CeilToInt(_imageSize.x / gridStepPixel);
            int verticalCellsCount = Mathf.CeilToInt(_imageSize.y / gridStepPixel);

            for (int i = 1; i < horizontalCellsCount; i++)
            {
                var line = Instantiate(_verticalLinePrefab, _grid).GetComponent<RectTransform>();
                line.anchoredPosition = new Vector2(gridStepPixel * i, 0);
                _verticalLines.Add(line.gameObject);
            }

            for (int i = 1; i < verticalCellsCount; i++)
            {
                var line = Instantiate(_horizontalLinePrefab, _grid).GetComponent<RectTransform>();
                line.anchoredPosition = new Vector2(0, -gridStepPixel * i);
                _horizontalLines.Add(line.gameObject);
            }
        }
    }
}