using Sever.Gridder.Editor;
using UnityEngine;

namespace Sever.Gridder.CommandSystem
{
    public class AddKnobCommand : ICommand
    {
        private KnobController _knobController;
        private KnobButton _knob;
        private Vector3 _position;
        
        
        public AddKnobCommand(KnobController knobController, Vector3 position)
        {
            _knobController = knobController;
            _position = position;
        }

        public void Execute()
        {
            _knob = _knobController.AddKnob(_position);
        }

        public void Undo()
        {
            _knobController.DeleteKnob(_knob);
        }
    }
}