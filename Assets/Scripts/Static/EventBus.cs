using System;
using Sever.Gridder.Data;
using Sever.Gridder.UI;

namespace Sever.Gridder
{
    public static class EventBus
    {
        public static event Action ProjectsLoaded;
        public static event Action<Project> ProjectSelected;
        public static event Action ProjectDeleted;
        public static event Action<Project> ProjectCreated;
        public static event Action<Project> ProjectAdded;


        public static void OnProjectsLoaded()
        {
            ScreenController.OpenScreen<ProjectSelectionScreen>();
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

        public static void OnProjectAdded(Project project)
        {
            ProjectAdded?.Invoke(project);
        }
    }
}