using Sever.Gridder.Data;
using Sever.Gridder.Editor;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.Gridder.UI
{
    public class EditorScreen : BaseScreen, IInitializable
    {
        [SerializeField] private GridController _gridController;
        [SerializeField] private KnobController _knobController;
        
        [Space, SerializeField] private Button _settingsButton;
        [SerializeField] private ProjectSettingsPanel _settingsPanel;
        
        [Space, SerializeField] private Button _saveButton;
        [SerializeField] private CanvasGroup _savedConfirmation;

        [Space, SerializeField] private Button _backButton;

        private Project _project;


        public void Init()
        {
            _savedConfirmation.gameObject.SetActive(false);
            _settingsPanel.Close();
            
            _settingsButton.onClick.AddListener(OpenSettings);
            _saveButton.onClick.AddListener(() => ProjectManager.SaveProject(_project, OnSaved));
            _backButton.onClick.AddListener(CloseProject);
            
            EventBus.ProjectSelected += OpenProject;
            EventBus.ProjectDeleted += Clear;
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        private void OpenSettings()
        {
            _knobController.SelectKnob(null);
            _settingsPanel.Open();
        }

        private void OnSaved()
        {
            LeanTween.alphaCanvas(_savedConfirmation, 1, 0);
            _savedConfirmation.gameObject.SetActive(true);

            LeanTween.alphaCanvas(_savedConfirmation, 0, 1f).setOnComplete(() =>
            {
                _savedConfirmation.gameObject.SetActive(false);
            });
    }
        
        private void OpenProject(Project project)
        {
            if (_project == project)
            {
                return;
            }
            
            Clear();
            _project = project;
            _gridController.SetProject(_project);
            _knobController.SetProject(_project);
            _settingsPanel.SetProject(_project, ApplySettings);
        }

        private void ApplySettings()
        {
            _gridController.UpdateGrid();
        }

        private void Clear()
        {
            _gridController.Clear();
            _knobController.Clear();
            _project = null;
        }

        private void CloseProject()
        {
            Save();
            _settingsPanel.Close();
            ScreenController.OpenScreen<ProjectSelectionScreen>();
        }

        private void Save()
        {
            _knobController.Save();
            DataLoader.SaveProject(_project);
        }
    }
}