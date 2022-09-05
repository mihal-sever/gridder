using System.Collections.Generic;
using System.Threading.Tasks;
using Sever.Gridder.Data;

namespace Sever.Gridder
{
    public static class ProjectManager
    {
        public static HashSet<Project> Projects { get; } = new();


        public static void SetProjects(List<Project> projects)
        {
            Projects.Clear();
            foreach (Project project in projects)
            {
                Projects.Add(project);
            }

            if (projects.Count == 0)
            {
                return;
            }

            EventBus.OnProjectsLoaded();
        }

        public static async Task<bool> CreateProject()
        {
            var path = TextureUtility.ChooseImageFromDisk();
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            var sprite = await DataLoader.LoadSpriteFromDisk(path);
            var project = new Project(sprite);
            EventBus.OnProjectCreated(project);
            return true;
        }

        public static void AddProject(Project project)
        {
            DataLoader.CreateProject(project);
            Projects.Add(project);
            EventBus.OnProjectAdded(project);
        }

        public static void DeleteProject(Project project)
        {
            Projects.Remove(project);
            DataLoader.DeleteProject(project);
            EventBus.OnProjectDeleted();
        }
    }
}