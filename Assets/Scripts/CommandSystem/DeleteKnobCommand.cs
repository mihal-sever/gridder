using Sever.Gridder.Data;
using Sever.Gridder.Editor;

namespace Sever.Gridder.CommandSystem
{
    public class DeleteKnobCommand : ICommand
    {
        private KnobController _knobController;
        private KnobButton _knob;
        private KnobDto _knobDto;
        
        
        public DeleteKnobCommand(KnobController knobController, KnobButton knob = null)
        {
            _knob = knob == null ? knobController.LastSelectedKnob : knob;
            _knobController = knobController;
            _knobDto = _knob.GetKnobDto();
            
            CommandsManager.Add(this);
        }

        public void Execute()
        {
            _knobController.DeleteKnob(_knob);
        }

        public void Undo()
        {
            _knobController.AddKnob(_knobDto);
        }
    }
}