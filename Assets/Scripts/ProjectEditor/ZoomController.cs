using System.Collections;
using UnityEngine;

namespace GridMapper.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class ZoomController : MonoBehaviour
    {
        [SerializeField] private float _zoomSpeed = .5f;
        [SerializeField] private float _maxZoom = 3f;

        private float _minZoom = 1;
        private RectTransform _rectTransform;

        private void Start()
        {
            StartCoroutine(Init());
        }


        private IEnumerator Init()
        {
            yield return null;
            yield return null;

            _rectTransform = GetComponent<RectTransform>();
            var viewPortSize = GetComponentInParent<GridController>().GetComponent<RectTransform>().rect.size;
            _minZoom = viewPortSize.y / _rectTransform.rect.size.y;
        }

        private void Update()
        {
            if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
            {
                return;
            }

            if (Input.mouseScrollDelta.y == 0)
            {
                return;
            }

            var scale = Mathf.Clamp(_rectTransform.localScale.y - Input.mouseScrollDelta.y * _zoomSpeed, _minZoom, _maxZoom);
            _rectTransform.localScale = new Vector3(scale, scale, scale);
        }
    }
}