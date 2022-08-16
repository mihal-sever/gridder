using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.Gridder.Editor
{
    [RequireComponent(typeof(Button))]
    public class KnobColorButton : MonoBehaviour
    {
        [SerializeField] private KnobColor _color;
        [SerializeField] private GameObject _selectorIndicator;

        private static event Action<KnobColorButton> Selected;

        private Button _button;

        public KnobColor Color => _color;
        public Sprite Sprite => _button.image.sprite;


        public void Init(Action onSelected)
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(() =>
            {
                Selected?.Invoke(this);
                onSelected?.Invoke();
            });

            Selected += selectedButton => { _selectorIndicator.SetActive(selectedButton == this); };
        }
    }
}