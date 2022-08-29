using Sever.Gridder.Editor;

namespace Sever.Gridder.CommandSystem
{
    public class SetKnobColorCommand : ICommand
    {
        private KnobColor _color;
        private KnobColor _prevColor;
        
        
        public SetKnobColorCommand(KnobColor color, KnobColor _previousColor)
        {
            _color = color;
            _prevColor = _previousColor;
        }

        public void Execute()
        {
            EventBus.OnColorSelected(_color);
        }

        public void Undo()
        {
            EventBus.OnColorSelected(_prevColor);
        }
    }
}