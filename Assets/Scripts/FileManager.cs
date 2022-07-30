using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public static class FileManager
{
    public static async Task<Sprite> LoadSpriteFromDisk(string path)
    {
        if (!File.Exists(path))
        {
            throw new IOException($"Requested file does not exist: {path}");
        }

        if (!path.Contains(Application.persistentDataPath))
        {
            var ext = path.Split('.').Last();
            var newPath = Path.Combine(Application.persistentDataPath, $"image.{ext}");
            File.Copy(path, newPath, true);
            path = newPath;
        }

        using var request = UnityWebRequestTexture.GetTexture(new Uri(path));
        await request.SendWebRequest();
        var texture = DownloadHandlerTexture.GetContent(request);
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));

        ProjectData.ImagePath = path;
        ProjectData.Image = sprite;
        
        return sprite;
    }

    public static void SaveProject(ProjectDto data)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(ProjectData.DataPath, json);
    }

    public static void LoadProject()
    {
        if (!File.Exists(ProjectData.DataPath))
        {
            return;
        }

        var json = File.ReadAllText(ProjectData.DataPath);
        var data = JsonUtility.FromJson<ProjectDto>(json);
        EventBus.OnProjectLoaded(data);
    }
}