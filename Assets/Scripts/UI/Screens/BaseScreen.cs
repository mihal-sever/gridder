using UnityEngine;

namespace Sever.Gridder.UI
{
    [RequireComponent(typeof(SliderScreen))]
    public abstract class BaseScreen : MonoBehaviour
    {
        private SliderScreen _sliderScreen;
        private SliderScreen SliderScreen => _sliderScreen ??= GetComponent<SliderScreen>();


        public void Setup(float animationDuration, ScreenPosition from, ScreenPosition to)
        {
            SliderScreen.Init(animationDuration, from, to);
        }

        public void Open()
        {
            SliderScreen.Open();
        }

        public void Close()
        {
            SliderScreen.Close();
        }
    }
}