using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sever.Gridder.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace Sever.Gridder
{
    public static class DataLoader
    {
        public static async Task<Sprite> LoadSpriteFromDisk(string path)
        {
            if (!File.Exists(path))
            {
                throw new IOException($"Requested file does not exist: {path}");
            }

            using var request = UnityWebRequestTexture.GetTexture(new Uri(path));
            await request.SendWebRequest();
            var texture = DownloadHandlerTexture.GetContent(request);
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));

            return sprite;
        }

        public static void CreateProject(Project project, string imagePath)
        {
            var projectDirectory = GetProjectDirectory(project.Guid);
            if (Directory.Exists(projectDirectory))
            {
                return;
            }
            
            Directory.CreateDirectory(projectDirectory);
            
            var newImagePath = GetImagePath(project.Guid, project.ImageExtension);
            File.Copy(imagePath, newImagePath);
            
            SaveProject(project);
        }

        public static void SaveProject(Project project)
        {
            var projectDto = new ProjectDto
            {
                guid = project.Guid,
                name = project.Name,
                gridStep = project.GridStep,
                canvasWidth = project.CanvasWidth,
                imageExtension = project.ImageExtension,
                knobs = project.Knobs
            };
            
            var json = JsonConvert.SerializeObject(projectDto, Formatting.Indented);
            File.WriteAllTextAsync(GetDataPath(project.Guid), json);
        }

        public static void DeleteProject(Project project)
        {
            var projectDirectory = GetProjectDirectory(project.Guid);
            if (!Directory.Exists(projectDirectory))
            {
                return;
            }
            Directory.Delete(projectDirectory, true);
        }
        
        public static async Task<Project> LoadProject(string projectGuid)
        {
            var dataPath = GetDataPath(projectGuid);
            if (!File.Exists(dataPath))
            {
                throw new IOException($"Requested file does not exist: {dataPath}");
            }

            var json = await File.ReadAllTextAsync(dataPath);
            var projectDto = JsonUtility.FromJson<ProjectDto>(json);
            var imagePath = GetImagePath(projectGuid, projectDto.imageExtension);
            var sprite = await LoadSpriteFromDisk(imagePath);
            var project = new Project(projectDto, sprite);

            return project;
        }

        public static async Task LoadProjects()
        {
            var projects = new List<Project>();
            var directories = Directory.EnumerateDirectories(Application.persistentDataPath);
            foreach (string directory in directories)
            {
                var projectGuid = directory.Split('/').Last();
                var project = await LoadProject(projectGuid);
                projects.Add(project);
            }

            ProjectManager.SetProjects(projects);
        }

        private static string GetProjectDirectory(string projectGuid) => Path.Combine(Application.persistentDataPath, projectGuid);
        private static string GetDataPath(string projectGuid) => Path.Combine(GetProjectDirectory(projectGuid), "data.json");
        private static string GetImagePath(string projectGuid, string ext) => Path.Combine(GetProjectDirectory(projectGuid), $"image.{ext}");
    }
}