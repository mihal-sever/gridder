using System;
using System.Threading.Tasks;
using Krivodeling.UI.Effects;
using UnityEngine;


namespace Sever.Gridder.UI
{
    [RequireComponent(typeof(UIBlur))]
    public class BlurController : MonoBehaviour, IInitializable
    {
        [SerializeField] private float _blurInSpeed = 3f;
        [SerializeField] private float _blurOutSpeed = 6f;

        private static BlurController _instance;
        public static BlurController Instance => _instance ??= FindObjectOfType<BlurController>(true);

        private UIBlur _blur;
        

        public void Init()
        {
            _blur = GetComponent<UIBlur>();
        }

        public async Task Enable()
        {
            _blur.Intensity = 1;
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }

        public void Disable()
        {
            _blur.Intensity = 0;
        }

        public void Activate()
        {
            _blur.BeginBlur(_blurInSpeed);
        }

        public void Deactivate()
        {
            _blur.EndBlur(_blurOutSpeed);
        }
    }
}