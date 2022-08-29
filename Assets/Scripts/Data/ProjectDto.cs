using System;
using System.Collections.Generic;

namespace Sever.Gridder.Data
{
    [Serializable]
    public class ProjectDto
    {
        public string guid;
        public ProjectSettings settings;
        public List<KnobDto> knobs;
    }
}