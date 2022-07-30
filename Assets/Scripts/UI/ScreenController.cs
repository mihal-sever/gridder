using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sever.UI
{
    public class ScreenController : MonoBehaviour
    {
        [SerializeField] private float _screenAnimationDuration = .5f;
        
        private static ScreenController _instance;
        public static ScreenController Instance => _instance ??= FindObjectOfType<ScreenController>(true);
        
        private static List<BaseScreen> _screens;
        private static BaseScreen _currentScreen;


        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _screens = GetComponentsInChildren<BaseScreen>(true).ToList();

            foreach (var screen in _screens)
            {
                screen.Setup(_screenAnimationDuration, ScreenPosition.Right, ScreenPosition.Left);
                screen.gameObject.SetActive(false);
            }

            OpenScreen<OpenFileScreen>();
        }

        public static void OpenScreen<T>() where T : BaseScreen
        {
            if (_currentScreen && _currentScreen.GetType() == typeof(T))
            {
                return;
            }

            if (_currentScreen)
            {
                _currentScreen.Close();
            }

            _currentScreen = _screens.First(x => x.GetType() == typeof(T));
            _currentScreen.Open();
        }

        public static T GetScreen<T>() where T : BaseScreen
        {
            return (T) _screens.First(x => x.GetType() == typeof(T));
        }
    }
}