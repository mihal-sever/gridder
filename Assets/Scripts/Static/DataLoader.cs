using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sever.Gridder.Data;
using Sever.Gridder.UI;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace Sever.Gridder
{
    public static class DataLoader
    {

        private static readonly object _lockProgress = new();
        private static readonly List<Project> _projects = new();
        private static int _loadedProjects;
        private static int _projectsToLoad;
        
        public static async Task<Sprite> LoadSpriteFromDisk(string path, bool tryRotate = true)
        {
            if (!File.Exists(path))
            {
                throw new IOException($"Requested file does not exist: {path}");
            }

            using var request = UnityWebRequestTexture.GetTexture(new Uri(path));
            await request.SendWebRequest();
            var texture = DownloadHandlerTexture.GetContent(request);
            if (tryRotate)
            {
                texture = texture.Rotate(path);
            }

            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));

            return sprite;
        }

        public static void CreateProject(Project project)
        {
            var projectDirectory = GetProjectDirectory(project.Guid);
            if (Directory.Exists(projectDirectory))
            {
                return;
            }

            Directory.CreateDirectory(projectDirectory);

            var imagePath = GetImagePath(project.Guid);
            var textureBytes = project.Image.texture.EncodeToPNG();
            File.WriteAllBytes(imagePath, textureBytes);

            SaveProject(project);
        }

        public static async Task SaveProject(Project project)
        {
            var projectDto = new ProjectDto
            {
                guid = project.Guid,
                settings = project.Settings,
                knobs = project.Knobs
            };

            var json = JsonConvert.SerializeObject(projectDto, Formatting.Indented);
            await File.WriteAllTextAsync(GetDataPath(project.Guid), json);

            var projectDirectory = GetProjectDirectory(project.Guid);
            Directory.SetLastWriteTime(projectDirectory, DateTime.Now);
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

        public static async Task LoadProjects()
        {
            try
            {
                var directories = Directory.EnumerateDirectories(Application.persistentDataPath);
                _projectsToLoad = directories.Count();
                _loadedProjects = 0;
                var tasks = new List<Task>();
                
                if (_projectsToLoad == 0)
                {
                    ScreenController.OpenScreen<ProjectSelectionScreen>();
                }
                else
                {
                    ScreenController.OpenScreen<ProgressBar>();
                }

                foreach (string directory in directories.OrderByDescending(Directory.GetLastWriteTime))
                {
                    var projectGuid = directory.Split('/').Last();
                    tasks.Add(LoadProject(projectGuid));
                }

                await Task.WhenAll(tasks);
                ProjectManager.SetProjects(_projects);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n\n{e.StackTrace}");
            }
        }

        private static async Task LoadProject(string projectGuid)
        {
            var dataPath = GetDataPath(projectGuid);
            if (!File.Exists(dataPath))
            {
                throw new IOException($"Requested file does not exist: {dataPath}");
            }

            var json = await File.ReadAllTextAsync(dataPath);
            var projectDto = JsonConvert.DeserializeObject<ProjectDto>(json);
            var imagePath = GetImagePath(projectGuid);
            var sprite = await LoadSpriteFromDisk(imagePath, false);
            var project = new Project(projectDto, sprite);
            _projects.Add(project);
            
            lock (_lockProgress)
            {
                _loadedProjects++;
                ProgressBar.UpdateProgress((float) _loadedProjects / _projectsToLoad);
            }
        }
        
        private static string GetProjectDirectory(string projectGuid) => Path.Combine(Application.persistentDataPath, projectGuid);
        private static string GetDataPath(string projectGuid) => Path.Combine(GetProjectDirectory(projectGuid), "data.json");
        private static string GetImagePath(string projectGuid) => Path.Combine(GetProjectDirectory(projectGuid), "image.png");
    }
}