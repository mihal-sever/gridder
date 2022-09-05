using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Sever.Gridder.UI
{
    public class ScreenController : MonoBehaviour, IInitializable
    {
        [SerializeField] private float _screenAnimationDuration = .5f;

        private static List<BaseScreen> _screens;
        private static BaseScreen _currentScreen;


        public void Init()
        {
            _screens = FindObjectsOfType<BaseScreen>(true).ToList();

            foreach (var screen in _screens)
            {
                screen.Setup(_screenAnimationDuration, ScreenPosition.Right, ScreenPosition.Left);
                screen.gameObject.SetActive(false);
            }
        }

        public static async Task OpenScreen<T>(float delay = 0) where T : BaseScreen
        {
            if (_currentScreen && _currentScreen.GetType() == typeof(T))
            {
                return;
            }

            await Task.Delay(TimeSpan.FromSeconds(delay));

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