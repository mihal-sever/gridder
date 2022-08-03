using System.Collections.Generic;
using System.Threading.Tasks;
using Sever.Gridder.Data;
using UnityEngine;

namespace Sever.Gridder
{
    public static class ProjectManager
    {
        public static HashSet<Project> Projects { get; } = new ();


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
            var path = FileManager.ChooseImageFromDisk();
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            var sprite = await DataLoader.LoadSpriteFromDisk(path);
            var project = new Project(path, sprite);
            DataLoader.CreateProject(project, path);
            EventBus.OnProjectCreated(project);
            return true;
        }

        public static void AddProject(Project project, string imagePath)
        {
            DataLoader.CreateProject(project, imagePath);
            Projects.Add(project);
        }

        public static void DeleteProject(Project project)
        {
            Projects.Remove(project);
            DataLoader.DeleteProject(project);
            EventBus.OnProjectDeleted();
        }

        public static void SaveProject(Project project)
        {
            DataLoader.SaveProject(project);
        }
    }
}