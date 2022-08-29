using Sever.Gridder.Editor;

namespace Sever.Gridder.CommandSystem
{
    public class SelectKnobCommand : ICommand
    {
        private KnobController _knobController;
        private KnobButton _knob;
        private KnobButton _lastSelectedKnob;
        
        
        public SelectKnobCommand(KnobController knobController, KnobButton knob)
        {
            _knobController = knobController;
            _knob = knob;
            _lastSelectedKnob = _knobController.LastSelectedKnob;
        }
        
        public void Execute()
        {
            _knobController.SelectKnob(_knob);
        }

        public void Undo()
        {
            _knobController.SelectKnob(_lastSelectedKnob);
        }
    }
}