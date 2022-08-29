using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sever.Gridder.UI
{
    [RequireComponent(typeof(Image))]
    public class GridderButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _defaultTextColor;

        [Space, SerializeField] private Color _selectedColor;
        [SerializeField] private Color _selectedTextColor;

        private Image _image;
        private TMP_Text _text;


        private void Awake()
        {
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<TMP_Text>();
        }

        private void OnDisable()
        {
            _image.color = _defaultColor;
            if (_text)
            {
                _text.color = _defaultTextColor;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _image.color = _selectedColor;
            if (_text)
            {
                _text.color = _selectedTextColor;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _image.color = _defaultColor;
            if (_text)
            {
                _text.color = _defaultTextColor;
            }
        }
    }
}