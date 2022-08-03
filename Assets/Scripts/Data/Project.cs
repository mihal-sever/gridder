using System.Collections.Generic;
using System.Linq;
using GridMapper.Data;
using UnityEngine;

namespace GridMapper
{
    public class Project
    {
        public string Name { get; private set; }

        public float CanvasWidth { get; private set; }
        public float CanvasHeight { get; private set; }
        public int GridStep { get; private set; }

        public string ImageExtension { get; private set; }
        public Sprite Image { get; private set; }
        public float ImageWidth { get; private set; }
        public float ImageHeight { get; private set; }


        public List<KnobDto> Knobs { get; private set; }

        // public float PixelsPerMm;
        public string Guid { get; }


        public Project(ProjectDto projectDto, Sprite image)
        {
            Guid = projectDto.guid;
            Name = projectDto.name;
            GridStep = projectDto.gridStep;
            CanvasWidth = projectDto.canvasWidth;
            Knobs = projectDto.knobs;
            Image = image;

            SetImagePath(projectDto.imageExtension);
        }

        public Project(string imagePath, Sprite image)
        {
            Guid = System.Guid.NewGuid().ToString();
            Image = image;

            SetImagePath(imagePath);
        }

        public void UpdateImage(string imagePath, Sprite image, Vector2 size)
        {
            Image = image;
            ImageWidth = size.x;
            ImageHeight = size.y;

            SetImagePath(imagePath);
        }

        public void UpdateImageSize(Vector2 size)
        {
            ImageWidth = size.x;
            ImageHeight = size.y;
        }

        public void UpdateSettings(string name, float canvasWidth, float canvasHeight, int gridStep)
        {
            Name = name;
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
            GridStep = gridStep;
        }

        private void SetImagePath(string path)
        {
            if (path.Contains(Application.persistentDataPath))
            {
                ImageExtension = path;
                return;
            }

            var ext = path.Split('.').Last();
            ImageExtension = ext;
        }
    }
}