using System;
using Krivodeling.UI.Effects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sever.Gridder.UI
{
    [RequireComponent(typeof(UIBlur))]
    public class BlurOnHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _blurSpeed = 5f;

        private static Action<BlurOnHoverEffect> ElementHovered;

        private UIBlur _blur;


        private void Awake()
        {
            _blur = GetComponent<UIBlur>();
            ElementHovered += OnElementHovered;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ElementHovered?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ElementHovered?.Invoke(null);
        }

        private void OnElementHovered(BlurOnHoverEffect hoveredElement)
        {
            if (hoveredElement == this)
            {
                return;
            }

            if (!hoveredElement)
            {
                _blur.EndBlur(_blurSpeed);
                return;
            }

            _blur.BeginBlur(_blurSpeed);
        }
    }
}