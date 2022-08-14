using System.Collections.Generic;
using UnityEngine;

namespace Sever.Gridder.Data
{
    public class Project
    {
        public string Guid { get; }
        public string Name { get; private set; }

        public float CanvasWidth { get; private set; }
        public float CanvasHeight { get; private set; }
        public int GridStep { get; private set; }

        public float PixelsPerMm { get; set; }

        public Sprite Image { get; private set; }
        
        public List<KnobDto> Knobs { get; private set; }

        
        public Project(ProjectDto projectDto, Sprite image)
        {
            Guid = projectDto.guid;
            Name = projectDto.name;
            GridStep = projectDto.gridStep;
            CanvasWidth = projectDto.canvasWidth;
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

        public void UpdateSettings(string name, float canvasWidth, float canvasHeight, int gridStep)
        {
            Name = name;
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
            GridStep = gridStep;
        }

        public void UpdateKnobs(List<KnobDto> knobs)
        {
            Knobs = knobs;
        }
    }
}