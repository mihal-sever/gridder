namespace Sever.Gridder.Data
{
    public struct ProjectSettings
    {
        public string name;
        public int canvasWidth;
        public int canvasHeight;
        public int gridStep;


        public bool IsNull()
        {
            return gridStep == default;
        }

        public static bool operator ==(ProjectSettings a, ProjectSettings b)
        {
            return a.name == b.name &&
                   a.canvasWidth == b.canvasWidth &&
                   a.canvasHeight == b.canvasHeight &&
                   a.gridStep == b.gridStep;
        }

        public static bool operator !=(ProjectSettings a, ProjectSettings b)
        {
            return a.name != b.name ||
                   a.canvasWidth != b.canvasWidth ||
                   a.canvasHeight != b.canvasHeight ||
                   a.gridStep != b.gridStep;
        }
    }
}