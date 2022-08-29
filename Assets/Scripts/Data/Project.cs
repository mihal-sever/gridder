using System.Collections.Generic;
using UnityEngine;

namespace Sever.Gridder.Data
{
    public class Project
    {
        public string Guid { get; }
        public string Name => Settings.name;
        public float CanvasWidth => Settings.canvasWidth;
        public float CanvasHeight => Settings.canvasHeight;
        public int GridStep => Settings.gridStep;

        public float PixelsPerMm { get; set; }
        public ProjectSettings Settings { get; private set; }
        public Sprite Image { get; private set; }
        public List<KnobDto> Knobs { get; private set; }

        
        public Project(ProjectDto projectDto, Sprite image)
        {
            Guid = projectDto.guid;
            Settings = projectDto.settings;
            Knobs = projectDto.knobs;
            Image = image;
        }

        public Project(Sprite image)
        {
            Guid = System.Guid.NewGuid().ToString();
            Image = image;
        }

        public void UpdateImage(Sprite image)
        {
            Image = image;
        }

        public void UpdateSettings(ProjectSettings settings)
        {
            Settings = settings;
        }

        public void UpdateKnobs(List<KnobDto> knobs)
        {
            Knobs = knobs;
        }
    }
}