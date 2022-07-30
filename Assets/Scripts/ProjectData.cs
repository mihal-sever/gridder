using System.IO;
using UnityEngine;

public static class ProjectData
{
    public static string Name;
    public static int GridStepMm;

    public static float PixelsPerMm;

    public static float CanvasWidth;
    public static float CanvasHeight;

    public static float ImageWidth;
    public static float ImageHeight;

    public static string ImagePath;
    public static Sprite Image;

    public static string DataPath = Path.Combine(Application.persistentDataPath, "knobsDto.json");
}