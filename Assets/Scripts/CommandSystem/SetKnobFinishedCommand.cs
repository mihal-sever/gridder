using Sever.Gridder.Editor;

namespace Sever.Gridder.CommandSystem
{
    public class SetKnobFinishedCommand : ICommand
    {
        private KnobButton _knob;
        
        
        public SetKnobFinishedCommand(KnobButton knob)
        {
            _knob = knob;
        }

        public void Execute()
        {
            _knob.ToggleFinished();
        }

        public void Undo()
        {
            _knob.ToggleFinished();
        }
    }
}