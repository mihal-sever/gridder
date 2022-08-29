using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sever.Gridder.UI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class GridderButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _defaultTextColor;

        [Space, SerializeField] private Color _selectedColor;
        [SerializeField] private Color _selectedTextColor;

        private Button _button;
        private Image _image;
        private TMP_Text _text;


        private void Awake()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<TMP_Text>();
        }

        private void OnDisable()
        {
            SetDefault();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_button.interactable)
            {
                return;
            }
            
            _image.color = _selectedColor;
            if (_text)
            {
                _text.color = _selectedTextColor;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_button.interactable)
            {
                return;
            }
            
            SetDefault();
        }

        private void SetDefault()
        {
            _image.color = _defaultColor;
            if (_text)
            {
                _text.color = _defaultTextColor;
            }
        }
    }
}