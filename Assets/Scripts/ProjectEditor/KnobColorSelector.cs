using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
        public event Action<KnobColor> ColorSelected;

        public static readonly Dictionary<KnobColor, Sprite> KnobColors = new();
        private List<KnobColorButton> _buttons;


        public void Init()
        {
            _buttons = GetComponentsInChildren<KnobColorButton>(true).ToList();
            _buttons.ForEach(x => x.Init(() => ColorSelected?.Invoke(x.Color)));

            foreach (var button in _buttons)
            {
                KnobColors.Add(button.Color, button.Sprite);
            }
            
            ColorSelected?.Invoke(KnobColor.Cyan);
        }

        public static Sprite GetSprite(KnobColor color) => KnobColors[color];
    }
}