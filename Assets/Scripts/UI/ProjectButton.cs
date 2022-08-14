using System;
using System.Collections;
using System.Threading.Tasks;
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

        private Project _project;
        private Button _button;
        
        public RectTransform RectTransform { get;private set; }

        
        public void Init(Project project, UnityAction onSelected, UnityAction onDeleted)
        {
            _button = GetComponent<Button>();
            RectTransform = GetComponent<RectTransform>();

            _project = project;
            _name.text = _project.Name;
            
            _button.onClick.AddListener(onSelected);
            _deleteButton.onClick.AddListener(onDeleted);

            if (isActiveAndEnabled)
            {
                StartCoroutine(Setup());
            }
        }

        private void OnEnable()
        {
            StartCoroutine(Setup());
        }

        private IEnumerator Setup()
        {
            if (_project == null)
            {
                yield break;
            }
            
            yield return null;
            
            var imageWidth = _button.image.rectTransform.rect.size.x;
            var imageHeight = imageWidth * _project.Image.texture.height / _project.Image.texture.width;
            _image.rectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);
            _image.sprite = _project.Image;
            _project = null;
        }
    }
}