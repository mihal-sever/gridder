using System.Collections.Generic;
using Sever.Gridder.Data;
using Sever.Gridder.Editor;

namespace Sever.Gridder.CommandSystem
{
    public class DeleteAllKnobsCommand : ICommand
    {
        private KnobController _knobController;
        private List<KnobDto> _knobs;


        public DeleteAllKnobsCommand(KnobController knobController)
        {
            _knobController = knobController;
            _knobs = _knobController.GetKnobsDto();
        }

        public void Execute()
        {
            _knobController.DeleteAllKnobs();
        }

        public void Undo()
        {
            foreach (var knob in _knobs)
            {
                _knobController.AddKnob(knob);
            }
        }
    }
}