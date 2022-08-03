using Sever.Gridder.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sever.Gridder.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Button))]
    public class ProjectButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _image;
        [SerializeField] private Button _deleteButton;

        private Button _button;
        
        public RectTransform RectTransform { get;private set; }

        
        public void Init(Project project, UnityAction onSelected, UnityAction onDeleted)
        {
            _button = GetComponent<Button>();
            RectTransform = GetComponent<RectTransform>();

            _name.text = project.Name;
            
            var imageWidth = _button.image.rectTransform.rect.size.x;
            var imageHeight = imageWidth * project.Image.texture.height / project.Image.texture.width;
            _image.rectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);
            _image.sprite = project.Image;

            _button.onClick.AddListener(onSelected);
            _deleteButton.onClick.AddListener(onDeleted);
        }
    }
}