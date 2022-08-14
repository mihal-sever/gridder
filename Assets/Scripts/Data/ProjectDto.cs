using System;
using System.Collections.Generic;

namespace Sever.Gridder.Data
{
    [Serializable]
    public class ProjectDto
    {
        public string guid;
        public string name;
        public int gridStep;
        public float canvasWidth;
        public List<KnobDto> knobs;
    }
}