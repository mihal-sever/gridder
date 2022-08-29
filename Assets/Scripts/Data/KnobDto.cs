using System;
using Sever.Gridder.Editor;

namespace Sever.Gridder.Data
{
    [Serializable]
    public class KnobDto
    {
        public bool isFinished;
        public KnobColor color;
        public float x;
        public float y;
    }
}