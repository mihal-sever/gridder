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
        private string _path;
        private Project _project;


        public void Init()
        {
            _previewImage.preserveAspect = true;

            _closeButton.onClick.AddListener(ScreenController.OpenScreen<ProjectSelectionScreen>);
            _changeImageButton.onClick.AddListener(ChooseImage);
            
            EventBus.ProjectCreated += OnProjectCreated;
        }

        private void OnProjectCreated(Project project)
        {
            _project = project;
            _settingsPanel.SetProject(_project, ApplySettings);
        }

        private void ApplySettings()
        {
            ProjectManager.AddProject(_project, _path);
            EventBus.OnProjectSelected(_project);
            ScreenController.OpenScreen<EditorScreen>();
        }

        private void ChooseImage()
        {
            _path = FileManager.ChooseImageFromDisk();
            if (_path == null)
            {
                return;
            }

            DownloadImage();
            _project.UpdateImage(_path, _sprite, _previewImage.rectTransform.rect.size);
        }

        private async void DownloadImage()
        {
            _sprite = await DataLoader.LoadSpriteFromDisk(_path);
            _previewImage.sprite = _sprite;
            _previewImage.gameObject.SetActive(true);
        }

        private void Reset()
        {
            _sprite = null;
            _path = null;
            _previewImage.sprite = null;
            _previewImage.gameObject.SetActive(false);
        }
    }
}