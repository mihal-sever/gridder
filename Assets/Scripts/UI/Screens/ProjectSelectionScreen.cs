using System.Collections.Generic;
using System.Threading.Tasks;
using Sever.Gridder.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.Gridder.UI
{
    public class ProjectSelectionScreen : BaseScreen, IInitializable
    {
        [SerializeField] private PaginationController _paginationController;
        [SerializeField] private GameObject _projectButtonPrefab;
        [SerializeField] private GameObject _createProjectButtonPrefab;

        private readonly HashSet<ProjectButton> _projectButtons = new();


        public void Init()
        {
            _paginationController.Init();
            _paginationController.AddItem<Button>(_createProjectButtonPrefab).onClick.AddListener(() => CreateProject());
            EventBus.ProjectsLoaded += OnProjectsLoaded;
        }

        private void OnProjectsLoaded()
        {
            SetupProjectsView(ProjectManager.Projects);
        }

        private void SetupProjectsView(HashSet<Project> projects)
        {
            foreach (Project project in projects)
            {
                AddProject(project);
            }
        }

        private async Task CreateProject()
        {
            await BlurController.Instance.Enable();
            bool isProjectCreated = await ProjectManager.CreateProject();
            BlurController.Instance.Deactivate();
            if (!isProjectCreated)
            {
                return;
            }

            ScreenController.OpenScreen<CreateProjectScreen>();
        }

        private void AddProject(Project project)
        {
            var projectButton = _paginationController.AddItem<ProjectButton>(_projectButtonPrefab);
            projectButton.Init(project, SelectProject, DeleteProject);
            _projectButtons.Add(projectButton);

            void SelectProject()
            {
                EventBus.OnProjectSelected(project);
            }

            void DeleteProject()
            {
                PopUp.Instance.ShowPopUp(() =>
                {
                    ProjectManager.DeleteProject(project);
                    _projectButtons.Remove(projectButton);
                    _paginationController.DeleteItem(projectButton.RectTransform);
                });
            }
        }
    }
}