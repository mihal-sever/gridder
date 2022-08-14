using Sever.Gridder.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.Gridder.UI
{
    public class CreateProjectScreen : BaseScreen, IInitializable
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Image _previewImage;
        [SerializeField] private Button _changeImageButton;
        [SerializeField] private ProjectSettingsPanel _settingsPanel;

        private Sprite _sprite;
        private Project _project;


        public void Init()
        {
            _previewImage.preserveAspect = true;
            _previewImage.type = Image.Type.Simple;

            _closeButton.onClick.AddListener(ScreenController.OpenScreen<ProjectSelectionScreen>);
            _changeImageButton.onClick.AddListener(ChooseImage);
            
            EventBus.ProjectCreated += OnProjectCreated;
        }

        private void OnProjectCreated(Project project)
        {
            _project = project;
            _settingsPanel.SetProject(_project, ApplySettings);

            _sprite = _project.Image;
            _previewImage.sprite = _sprite;
            _previewImage.gameObject.SetActive(true);
        }

        private void ApplySettings()
        {
            ProjectManager.AddProject(_project);
            EventBus.OnProjectSelected(_project);
            ScreenController.OpenScreen<EditorScreen>();
        }

        private void ChooseImage()
        {
            DownloadImage();
        }

        private async void DownloadImage()
        {
            var path = TextureUtility.ChooseImageFromDisk();
            if (path == null)
            {
                return;
            }

            _sprite = await DataLoader.LoadSpriteFromDisk(path);
            _previewImage.sprite = _sprite;
            _previewImage.gameObject.SetActive(true);
            _project.UpdateImage(_sprite);
            _settingsPanel.UpdateCanvasSize(_sprite.texture.width, _sprite.texture.height);
        }

        private void Reset()
        {
            _sprite = null;
            _previewImage.sprite = null;
            _previewImage.gameObject.SetActive(false);
        }
    }
}