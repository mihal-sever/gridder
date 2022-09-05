using System;
using Sever.Gridder.Data;
using Sever.Gridder.Editor;
using Sever.Gridder.UI;

namespace Sever.Gridder
{
    public static class EventBus
    {
        public static event Action ProjectsLoaded;
        public static event Action<Project> ProjectOpened;
        public static event Action<Project> ProjectClosed;
        public static event Action ProjectDeleted;
        public static event Action<Project> ProjectCreated;
        public static event Action<Project> ProjectAdded;
        public static event Action<KnobColor> ColorSelected;
        

        public static void OnProjectsLoaded()
        {
            ScreenController.OpenScreen<ProjectSelectionScreen>(.5f);
            ProjectsLoaded?.Invoke();
        }

        public static void OnProjectOpened(Project project)
        {
            ProjectOpened?.Invoke(project);
        }
        
        public static void OnProjectClosed(Project project)
        {
            ProjectClosed?.Invoke(project);
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

        public static void OnColorSelected(KnobColor color)
        {
            ColorSelected?.Invoke(color);
        }
    }
}