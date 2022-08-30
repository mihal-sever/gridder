using System;
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

        private readonly Dictionary<Project, ProjectButton> _projectButtons = new();


        public void Init()
        {
            _paginationController.Init();
            _paginationController.AddLastItem<Button>(_createProjectButtonPrefab).onClick.AddListener(() => CreateProject());
            EventBus.ProjectsLoaded += OnProjectsLoaded;
            EventBus.ProjectAdded += project => AddProject(project, true);
            EventBus.ProjectClosed += OnProjectClosed;
        }

        private void OnProjectsLoaded()
        {
            SetupProjectsView(ProjectManager.Projects);
        }

        private void SetupProjectsView(HashSet<Project> projects)
        {
            foreach (Project project in projects)
            {
                AddProject(project, false);
            }
        }

        private async Task CreateProject()
        {
            try
            {
                await BlurOverlay.Instance.Enable();
                bool isProjectCreated = await ProjectManager.CreateProject();
                BlurOverlay.Instance.Deactivate();
                if (!isProjectCreated)
                {
                    return;
                }

                ScreenController.OpenScreen<CreateProjectScreen>();
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n\n{e.StackTrace}");
            }
        }

        private void AddProject(Project project, bool addFirst = true)
        {
            var projectButton = addFirst
                ? _paginationController.AddFirstItem<ProjectButton>(_projectButtonPrefab)
                : _paginationController.AddLastItem<ProjectButton>(_projectButtonPrefab);
            
            projectButton.Init(project, SelectProject, DeleteProject);
            _projectButtons.Add(project, projectButton);

            void SelectProject()
            {
                EventBus.OnProjectOpened(project);
                ScreenController.OpenScreen<EditorScreen>();
            }

            void DeleteProject()
            {
                PopUp.Instance.ShowPopUp("Are you sure you want to delete the project?",
                    () =>
                {
                    ProjectManager.DeleteProject(project);
                    _projectButtons.Remove(project);
                    _paginationController.DeleteItem(projectButton.RectTransform);
                });
            }
        }

        private void OnProjectClosed(Project project)
        {
            _paginationController.SetAsFirstItem(_projectButtons[project].RectTransform);
            // _projectButtons.Remove(project);
            // _paginationController.DeleteItem(_projectButtons[project].RectTransform);
            //
            // AddProject(project, true);
        }
    }
}