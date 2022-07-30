using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.UI
{
    public class OpenFileScreen : BaseScreen
    {
        [SerializeField] private Image _previewImage;
        [SerializeField] private TMP_Text _imagePathText;
        [SerializeField] private Button _chooseImageButton;
        [SerializeField] private Button _submitButton;

        private Sprite _sprite;
        

        protected override void Init()
        {
            EventBus.ProjectLoaded += data =>
            {
                if (data == null)
                {
                    _previewImage.gameObject.SetActive(false);
                    _submitButton.interactable = false;
                }
                else
                {
                    _submitButton.interactable = true;
                    DownloadImage(data.imagePath);
                }
            };

            _chooseImageButton.onClick.AddListener(ChooseImage);
            _submitButton.onClick.AddListener(Submit);
        }
        
        private void Start()
        {
            FileManager.LoadProject();
        }

        private void ChooseImage()
        {
            var extensions = new[]
            {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg"),
            };
            var filePaths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);

            if (filePaths.Length == 0)
            {
                return;
            }

            DownloadImage(filePaths[0]);
        }

        private void Submit()
        {
            EventBus.OnImageChosen(_sprite);
            ScreenController.OpenScreen<MainScreen>();
        }

        private async void DownloadImage(string path)
        {
            _sprite = await FileManager.LoadSpriteFromDisk(path);
            _previewImage.sprite = _sprite;
            _previewImage.preserveAspect = true;
            _previewImage.gameObject.SetActive(true);
            _imagePathText.text = path;
            _submitButton.interactable = true;
        }
    }
}