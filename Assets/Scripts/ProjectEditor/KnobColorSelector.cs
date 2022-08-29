using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sever.Gridder.Editor
{
    public enum KnobColor
    {
        Cyan,
        Pink,
        Yellow
    }

    public class KnobColorSelector : MonoBehaviour, IInitializable
    {
        private static readonly Dictionary<KnobColor, Sprite> _knobColors = new();
        private List<KnobColorButton> _buttons;
        private KnobColor _color;


        public void Init()
        {
            _buttons = GetComponentsInChildren<KnobColorButton>(true).ToList();
            _buttons.ForEach(x => x.Init(() => SelectColor(x.Color)));

            foreach (var button in _buttons)
            {
                _knobColors.Add(button.Color, button.Sprite);
            }

            SelectColor(KnobColor.Cyan);
        }

        public static Sprite GetSprite(KnobColor color) => _knobColors[color];

        private void SelectColor(KnobColor color)
        {
            if (_color == color)
            {
                return;
            }

            EditorController.SetKnobColor(color, _color);
            _color = color;
        }
    }
}