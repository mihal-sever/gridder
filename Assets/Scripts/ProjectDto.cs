using System;
using System.Collections.Generic;

[Serializable]
public class ProjectDto
{
    public string imagePath;
    public int gridStepMm;
    public float canvasWidth;
    public List<KnobDto> knobs;
}