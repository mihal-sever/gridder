using UnityEngine;

namespace Sever.Gridder.Data
{
    public struct ProjectSettings
    {
        public string name;
        public float canvasWidth;
        public float canvasHeight;
        public int gridStep;

        
        public bool IsNull()
        {
            return gridStep == default;
        }
        
        public static bool operator ==(ProjectSettings a, ProjectSettings b)
        {
            return a.name == b.name &&
                   Mathf.Abs(a.canvasWidth - b.canvasWidth) <= .0001f &&
                   Mathf.Abs(a.canvasHeight - b.canvasHeight) <= .0001f &&
                   a.gridStep == b.gridStep;
        }
        
        public static bool operator !=(ProjectSettings a, ProjectSettings b)
        {
            return a.name != b.name ||
                   Mathf.Abs(a.canvasWidth - b.canvasWidth) > .0001f ||
                   Mathf.Abs(a.canvasHeight - b.canvasHeight) > .0001f ||
                   a.gridStep != b.gridStep;
        }
    }
}