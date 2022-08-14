using UnityEngine.UI;

namespace Sever.Gridder.UI
{
    public class ProgressBar : BaseScreen, IInitializable
    {
        private static ProgressBar _instance;
        private static ProgressBar Instance => _instance ??= FindObjectOfType<ProgressBar>(true);

        private Scrollbar _scrollbar;


        public void Init()
        {
            _scrollbar = GetComponentInChildren<Scrollbar>(true);
        }

        public void OnDisable()
        {
            if (_scrollbar)
            {
                _scrollbar.size = 0;
            }
        }

        public static void UpdateProgress(float progress)
        {
            LeanTween.value(Instance.gameObject, Instance._scrollbar.size, progress, .1f).setOnUpdate(
                value =>
                {
                    Instance._scrollbar.size = value;
                });
        }
    }
}