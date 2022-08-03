using System;
using UnityEngine;

namespace Sever.UI
{
    [RequireComponent(typeof(SliderScreen))]
    public abstract class BaseScreen : MonoBehaviour
    {
        public Action Exited;

        private SliderScreen _sliderScreen;
        private SliderScreen SliderScreen => _sliderScreen ??= GetComponentInChildren<SliderScreen>(true);
        

        protected abstract void Init();

        public void Setup(float animationDuration, ScreenPosition from, ScreenPosition to)
        {
            SliderScreen.Init(animationDuration, from, to);
            Init();
        }

        public void Open()
        {
            SliderScreen.Open(OnOpened);
        }

        protected virtual void OnOpened()
        {
        }

        public void Close()
        {
            SliderScreen.Close();
        }

        protected void Exit()
        {
            Close();
            Exited?.Invoke();
        }
    }
}