using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Transform _grid;
    [SerializeField] private GameObject _gridPrefab;

    private const int CellsInGridPart = 5;
    private const float GridSizePixel = 250;

    private Sprite _sprite;
    

    private void Awake()
    {
        EventBus.UserInputValidated += CreateGrid;
    }

    private IEnumerator Start()
    {
        _image.sprite = ProjectData.Image;

        yield return null;

        float imageScaleFactor = GetComponent<RectTransform>().rect.size.x / _image.sprite.texture.width;
        _image.rectTransform.sizeDelta = new Vector2(0, _image.sprite.texture.height * imageScaleFactor);
        
        ProjectData.ImageWidth = _image.rectTransform.rect.size.x;
        ProjectData.ImageHeight = _image.rectTransform.rect.size.y;
        
        EventBus.OnImageSetup();
    }

    private void CreateGrid()
    {
        var _canvasWidth = ProjectData.CanvasWidth;

        var gridSizeMm = ProjectData.GridStepMm * CellsInGridPart;
        var gridSizePixel = GridSizePixel;

        int horizontalGrids = Mathf.CeilToInt(_canvasWidth / gridSizeMm);
        int verticalGrids = Mathf.CeilToInt(ProjectData.CanvasHeight / gridSizeMm);

        var gridScaleFactor = ProjectData.ImageWidth / (_canvasWidth / gridSizeMm * gridSizePixel);
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