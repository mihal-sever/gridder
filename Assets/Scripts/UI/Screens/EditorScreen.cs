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

        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _saveButton;

        [Space, SerializeField] private ProjectSettingsPanel _settingsPanel;

        private Project _project;


        public void Init()
        {
            _settingsPanel.Close();
            
            _settingsButton.onClick.AddListener(_settingsPanel.Open);
            _saveButton.onClick.AddListener(() => ProjectManager.SaveProject(_project));

            EventBus.ProjectSelected += OpenProject;
            EventBus.ProjectDeleted += DeleteProject;
        }

        private void OnApplicationQuit()
        {
            ProjectManager.SaveProject(_project);
        }

        private void OpenProject(Project project)
        {
            if (_project == project)
            {
                return;
            }
            
            DeleteProject();
            _project = project;
            _gridController.SetProject(_project);
            _knobController.SetProject(_project);
            _settingsPanel.SetProject(_project, ApplySettings);
        }

        private void ApplySettings()
        {
            _gridController.UpdateGrid();
        }

        private void DeleteProject()
        {
            _gridController.Clear();
            _knobController.Clear();
            _project = null;
        }
    }
}