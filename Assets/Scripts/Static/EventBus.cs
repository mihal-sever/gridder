using System;
using Sever.Gridder.Data;

namespace Sever.Gridder
{
    public static class EventBus
    {
        public static event Action ProjectsLoaded;
        public static event Action<Project> ProjectSelected;
        public static event Action ProjectDeleted;
        public static event Action<Project> ProjectCreated;
        public static event Action<ProjectDto> ProjectLoaded;


        public static void OnProjectsLoaded()
        {
            ProjectsLoaded?.Invoke();
        }

        public static void OnProjectSelected(Project project)
        {
            ProjectSelected?.Invoke(project);
        }
        
        public static void OnProjectDeleted()
        {
            ProjectDeleted?.Invoke();
        }

        public static void OnProjectCreated(Project project)
        {
            ProjectCreated?.Invoke(project);
        }

        public static void OnProjectLoaded(ProjectDto projectDto)
        {
            ProjectLoaded?.Invoke(projectDto);
        }
    }
}